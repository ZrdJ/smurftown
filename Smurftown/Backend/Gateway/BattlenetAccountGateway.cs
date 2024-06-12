using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using Smurftown.Backend.Entity;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Smurftown.Backend.Gateway
{
    public class BattlenetAccountGateway
    {
        public static readonly BattlenetAccountGateway Instance = new();

        private readonly string _configDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _configFileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".smurftown");
        private readonly string _configFile;

        private readonly IDeserializer _yamlIn = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        private readonly ISerializer _yamlOut = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        private BattlenetAccountGateway()
        {
            _configFile = Path.Combine(_configFileDirectory, "data.yaml");
            foreach (var account in ReadFromConfigFile())
            {
                BattlenetAccounts.Add(account);
            }

            BattlenetAccountsFiltered = CollectionViewSource.GetDefaultView(BattlenetAccounts);
            BattlenetAccountsFiltered.SortDescriptions.Add(
                new SortDescription(nameof(BattlenetAccount.LatestInteractionAt), ListSortDirection.Descending));
            BattlenetAccountsFiltered.SortDescriptions.Add(new SortDescription(nameof(BattlenetAccount.Name),
                ListSortDirection.Ascending));
        }


        public ObservableCollection<BattlenetAccount> BattlenetAccounts { get; set; } =
            new ObservableCollection<BattlenetAccount>();

        public ICollectionView BattlenetAccountsFiltered { get; set; }

        public static Predicate<T> Or<T>(Predicate<T> p1, Predicate<T> p2)
        {
            return x => p1(x) || p2(x);
        }

        private Predicate<BattlenetAccount> CreatePredicate(string searchQuery, bool overwatch, bool hots, bool diablo,
            bool wow)
        {
            return item =>
                (string.IsNullOrEmpty(searchQuery) || Contains(item, searchQuery)) &&
                (!overwatch || item.Overwatch) &&
                (!hots || item.Hots) &&
                (!diablo || item.Diablo) &&
                (!wow || item.Wow) ||
                (string.IsNullOrEmpty(searchQuery) && !overwatch && !hots && !diablo && !wow);
        }

        private bool Contains(BattlenetAccount account, string searchQuery)
        {
            var parts = new List<string> { account.Name, account.Discriminator, account.Email };
            return searchQuery.Split(" ")
                .All(word => parts.Any(part => part.Contains(word, StringComparison.OrdinalIgnoreCase)));
        }

        public void FilterBy(string searchQuery, bool overwatch, bool hots, bool diablo, bool wow)
        {
            var filter = CreatePredicate(searchQuery, overwatch, hots, diablo, wow);
            BattlenetAccountsFiltered.Filter = (obj) =>
            {
                if (obj is BattlenetAccount account)
                {
                    return filter?.Invoke(account) ?? true;
                }

                return false;
            };
        }

        public void AddOrUpdate(BattlenetAccount account)
        {
            BattlenetAccounts.Remove(account);
            BattlenetAccounts.Add(account);
            SaveToConfigFile();
        }

        public void Remove(BattlenetAccount account)
        {
            BattlenetAccounts.Remove(account);
            SaveToConfigFile();
        }

        private List<BattlenetAccount> ReadFromConfigFile()
        {
            ensureConfigFileExists();
            var content = File.ReadAllText(_configFile);
            var accountsFromList = _yamlIn.Deserialize<List<BattlenetAccount>>(new StringReader(content));
            return accountsFromList ?? ( []);
        }

        private void SaveToConfigFile()
        {
            ensureConfigFileExists();
            var content = _yamlOut.Serialize(BattlenetAccounts.AsEnumerable());
            File.WriteAllText(_configFile, content);
        }

        private void ensureConfigFileExists()
        {
            if (!File.Exists(_configFile))
            {
                Directory.CreateDirectory(_configFileDirectory);
                using (File.Create(_configFile)) { }
            }
        }

        public void Reload()
        {
            BattlenetAccounts.Clear();
            foreach (var battlenetAccount in ReadFromConfigFile()) BattlenetAccounts.Add(battlenetAccount);
        }

        public void UpdateInteraction(BattlenetAccount account)
        {
            account.LatestInteractionAt = DateTime.Now;
            AddOrUpdate(account);
        }
    }
}