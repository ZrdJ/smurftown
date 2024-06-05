using System.Windows;
using System.Windows.Input;

namespace Smurftown.UI.MVVM.View;

public partial class AddOrEditAccount : Window
{
    public AddOrEditAccount()
    {
        InitializeComponent();
    }

    private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }
}