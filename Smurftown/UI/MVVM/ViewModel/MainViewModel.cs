using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Smurftown.UI.MVVM.ViewModel
{
    class MainViewModel: Observable
    {
        public RelayCommand AccountsViewCommand { get; set; }
        public RelayCommand UsersViewCommand { get; set; }
        public AccountsViewModel AccountsVM { get; set; }
        public UsersViewModel UsersVM { get; set; }

        private object? _currentView;

        public object? CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            AccountsVM = new AccountsViewModel();
            UsersVM = new UsersViewModel();
            CurrentView = AccountsVM;
            AccountsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountsVM;
            });
            UsersViewCommand = new RelayCommand(o =>
            {
                CurrentView = UsersVM;
            });
            
        }  
    }
}
