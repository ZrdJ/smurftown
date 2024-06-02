using Smurftown.Backend;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountsViewModel: Observable
    {
        private readonly BattlenetAccountGateway _battlenetAccountGateway = new BattlenetAccountGateway();
        public readonly ObservableHashSet<BattlenetAccount> BattlenetAccounts;
        public AccountsViewModel() {
            BattlenetAccounts = _battlenetAccountGateway.BattlenetAccounts;
            OnPropertyChanged();
        }

    }
}
