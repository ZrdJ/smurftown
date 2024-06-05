using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Smurftown.UI.MVVM.View;
using System.Security.Principal;
using System.Windows;
using ToastNotifications.Messages;

namespace Smurftown.UI.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private object? _currentView;

        public MainViewModel()
        {
            RegisterExceptionHandler();
            AccountsVM = new AccountsViewModel();
            UsersVM = new UsersViewModel();
            CurrentView = AccountsVM;
            AccountsViewCommand = new RelayCommand(() => { CurrentView = AccountsVM; });
            UsersViewCommand = new RelayCommand(() => { CurrentView = UsersVM; });
        }

        private void RegisterExceptionHandler()
        {
            if (Application.Current != null)
            {
                Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
              
            }
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log and/or display the exception
            //ShowErrorDialog(viewModel => Dialogs.DialogService.ShowDialog(this, viewModel), e.Exception);
            Dialogs.Toast.ShowError(e.Exception.Message);
            //Dialogs.DialogService.ShowMessageBox(this, caption: "An error occured", messageBoxText: e.Exception.Message);
            //MessageBox.Show($"{e.Exception.Message}", "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true; // Prevent the application from crashing
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log and/or display the exception
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Unhandled domain exception: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Log and/or display the exception
            MessageBox.Show($"Unobserved task exception: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.SetObserved(); // Prevent the exception from terminating the application
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

        private void ShowErrorDialog(Func<ErrorBoxViewModel, bool?> showDialog, Exception error)
        {
            var dialogViewModel = new ErrorBoxViewModel(error);
           showDialog(dialogViewModel); 
        }
    }
}