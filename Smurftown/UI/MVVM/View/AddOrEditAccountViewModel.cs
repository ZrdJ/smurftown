using System.Windows.Input;
using MvvmDialogs;

namespace Smurftown.UI.MVVM.View;

public class AddOrEditAccountViewModel : Observable, IModalDialogViewModel
{
    private bool? dialogResult;
    private string? text;

    public AddOrEditAccountViewModel()
    {
        OkCommand = new RelayCommand((ignore) => Ok());
    }

    public string? Text
    {
        get => text;
        set
        {
            text = value;
            OnPropertyChanged();
        }
    }

    public ICommand OkCommand { get; }

    public bool? DialogResult
    {
        get => dialogResult;
        private set
        {
            dialogResult = value;
            OnPropertyChanged();
        }
    }

    private void Ok()
    {
        if (!string.IsNullOrEmpty(Text))
        {
            DialogResult = true;
        }
    }
}