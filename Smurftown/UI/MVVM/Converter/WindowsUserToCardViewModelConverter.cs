using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Smurftown.Backend.Entity;
using Smurftown.UI.MVVM.View;
using Smurftown.UI.MVVM.ViewModel;

namespace Smurftown.UI.MVVM.Converter;

[ValueConversion(typeof(WindowsUserAccount), typeof(UserCardViewModel))]
class WindowsAccountToCardViewModelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is WindowsUserAccount account)
        {
            return new UserCardViewModel(account);
        }
        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is UserCardViewModel model)
        {
            return model.User;
        }
        return DependencyProperty.UnsetValue;
    }
}