using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smurftown.Backend;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;
using Smurftown.UI.MVVM.View;
using System.ComponentModel;
using System.Security.Principal;
using System.Windows.Input;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class AccountsViewModel : ObservableObject
    {
        private static readonly BattlenetAccountGateway _battlenetAccountGateway = BattlenetAccountGateway.Instance;
        private ICollectionView _battlenetAccounts;
        private bool _overwatchFiltered;
        private bool _hotsFiltered;
        private bool _wowFiltered;
        private bool _diabloFiltered;
        private RelayCommand _createAccountCommand;

        public AccountsViewModel()
        {
            _battlenetAccountGateway.Reload();
            BattlenetAccounts = _battlenetAccountGateway.BattlenetAccountsFiltered;
        }

        public ICommand CreateAccountCommand
        {
            get
            {
                if (_createAccountCommand == null)
                {
                    _createAccountCommand = new RelayCommand(
                        this.CreateAccount,
                        this.CanCreateAccount
                    );
                }

                return _createAccountCommand;
            }
        }

        private bool CanCreateAccount()
        {
            return true;
        }

        private void CreateAccount()
        {
            ShowDialog(viewModel => Dialogs.DialogService.ShowDialog(this, viewModel));
        }

        private void ShowDialog(Func<AddOrEditAccountViewModel, bool?> showDialog)
        {
            var dialogViewModel = new AddOrEditAccountViewModel(null);

            var success = showDialog(dialogViewModel);
            dialogViewModel.Execute(success);
        }

        public bool OverwatchFiltered { get => _overwatchFiltered; set => SetProperty(ref _overwatchFiltered, value); }

        public bool HotsFiltered { get => _hotsFiltered; set => SetProperty(ref _hotsFiltered, value); }

        public bool DiabloFiltered { get => _diabloFiltered; set => SetProperty(ref _diabloFiltered, value); }

        public bool WowFiltered { get => _wowFiltered; set => SetProperty(ref _wowFiltered, value); }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            _battlenetAccountGateway.FilterBy(OverwatchFiltered, HotsFiltered, DiabloFiltered, WowFiltered);
        }

        public ICollectionView BattlenetAccounts
        {
            get { return _battlenetAccounts; }
            set => SetProperty(ref _battlenetAccounts, value);
        }
    }
}