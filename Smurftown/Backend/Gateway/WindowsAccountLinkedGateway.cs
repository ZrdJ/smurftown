using Smurftown.Backend.Entity;

namespace Smurftown.Backend.Gateway;

public class WindowsAccountLinkedGateway
{
    public static readonly WindowsAccountLinkedGateway Instance = new();
    
    private readonly WindowsAccountGateway _windowsAccountGateway = WindowsAccountGateway.Instance;
    private readonly BattlenetAccountGateway _battlenetAccountGateway = BattlenetAccountGateway.Instance;

    private List<WindowsUserAccountLinked> _windowsUserAccountsLinked = [];

    public IReadOnlyList<WindowsUserAccountLinked> WindowsAccountsLinked { get => _windowsUserAccountsLinked.AsReadOnly(); }

    private WindowsAccountLinkedGateway()
    {
        Reset();
    }

    private void Reset()
    {
        _windowsAccountGateway.Reload();
        _battlenetAccountGateway.Reload();
        _windowsUserAccountsLinked.Clear();
        var windowsAccounts = _windowsAccountGateway.WindowsAccounts;
        var battlenetAccounts = _battlenetAccountGateway.BattlenetAccounts;
        var battlenetAccountsByBattletag = battlenetAccounts.ToDictionary(ByBattletag);
        foreach (var windowsUserAccount in windowsAccounts)
        {
            var battlenetAccount = battlenetAccountsByBattletag!.GetValueOrDefault(windowsUserAccount.Name.ToLower(), null);
            _windowsUserAccountsLinked.Add(new WindowsUserAccountLinked()
            {
                BattlenetAccount = battlenetAccount,
                WindowsUserAccount = windowsUserAccount
            });
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

    public void Reload()
    {
        Reset();
    }
}