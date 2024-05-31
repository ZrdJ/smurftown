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
    class BattlenetAccountGateway
    {
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
        public BattlenetAccountGateway()
        {
            _configFile = Path.Combine(_configDirectory, "data.yaml");
            _battlenetAccounts = new ObservableHashSet<BattlenetAccount>(readFromConfigFile());
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

        private List<BattlenetAccount> readFromConfigFile()
        {
            ensureConfigFileExists();
            var content = File.ReadAllText(_configFile);
            return _yamlIn.Deserialize<List<BattlenetAccount>>(new StringReader(content));
        }
        private void saveToConfigFile()
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
    }
}
