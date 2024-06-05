using System.Collections.Immutable;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using Smurftown.Backend.Entity;

namespace Smurftown.Backend.Gateway
{
    public class WindowsAccountGateway
    {
        private const uint USER_PRIV_USER = 1;
        private const uint UF_SCRIPT = 1;
        private const uint UF_DONT_EXPIRE_PASSWD = 0x10000;
        public static readonly WindowsAccountGateway Instance = new();

        private SortedSet<WindowsUserAccount> _windowsAccounts;

        private WindowsAccountGateway()
        {
            _windowsAccounts = [..ReadWindowsAccounts()];
        }

        public IReadOnlyList<WindowsUserAccount> WindowsAccounts
        {
            get => _windowsAccounts.ToImmutableSortedSet();
        }

        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetUserAdd([MarshalAs(UnmanagedType.LPWStr)] string servername, uint level,
            ref USER_INFO_1 buf, out uint parm_err);

        public void Add(BattlenetAccount account)
        {
            var windowsAccount = ToWindowsAccount(account);
            var userCreated = CreateWindowsAccount(windowsAccount);
            if (userCreated) Reload();
        }

        private WindowsUserAccount ToWindowsAccount(BattlenetAccount account)
        {
            return new WindowsUserAccount
            {
                Name = ToWindowsUser(account),
                Password = ToWindowsUser(account)
            };
        }

        private List<WindowsUserAccount> ReadWindowsAccounts()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
            return (from ManagementObject user in searcher.Get()
                select new WindowsUserAccount
                {
                    Name = user["Name"].ToString(),
                    Password = user["Name"].ToString().ToLower()
                }).ToList();
        }

        private static bool CreateWindowsAccount(WindowsUserAccount account)
        {
            var userInfo = new USER_INFO_1
            {
                usri1_name = account.Name,
                usri1_password = account.Password,
                usri1_priv = USER_PRIV_USER,
                usri1_home_dir = null,
                usri1_comment = "New user account",
                usri1_flags = UF_SCRIPT | UF_DONT_EXPIRE_PASSWD,
                usri1_script_path = null
            };

            uint parm_err;
            var result = NetUserAdd(null, 1, ref userInfo, out parm_err);
            Console.WriteLine(result == 0 ? "User created successfully." : $"Error creating user: {result}");
            return result == 0;
        }

        public void Reload()
        {
            _windowsAccounts = [..ReadWindowsAccounts()];
        }

        private string ToWindowsUser(BattlenetAccount b)
        {
            return (b.Name + "-" + b.Discriminator).ToLower();
        }

        public void OpenBattlenet(BattlenetAccount account)
        {
            const string programPath = @"C:\Program Files (x86)\Battle.net\Battle.net.exe";
            var username = ToWindowsUser(account);
            var windowsUser = _windowsAccounts.Single(u => u.Name.Equals(username));
            const string domain = ".";

            // Create a new process start info
            var startInfo = windowsUser == null
                ? new ProcessStartInfo
                {
                    FileName = programPath,
                    UseShellExecute = true, // Must be false to use UserName and Password
                    LoadUserProfile = false // Optionally load the user profile
                }
                : new ProcessStartInfo
                {
                    FileName = programPath,
                    UserName = username,
                    Password = ConvertToSecureString(username),
                    Domain = domain,
                    UseShellExecute = false, // Must be false to use UserName and Password
                    LoadUserProfile = false // Optionally load the user profile
                };

            try
            {
                // Start the process with the specified user credentials
                using var process = Process.Start(startInfo);
                Console.WriteLine("Process started successfully.");
                //process?.WaitForExit(); // Optionally wait for the process to exit
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static System.Security.SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var securePassword = new System.Security.SecureString();
            foreach (char c in password)
                securePassword.AppendChar(c);
            securePassword.MakeReadOnly();
            return securePassword;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct USER_INFO_1
        {
            public string usri1_name;
            public string usri1_password;
            public uint usri1_password_age;
            public uint usri1_priv;
            public string usri1_home_dir;
            public string usri1_comment;
            public uint usri1_flags;
            public string usri1_script_path;
        }
    }
}