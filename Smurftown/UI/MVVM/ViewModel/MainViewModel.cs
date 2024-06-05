using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Smurftown.UI.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private object? _currentView;

        public MainViewModel()
        {
            AccountsVM = new AccountsViewModel();
            UsersVM = new UsersViewModel();
            CurrentView = AccountsVM;
            AccountsViewCommand = new RelayCommand(() => { CurrentView = AccountsVM; });
            UsersViewCommand = new RelayCommand(() => { CurrentView = UsersVM; });
        }

        public RelayCommand AccountsViewCommand { get; set; }
        public RelayCommand UsersViewCommand { get; set; }
        public AccountsViewModel AccountsVM { get; set; }
        public UsersViewModel UsersVM { get; set; }

        public object? CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
    }
}