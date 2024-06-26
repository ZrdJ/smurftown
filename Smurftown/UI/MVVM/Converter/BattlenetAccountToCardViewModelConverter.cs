﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Smurftown.Backend.Entity;
using Smurftown.UI.MVVM.ViewModel;

namespace Smurftown.UI.MVVM.Converter;

[ValueConversion(typeof(BattlenetAccount), typeof(AccountCardViewModel))]
class BattlenetAccountToCardViewModelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BattlenetAccount account)
        {
            return new AccountCardViewModel(account);
        }
        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountCardViewModel model)
        {
            return model.Account;
        }
        return DependencyProperty.UnsetValue;
    }
}