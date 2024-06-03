﻿using Smurftown.Backend.Entity;

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
                if (value.Overwatch && value.Hots)
                {
                    ImageSource =  "pack://application:,,,/UI/Images/overwatchhots_full.png";
                } else
                {
                    ImageSource = value.Overwatch ? "pack://application:,,,/UI/Images/overwatch_full.png" : "pack://application:,,,/UI/Images/hots_full.png";
                }
                
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
