using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Smurftown.Backend.Entity;

namespace Smurftown.UI.MVVM.ViewModel;

public class UserCardViewModel : ObservableObject
{
    private string _battletag = null;
    private Visibility _hasAccountLinked = Visibility.Hidden;
    private WindowsUserAccountLinked? _user;
    private string _username = null;

    public UserCardViewModel(WindowsUserAccountLinked user) => User = user;

    public UserCardViewModel()
    {
    }

    public Visibility HasAccountLinked
    {
        get { return _hasAccountLinked; }
        set
        {
            _hasAccountLinked = value;
            OnPropertyChanged();
        }
    }

    public WindowsUserAccountLinked? User
    {
        get { return _user; }
        set
        {
            _user = value;
            HasAccountLinked = User.BattlenetAccount != null ? Visibility.Visible : Visibility.Hidden;
            Username = User.WindowsUserAccount.Name;
            Battletag = User.BattlenetAccount?.Battletag() ?? "";
            OnPropertyChanged();
        }
    }

    public string Username
    {
        get { return _username; }
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Battletag
    {
        get { return _battletag; }
        set
        {
            _battletag = value;
            OnPropertyChanged();
        }
    }
}