using Smurftown.Backend.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Smurftown.Backend.Gateway
{
    class WindowsAccountGateway
    {
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
        public WindowsAccountGateway()
        {
        }

        public void Add(WindowsUserAccount account)
        {
            
        }

        private List<WindowsUserAccount> ReadWindowsAccounts()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
            return (from ManagementObject user in searcher.Get()
                    select new WindowsUserAccount
                    {
                        Name = user["Name"].ToString(),
                        Password = user["Name"].ToString().ToLower()
                    }).ToList();
           
        }
        private void CreateWindowsAccount(WindowsUserAccount account)
        {
            USER_INFO_1 userInfo = new USER_INFO_1
            {
                usri1_name = "NewUser",
                usri1_password = "Password123",
                usri1_priv = USER_PRIV_USER,
                usri1_home_dir = null,
                usri1_comment = "New user account",
                usri1_flags = UF_SCRIPT | UF_DONT_EXPIRE_PASSWD,
                usri1_script_path = null
            };

            uint parm_err;
            int result = NetUserAdd(null, 1, ref userInfo, out parm_err);
            if (result == 0)
            {
                Console.WriteLine("User created successfully.");
            }
            else
            {
                Console.WriteLine($"Error creating user: {result}");
            }
        }
    }
}
