using Smurftown.Backend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel: Observable
    {

        private BattlenetAccount? _account;

        public BattlenetAccount? Account
        {
            get { return _account; }
            set { 
                _account = value;
                ImageSource = value.Overwatch ? "pack://application:,,,/UI/Images/overwatch_full.png" : "pack://application:,,,/UI/Images/hots_full.png";
                OnPropertyChanged(); 
            }
        }

        private string _imageSource;

        public string ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }


        public AccountCardViewModel(BattlenetAccount account) => Account = account;

        public AccountCardViewModel() { }
    }
}
