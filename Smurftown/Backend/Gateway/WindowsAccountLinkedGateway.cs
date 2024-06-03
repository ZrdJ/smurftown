using System.Collections;
using System.Collections.Specialized;
using Smurftown.Backend.Entity;

namespace Smurftown.Backend.Gateway;

public class WindowsAccountLinkedGateway
{
    public static readonly WindowsAccountLinkedGateway Instance = new();
    
    private readonly WindowsAccountGateway _windowsAccountGateway = WindowsAccountGateway.Instance;
    private readonly BattlenetAccountGateway _battlenetAccountGateway = BattlenetAccountGateway.Instance;

    public ObservableHashSet<WindowsUserAccountLinked> WindowsAccountsLinked { get; } = [];
    private ObservableHashSet<WindowsUserAccount> WindowsUserAccounts { get; set; }
    private ObservableHashSet<BattlenetAccount> BattlenetAccounts { get; set;  }

    private WindowsAccountLinkedGateway()
    {
        WindowsUserAccounts = _windowsAccountGateway.WindowsAccounts;
        WindowsUserAccounts.CollectionChanged += OnWindowsUserAccountsChanged;
        BattlenetAccounts = _battlenetAccountGateway.BattlenetAccounts;
        BattlenetAccounts.CollectionChanged += OnBattlenetAccountsChanged;
        
        Reset();
    }

    private void Reset()
    {
        var battlenetAccountsByBattletag = BattlenetAccounts.ToDictionary(ByBattletag);
        foreach (var windowsUserAccount in WindowsUserAccounts)
        {
            var battlenetAccount = battlenetAccountsByBattletag!.GetValueOrDefault(windowsUserAccount.Name.ToLower(), null);
            WindowsAccountsLinked.Add(new WindowsUserAccountLinked()
            {
                BattlenetAccount = battlenetAccount,
                WindowsUserAccount = windowsUserAccount
            });
        }
    }

    private void OnBattlenetAccountsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var windowsAccountsByBattletag = WindowsUserAccounts.ToDictionary(ByBattletag);
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var addedBattlenetAccount in (e.NewItems ?? new ArrayList()).Cast<BattlenetAccount>())
                {
                    var windowsAccount = windowsAccountsByBattletag!.GetValueOrDefault(ByBattletag(addedBattlenetAccount), null);
                    if (windowsAccount == null) continue;
                    var newWindowsUserAccountLinked = new WindowsUserAccountLinked()
                    {
                        BattlenetAccount = addedBattlenetAccount,
                        WindowsUserAccount = windowsAccount
                    };
                    WindowsAccountsLinked.Add(newWindowsUserAccountLinked);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (var removedBattlenetAccount in (e.NewItems ?? new ArrayList()).Cast<BattlenetAccount>())
                {
                    var windowsAccount = windowsAccountsByBattletag!.GetValueOrDefault(ByBattletag(removedBattlenetAccount), null);
                    if (windowsAccount == null) continue;
                    var newWindowsUserAccountLinked = new WindowsUserAccountLinked()
                    {
                        BattlenetAccount = null,
                        WindowsUserAccount = windowsAccount
                    };
                    WindowsAccountsLinked.Add(newWindowsUserAccountLinked);
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                // we dont care
                break;

            case NotifyCollectionChangedAction.Move:
                // we dont care
                break;

            case NotifyCollectionChangedAction.Reset:
                Reset();
                break;
        }
    }

    private void OnWindowsUserAccountsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var battlenetAccountsByBattletag = BattlenetAccounts.ToDictionary(ByBattletag);
        
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var addedWindowsUserAccount in (e.NewItems ?? new ArrayList()).Cast<WindowsUserAccount>())
                {
                    var battlenetAccount = battlenetAccountsByBattletag!.GetValueOrDefault(addedWindowsUserAccount.Name.ToLower(), null);
                    var newWindowsUserAccountLinked = new WindowsUserAccountLinked()
                    {
                        BattlenetAccount = battlenetAccount,
                        WindowsUserAccount = addedWindowsUserAccount
                    };
                    WindowsAccountsLinked.Add(newWindowsUserAccountLinked);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (var removedWindowsUserAccount in (e.OldItems ?? new ArrayList()).Cast<WindowsUserAccount>())
                {
                  
                    WindowsAccountsLinked.Remove(new WindowsUserAccountLinked()
                    {
                        BattlenetAccount = null,
                        WindowsUserAccount = removedWindowsUserAccount
                    });
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                foreach (var addedWindowsUserAccount in (e.NewItems ?? new ArrayList()).Cast<WindowsUserAccount>())
                {
                    var battlenetAccount = battlenetAccountsByBattletag!.GetValueOrDefault(addedWindowsUserAccount.Name.ToLower(), null);
                    var newWindowsUserAccountLinked = new WindowsUserAccountLinked()
                    {
                        BattlenetAccount = battlenetAccount,
                        WindowsUserAccount = addedWindowsUserAccount
                    };
                    WindowsAccountsLinked.Add(newWindowsUserAccountLinked);
                }
                break;

            case NotifyCollectionChangedAction.Move:
                // we dont care
                break;

            case NotifyCollectionChangedAction.Reset:
                Reset();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static string ByBattletag(BattlenetAccount b)
    {
        return (b.Name + "-" + b.Discriminator).ToLower();
    }
    
    private static string ByBattletag(WindowsUserAccount b)
    {
        return b.Name.ToLower();
    }
}