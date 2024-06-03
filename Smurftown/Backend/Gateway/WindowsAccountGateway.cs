using System.Management;
using System.Runtime.InteropServices;
using Smurftown.Backend.Entity;

namespace Smurftown.Backend.Gateway
{
    public class WindowsAccountGateway
    {
        public static readonly WindowsAccountGateway Instance = new();
        
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

        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetUserAdd([MarshalAs(UnmanagedType.LPWStr)] string servername, uint level,ref USER_INFO_1 buf,out uint parm_err);

        private const uint USER_PRIV_USER = 1;
        private const uint UF_SCRIPT = 1;
        private const uint UF_DONT_EXPIRE_PASSWD = 0x10000;

        private readonly ObservableHashSet<WindowsUserAccount> _windowsAccounts = [];
        public ObservableHashSet<WindowsUserAccount> WindowsAccounts { get => _windowsAccounts; }
        private WindowsAccountGateway()
        {
            _windowsAccounts = new ObservableHashSet<WindowsUserAccount>(ReadWindowsAccounts());
        }

        public void Add(WindowsUserAccount account)
        {
            CreateWindowsAccount(account);
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
        private static void CreateWindowsAccount(WindowsUserAccount account)
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
        }

        public void Reload()
        {
            WindowsAccounts.Clear();
            foreach (var readWindowsAccount in ReadWindowsAccounts()) WindowsAccounts.Add(readWindowsAccount);
        }
    }
}
