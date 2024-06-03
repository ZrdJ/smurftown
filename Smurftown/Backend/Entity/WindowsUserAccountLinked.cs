namespace Smurftown.Backend.Entity
{
    public class WindowsUserAccountLinked
    {
        public required WindowsUserAccount WindowsUserAccount { get; set; }

        public required BattlenetAccount? BattlenetAccount { get; set; }

        protected bool Equals(WindowsUserAccountLinked other)
        {
            return WindowsUserAccount.Equals(other.WindowsUserAccount);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WindowsUserAccountLinked)obj);
        }

        public override int GetHashCode()
        {
            return WindowsUserAccount.GetHashCode();
        }
    }
}
