using CommunityToolkit.Mvvm.ComponentModel;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class UsersViewModel : ObservableObject
    {
        private static readonly WindowsAccountLinkedGateway _windowsAccountLinkedGateway =
            WindowsAccountLinkedGateway.Instance;

        private IReadOnlyList<WindowsUserAccountLinked> _windowsUserAccountsLinked;

        public UsersViewModel()
        {
            _windowsAccountLinkedGateway.Reload();
            WindowsAccountLinked = _windowsAccountLinkedGateway.WindowsAccountsLinked;
        }

        public IReadOnlyList<WindowsUserAccountLinked> WindowsAccountLinked
        {
            get { return _windowsUserAccountsLinked; }
            set
            {
                _windowsUserAccountsLinked = value;
                OnPropertyChanged();
            }
        }
    }
}