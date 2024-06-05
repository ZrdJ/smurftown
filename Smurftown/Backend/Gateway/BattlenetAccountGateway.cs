using System.Collections.Immutable;
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
        private readonly string _configFile;

        private readonly IDeserializer _yamlIn = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        private readonly ISerializer _yamlOut = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        private BattlenetAccountGateway()
        {
            _configFile = Path.Combine(_configDirectory, "data.yaml");
            foreach (var account in ReadFromConfigFile())
            {
                BattlenetAccounts.Add(account);
            };
            BattlenetAccountsFiltered = CollectionViewSource.GetDefaultView(BattlenetAccounts);
            BattlenetAccountsFiltered.SortDescriptions.Add(new SortDescription(nameof(BattlenetAccount.Name), ListSortDirection.Ascending));
        }

        public void FilterBy(bool overwatch, bool hots, bool diablo, bool wow)
        {
           
            BattlenetAccountsFiltered.Filter = (obj) =>
            {
                if (obj is BattlenetAccount account)
                {
                    
                    return overwatch ? account.Overwatch : true 
                        && hots ? account.Hots : true
                        && wow ? account.Wow : true
                        && diablo ? account.Diablo : true
                        ;
                }
                return false;
            };
        }
       

        public ObservableCollection<BattlenetAccount> BattlenetAccounts
        {
            get; set;
        } = new ObservableCollection<BattlenetAccount>();

        public ICollectionView BattlenetAccountsFiltered
        {
            get; set;
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
                File.Create(_configFile);
            }
        }

        public void Reload()
        {
            BattlenetAccounts.Clear();
            foreach (var battlenetAccount in ReadFromConfigFile()) BattlenetAccounts.Add(battlenetAccount);
        }
    }
}