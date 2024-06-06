﻿using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using Smurftown.Backend.Entity;

namespace Smurftown.Backend.Gateway
{
    public class WindowsAccountGateway
    {
        const int USER_PRIV_ADMIN = 2;
        private const uint USER_PRIV_USER = 1;
        private const uint UF_SCRIPT = 1;
        private const uint UF_DONT_EXPIRE_PASSWD = 0x10000;
        const int UF_NORMAL_ACCOUNT = 0x0200;
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
            CreateWindowsAccount(windowsAccount);
            Reload();
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

        private static void CreateWindowsAccount(WindowsUserAccount account)
        {
            var homeDirectory = Path.Combine(@"C:\Users", account.Name);

            var userInfo = new USER_INFO_1
            {
                usri1_name = account.Name,
                usri1_password = account.Password,
                usri1_priv = USER_PRIV_ADMIN,
                usri1_home_dir = homeDirectory,
                usri1_comment = "created by Smurftown",
                usri1_flags = UF_SCRIPT | UF_NORMAL_ACCOUNT | UF_DONT_EXPIRE_PASSWD,
                usri1_script_path = null
            };

            uint parm_err;
            var result = NetUserAdd(null, 1, ref userInfo, out parm_err);
            if (result != 0)
            {
                throw new InvalidOperationException(
                    "We were unable to create a windows user for this account.Please make sure to start this application as a administrator to avoid such errors.");
            }

            // Create the home directory if it does not exist
            if (!Directory.Exists(homeDirectory))
            {
                Directory.CreateDirectory(homeDirectory);
            }

            // Set the directory permissions to the new user
            SetHomeDirectoryPermissions(account.Name, homeDirectory);
        }

        static void SetHomeDirectoryPermissions(string username, string homeDirectory)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(homeDirectory);
                var directorySecurity = directoryInfo.GetAccessControl();

                // Add the user to the directory security
                var userSid =
                    new NTAccount(Environment.MachineName, username).Translate(typeof(SecurityIdentifier)) as
                        SecurityIdentifier;
                var accessRule = new FileSystemAccessRule(userSid,
                    FileSystemRights.FullControl,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                directorySecurity.AddAccessRule(accessRule);
                directoryInfo.SetAccessControl(directorySecurity);
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("Error setting home directory permissions: " + ex.Message);
            }
        }

        public void Reload()
        {
            _windowsAccounts = [..ReadWindowsAccounts()];
        }

        private static string ToWindowsUser(BattlenetAccount b)
        {
            return (b.Name + "-" + b.Discriminator).ToLower();
        }

        public void OpenBattlenet(BattlenetAccount account)
        {
            const string programPath = @"C:\Program Files (x86)\Battle.net\Battle.net.exe";
            var username = ToWindowsUser(account);
            var windowsUser = _windowsAccounts.FirstOrDefault(u => u.Name.Equals(username));
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
            foreach (var c in password)
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