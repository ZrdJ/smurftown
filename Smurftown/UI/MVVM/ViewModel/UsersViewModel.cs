using Smurftown.Backend;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class UsersViewModel: Observable
    {
        private static readonly WindowsAccountLinkedGateway _windowsAccountLinkedGateway = WindowsAccountLinkedGateway.Instance;
        private IReadOnlyList<WindowsUserAccountLinked> _windowsUserAccountsLinked;

        public IReadOnlyList<WindowsUserAccountLinked> WindowsAccountLinked
        {
            get { return _windowsUserAccountsLinked; }
            set { _windowsUserAccountsLinked = value; OnPropertyChanged(); }
        }
        
        public UsersViewModel()
        {
            _windowsAccountLinkedGateway.Reload();
            WindowsAccountLinked = _windowsAccountLinkedGateway.WindowsAccountsLinked;
        }
    }
}
