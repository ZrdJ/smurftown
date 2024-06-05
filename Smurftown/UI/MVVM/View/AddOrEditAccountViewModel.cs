using System.Windows;
using System.Windows.Input;
using MvvmDialogs;

namespace Smurftown.UI.MVVM.View;

public partial class AddOrEditAccountViewModel : Observable, IModalDialogViewModel
{
    private bool? _dialogResult;
    private long? _discriminator;
    private string? _email;
    private string? _name;
    private Visibility _saveButtonVisibility = Visibility.Hidden;

    public AddOrEditAccountViewModel()
    {
        OkCommand = new RelayCommand((ignore) => Ok());
    }

    public Visibility SaveButtonVisibility
    {
        get { return _saveButtonVisibility; }
        set
        {
            _saveButtonVisibility = value;
            OnPropertyChanged();
        }
    }

    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string Password { private get; set; }

    public long? Discrimnator
    {
        get => _discriminator;
        set
        {
            _discriminator = value;
            OnPropertyChanged();
        }
    }

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public ICommand OkCommand { get; }

    public bool? DialogResult
    {
        get => _dialogResult;
        private set
        {
            _dialogResult = value;
            OnPropertyChanged();
        }
    }

    private void Ok()
    {
        if (!string.IsNullOrEmpty(Name))
        {
            DialogResult = true;
        }
    }
}