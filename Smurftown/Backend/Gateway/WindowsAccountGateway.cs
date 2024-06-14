using System.Collections.Immutable;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Management;
using Smurftown.Backend.Entity;

namespace Smurftown.Backend.Gateway
{
    public class WindowsAccountGateway
    {
        public static readonly WindowsAccountGateway Instance = new();
        private readonly string _psExecExecutable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PsExec.exe");

        private SortedSet<WindowsUserAccount> _windowsAccounts;

        private WindowsAccountGateway()
        {
            _windowsAccounts = [..ReadWindowsAccounts()];
        }

        public IReadOnlyList<WindowsUserAccount> WindowsAccounts
        {
            get => _windowsAccounts.ToImmutableSortedSet();
        }

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
            using (var ctx = new PrincipalContext(ContextType.Machine))
            {
                var user = new UserPrincipal(ctx);
                user.SamAccountName = account.Name;
                user.SetPassword(account.Name);
                user.DisplayName = account.Name;
                user.Description = "Created by Smurftown";
                user.Save();
            }
        }

        public void Reload()
        {
            _windowsAccounts = [..ReadWindowsAccounts()];
        }

        private static string ToWindowsUser(BattlenetAccount b)
        {
            return (b.Name + b.Discriminator).ToLower();
        }

        public async Task OpenBattlenet(BattlenetAccount account)
        {
            var windowsUser = ToWindowsAccount(account);
            const string programPath = @"C:\Program Files (x86)\Battle.net\Battle.net.exe";
            var command = account.DedicatedWindowsUser
                ? $"{_psExecExecutable} -accepteula -u {windowsUser.Name} -p {windowsUser.Name} \"{programPath}\""
                : $"\"{programPath}\"";
            Console.WriteLine(command);
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true
            };

            using (var process = new Process())
            {
                process.StartInfo = startInfo;
                Console.WriteLine("Starting process");
                process.Start();

                Console.WriteLine("Waiting for 500 ms");
                await Task.Delay(500);

                // Kill the process if it's still running
                Console.WriteLine("Checking if process is finished: " + process.HasExited);
                if (!process.HasExited)
                {
                    Console.WriteLine("Killing process..");
                    process.Kill();
                    Console.WriteLine("Killing process.. DONE!");
                }
            }
        }
    }
}