﻿using Smurftown.UI.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Smurftown.UI.MVVM.View
{
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        public AccountsView()
        {
            InitializeComponent();
            var overwatchImage = new BitmapImage(new Uri("pack://application:,,,/UI/Images/overwatch_full.png")); ;

            var accountCardVM = new AccountCardViewModel()
            {
                ImageSource = overwatchImage,
                CardText = "Sample Text"
            };

            // Set the DataContext of the CardControl to the ViewModel
            accountCard.DataContext = accountCardVM;

        }
    }
}
