using System.Windows;
using Smurftown.Backend.Entity;

namespace Smurftown.UI.MVVM.ViewModel;

public class UserCardViewModel : Observable
{
    private WindowsUserAccountLinked? _user;
    private Visibility _hasAccountLinked = Visibility.Hidden;
    private string _username = null;
    private string _battletag = null;

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

    public UserCardViewModel(WindowsUserAccountLinked user) => User = user;

    public UserCardViewModel()
    {
    }
}