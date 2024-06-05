using System.Windows.Input;
using MvvmDialogs;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;
using Smurftown.UI.MVVM.View;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel : Observable
    {
        private static readonly WindowsAccountGateway _windowsAccountGateway = WindowsAccountGateway.Instance;
        private static readonly IDialogService _dialogService = new DialogService();
        private BattlenetAccount? _account;

        private string _imageSource;


        private RelayCommand _openBattlenetCommand;
        private RelayCommand _openSettingsCommand;
        public AccountCardViewModel(BattlenetAccount account) => Account = account;

        public AccountCardViewModel()
        {
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
                        param => this.OpenBattlenet(),
                        param => this.CanOpenBattlenet()
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
                        param => this.OpenSettings(),
                        param => this.CanOpenSettings()
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
            ShowDialog(viewModel => _dialogService.ShowDialog(this, viewModel));
        }

        private void ShowDialog(Func<AddOrEditAccountViewModel, bool?> showDialog)
        {
            var dialogViewModel = new AddOrEditAccountViewModel(_account!);

            var success = showDialog(dialogViewModel);
            dialogViewModel.Execute(success);
        }
    }
}