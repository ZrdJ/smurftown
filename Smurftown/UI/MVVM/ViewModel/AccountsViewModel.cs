using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smurftown.Backend.Gateway;
using Smurftown.UI.MVVM.View;

namespace Smurftown.UI.MVVM.ViewModel
{
    internal class AccountsViewModel : ObservableObject
    {
        private static readonly BattlenetAccountGateway _battlenetAccountGateway = BattlenetAccountGateway.Instance;
        private ICollectionView _battlenetAccounts;
        private RelayCommand _createAccountCommand;
        private bool _diabloFiltered;
        private bool _hotsFiltered;
        private bool _overwatchFiltered;
        private string _searchQuery;
        private bool _wowFiltered;

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

        public bool OverwatchFiltered
        {
            get => _overwatchFiltered;
            set => SetProperty(ref _overwatchFiltered, value);
        }

        public bool HotsFiltered
        {
            get => _hotsFiltered;
            set => SetProperty(ref _hotsFiltered, value);
        }

        public bool DiabloFiltered
        {
            get => _diabloFiltered;
            set => SetProperty(ref _diabloFiltered, value);
        }

        public bool WowFiltered
        {
            get => _wowFiltered;
            set => SetProperty(ref _wowFiltered, value);
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public ICollectionView BattlenetAccounts
        {
            get { return _battlenetAccounts; }
            set => SetProperty(ref _battlenetAccounts, value);
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


        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            _battlenetAccountGateway.FilterBy(SearchQuery, OverwatchFiltered, HotsFiltered, DiabloFiltered,
                WowFiltered);
        }
    }
}