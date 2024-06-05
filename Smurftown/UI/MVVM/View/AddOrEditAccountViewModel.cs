using System.Windows.Input;
using MvvmDialogs;

namespace Smurftown.UI.MVVM.View;

public class AddOrEditAccountViewModel : Observable, IModalDialogViewModel
{
    private bool? _dialogResult;
    private long? _discriminator;
    private string? _email;
    private bool _hotsChecked;
    private string? _name;
    private bool _overwatchChecked;
    private bool _saveButtonEnabled;

    public AddOrEditAccountViewModel()
    {
        OverwatchChecked = false;
        HotsChecked = false;
        SaveButtonEnabled = false;
        OkCommand = new RelayCommand((ignore) => Ok());
    }

    public bool SaveButtonEnabled
    {
        get { return _saveButtonEnabled; }
        set
        {
            if (value == _saveButtonEnabled) return;
            _saveButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public string? Email
    {
        get => _email;
        set
        {
            if (value == _email) return;
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
            if (value == _discriminator) return;
            _discriminator = value;
            OnPropertyChanged();
        }
    }

    public string? Name
    {
        get => _name;
        set
        {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public ICommand OkCommand { get; }

    public bool OverwatchChecked
    {
        get => _overwatchChecked;
        set
        {
            if (value == _overwatchChecked) return;
            _overwatchChecked = value;
            OnPropertyChanged();
        }
    }

    public bool HotsChecked
    {
        get => _hotsChecked;
        set
        {
            if (value == _hotsChecked) return;
            _hotsChecked = value;
            OnPropertyChanged();
        }
    }

    public bool? DialogResult
    {
        get => _dialogResult;
        private set
        {
            _dialogResult = value;
            OnPropertyChanged();
        }
    }

    protected override void OnPropertyChanged(string? callerMemberName = null)
    {
        base.OnPropertyChanged(callerMemberName);
        SaveButtonEnabled = !string.IsNullOrEmpty(Password)
                            && !string.IsNullOrEmpty(_email)
                            && !string.IsNullOrEmpty(_name)
                            && _discriminator > 0
                            && (_hotsChecked || _overwatchChecked);
    }

    private void Ok()
    {
        if (!string.IsNullOrEmpty(Name))
        {
            DialogResult = true;
        }
    }
}