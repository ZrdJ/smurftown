using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;
using Smurftown.UI.MVVM.View;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel : ObservableObject
    {
        private static readonly WindowsAccountGateway _windowsAccountGateway = WindowsAccountGateway.Instance;
        
        private BattlenetAccount? _account;

        private string _imageSource;


        private RelayCommand _openBattlenetCommand;
        private RelayCommand _openSettingsCommand;
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

        private Visibility _overwatch;
        private Visibility _hots;
        private Visibility _diablo;
        private Visibility _wow;

        public Visibility Overwatch
        {
            get { return _overwatch; }
            set { SetProperty(ref _overwatch , value); }
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

        private bool CanOpenBattlenet()
        {
            return true;
        }

        private void OpenBattlenet()
        {
            _windowsAccountGateway.OpenBattlenet(_account);
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
            var dialogViewModel = new AddOrEditAccountViewModel(_account!);

            var success = showDialog(dialogViewModel);
            dialogViewModel.Execute(success);
        }
    }
}