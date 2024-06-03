using Smurftown.Backend.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.IO;

namespace Smurftown.Backend.Gateway
{
    public class BattlenetAccountGateway
    {
        public static readonly BattlenetAccountGateway Instance = new();
        
        private readonly string _configDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _configFile;
        private readonly ObservableHashSet<BattlenetAccount> _battlenetAccounts;
        private readonly ISerializer _yamlOut = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        private readonly IDeserializer _yamlIn = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        public ObservableHashSet<BattlenetAccount> BattlenetAccounts { get => _battlenetAccounts; }
        private BattlenetAccountGateway()
        {
            _configFile = Path.Combine(_configDirectory, "data.yaml");
            _battlenetAccounts = new ObservableHashSet<BattlenetAccount>(ReadFromConfigFile());
        }

        public void AddOrUpdate(BattlenetAccount account)
        {
            _battlenetAccounts.Add(account);

            var content = _yamlOut.Serialize(_battlenetAccounts.AsEnumerable());
            File.WriteAllText(_configFile, content);
        }

        public void Remove(BattlenetAccount account)
        {
            _battlenetAccounts.Remove(account);
        }

        private List<BattlenetAccount> ReadFromConfigFile()
        {
            ensureConfigFileExists();
            var content = File.ReadAllText(_configFile);
            var accountsFromList = _yamlIn.Deserialize<List<BattlenetAccount>>(new StringReader(content));
            return accountsFromList ?? ([]);
        }
        private void SaveToConfigFile()
        {
            ensureConfigFileExists();
            var content = _yamlOut.Serialize(_battlenetAccounts.AsEnumerable());
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
            _battlenetAccounts.Clear();
            foreach (var battlenetAccount in ReadFromConfigFile()) _battlenetAccounts.Add(battlenetAccount);
        }
    }
}
