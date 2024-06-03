using System.Windows;
using Smurftown.Backend.Entity;

namespace Smurftown.UI.MVVM.ViewModel;

public class UserCardViewModel : Observable
{
    public Visibility HasAccountLinked
    {
        get { return _hasAccountLinked; }
        set
        {
            _hasAccountLinked = value;
            OnPropertyChanged();
        }
    }

    private WindowsUserAccount? _user;
    private Visibility _hasAccountLinked = Visibility.Hidden;

    public WindowsUserAccount? User
    {
        get { return _user; }
        set
        {
            _user = value;
            HasAccountLinked = User.BattlenetEmail != null ? Visibility.Visible : Visibility.Hidden;
            OnPropertyChanged();
        }
    }

    public UserCardViewModel(WindowsUserAccount user) => User = user;

    public UserCardViewModel()
    {
    }
}