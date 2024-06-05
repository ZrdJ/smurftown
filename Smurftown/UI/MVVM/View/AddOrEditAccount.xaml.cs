using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Smurftown.UI.MVVM.View;

public partial class AddOrEditAccount : Window
{
    public AddOrEditAccount()
    {
        InitializeComponent();
    }

    private void DiscriminatorValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        var regex = MyRegex();
        e.Handled = regex.IsMatch(e.Text);
    }

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex MyRegex();

    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext != null)
        {
            ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}