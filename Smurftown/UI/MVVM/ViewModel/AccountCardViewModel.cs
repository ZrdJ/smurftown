using System.Windows.Input;
using Smurftown.Backend.Entity;
using Smurftown.Backend.Gateway;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel : Observable
    {
        private static readonly WindowsAccountGateway _windowsAccountGateway = WindowsAccountGateway.Instance;
        private BattlenetAccount? _account;

        private string _imageSource;


        private RelayCommand _openBattlenetCommand;
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

        private bool CanOpenBattlenet()
        {
            return true;
        }

        private void OpenBattlenet()
        {
            _windowsAccountGateway.OpenBattlenet(_account);
        }
    }
}