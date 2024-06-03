using Smurftown.Backend;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class AccountsViewModel: Observable
    {
        private readonly BattlenetAccountGateway _battlenetAccountGateway = BattlenetAccountGateway.Instance;
        private ObservableHashSet<BattlenetAccount> _battlenetAccounts;

        public ObservableHashSet<BattlenetAccount> BattlenetAccounts

        {
            get { return _battlenetAccounts; }
            set { _battlenetAccounts = value; OnPropertyChanged(); }
        }
        
        public AccountsViewModel() {
           _battlenetAccounts = _battlenetAccountGateway.BattlenetAccounts;
        }
    }
}
