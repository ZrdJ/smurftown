using CommunityToolkit.Mvvm.ComponentModel;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class AccountsViewModel : ObservableObject
    {
        private static readonly BattlenetAccountGateway _battlenetAccountGateway = BattlenetAccountGateway.Instance;
        private IReadOnlyList<BattlenetAccount> _battlenetAccounts;

        public AccountsViewModel()
        {
            _battlenetAccountGateway.Reload();
            BattlenetAccounts = _battlenetAccountGateway.BattlenetAccounts;
        }

        public IReadOnlyList<BattlenetAccount> BattlenetAccounts
        {
            get { return _battlenetAccounts; }
            set => SetProperty(ref _battlenetAccounts, value);
        }
    }
}