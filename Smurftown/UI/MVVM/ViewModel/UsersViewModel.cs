using Smurftown.Backend;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class UsersViewModel: Observable
    {
        private static readonly WindowsAccountLinkedGateway _windowsAccountLinkedGateway = WindowsAccountLinkedGateway.Instance;
        private ObservableHashSet<WindowsUserAccountLinked> _windowsUserAccountsLinked;

        public ObservableHashSet<WindowsUserAccountLinked> WindowsAccountLinked
        {
            get { return _windowsUserAccountsLinked; }
            set { _windowsUserAccountsLinked = value; OnPropertyChanged(); }
        }
        
        public UsersViewModel()
        {
            WindowsAccountLinked = _windowsAccountLinkedGateway.WindowsAccountsLinked;
            _windowsAccountLinkedGateway.Reload();
        }
    }
}
