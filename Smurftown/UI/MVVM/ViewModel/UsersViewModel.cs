using Smurftown.Backend;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    class UsersViewModel: Observable
    {
        private readonly WindowsAccountGateway _windowsUserAccountGateway = new();
        private ObservableHashSet<WindowsUserAccount> _windowsUserAccounts;

        public ObservableHashSet<WindowsUserAccount> WindowsAccounts
        {
            get { return _windowsUserAccounts; }
            set { _windowsUserAccounts = value; OnPropertyChanged(); }
        }
        
        public UsersViewModel() {
            _windowsUserAccounts = _windowsUserAccountGateway.WindowsAccounts;
        }
    }
}
