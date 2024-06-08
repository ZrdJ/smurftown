using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;
using Smurftown.UI.MVVM.View;
using ToastNotifications.Messages;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel : ObservableObject
    {
        private static readonly WindowsAccountGateway _windowsAccountGateway = WindowsAccountGateway.Instance;

        private BattlenetAccount? _account;
        private RelayCommand _copyPasswordCommand;
        private RelayCommand _copyUsernameCommand;
        private Visibility _diablo;
        private Visibility _hots;

        private string _imageSource;


        private RelayCommand _openBattlenetCommand;
        private RelayCommand _openSettingsCommand;

        private Visibility _overwatch;
        private Visibility _wow;

        public AccountCardViewModel(BattlenetAccount account)
        {
            Account = account;
            Overwatch = account.Overwatch ? Visibility.Visible : Visibility.Collapsed;
            Hots = account.Hots ? Visibility.Visible : Visibility.Collapsed;
            Diablo = account.Diablo ? Visibility.Visible : Visibility.Collapsed;
            Wow = account.Wow ? Visibility.Visible : Visibility.Collapsed;
        }

        public AccountCardViewModel()
        {
        }

        public Visibility Overwatch
        {
            get { return _overwatch; }
            set { SetProperty(ref _overwatch, value); }
        }

        public Visibility Hots
        {
            get { return _hots; }
            set { SetProperty(ref _hots, value); }
        }

        public Visibility Wow
        {
            get { return _wow; }
            set { SetProperty(ref _wow, value); }
        }

        public Visibility Diablo
        {
            get { return _diablo; }
            set { SetProperty(ref _diablo, value); }
        }

        public BattlenetAccount? Account
        {
            get { return _account; }
            set
            {
                _account = value;
                if (value.Overwatch && value.Hots)
                {
                    ImageSource = "pack://application:,,,/UI/Images/overwatchhots_full.png";
                }
                else
                {
                    ImageSource = value.Overwatch
                        ? "pack://application:,,,/UI/Images/overwatch_full.png"
                        : "pack://application:,,,/UI/Images/hots_full.png";
                }

                OnPropertyChanged();
            }
        }

        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public ICommand CopyPasswordCommand
        {
            get
            {
                if (_copyPasswordCommand == null)
                {
                    _copyPasswordCommand = new RelayCommand(
                        this.CopyPassword,
                        this.CanCopyPassword
                    );
                }

                return _copyPasswordCommand;
            }
        }

        public ICommand CopyUsernameCommand
        {
            get
            {
                if (_copyUsernameCommand == null)
                {
                    _copyUsernameCommand = new RelayCommand(
                        this.CopyUsername,
                        this.CanCopyUsername
                    );
                }

                return _copyUsernameCommand;
            }
        }

        public ICommand OpenBattlenetCommand
        {
            get
            {
                if (_openBattlenetCommand == null)
                {
                    _openBattlenetCommand = new RelayCommand(
                        this.OpenBattlenet,
                        this.CanOpenBattlenet
                    );
                }

                return _openBattlenetCommand;
            }
        }

        public ICommand OpenSettingsCommand
        {
            get
            {
                if (_openSettingsCommand == null)
                {
                    _openSettingsCommand = new RelayCommand(
                        OpenSettings,
                        CanOpenSettings
                    );
                }

                return _openSettingsCommand;
            }
        }

        private bool CanCopyPassword()
        {
            return true;
        }

        private void CopyPassword()
        {
            Clipboard.SetText(_account?.Password);
            Dialogs.Toast.ShowInformation("Password copied to clipboard");
        }

        private bool CanCopyUsername()
        {
            return true;
        }

        private void CopyUsername()
        {
            Clipboard.SetText(_account?.Email);
            Dialogs.Toast.ShowInformation("E-Mail copied to clipboard");
        }

        private bool CanOpenBattlenet()
        {
            return true;
        }

        private async void OpenBattlenet()
        {
            await Task.Run(() => _windowsAccountGateway.OpenBattlenet(_account));
        }

        private bool CanOpenSettings()
        {
            return true;
        }

        private void OpenSettings()
        {
            ShowDialog(viewModel => Dialogs.DialogService.ShowDialog(this, viewModel));
        }

        private void ShowDialog(Func<AddOrEditAccountViewModel, bool?> showDialog)
        {
            Application.Current.MainWindow!.Opacity = 0.4;
            var dialogViewModel = new AddOrEditAccountViewModel(_account!);

            var success = showDialog(dialogViewModel);
            Application.Current.MainWindow!.Opacity = 100;
            dialogViewModel.Execute(success);
        }
    }
}