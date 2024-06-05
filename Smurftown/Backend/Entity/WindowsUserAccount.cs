namespace Smurftown.Backend.Entity
{
    public class WindowsUserAccount : IComparable<WindowsUserAccount>
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? BattlenetEmail { get; set; }

        public int CompareTo(WindowsUserAccount? other)
        {
            return other == null ? 1 : string.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }

        private bool Equals(WindowsUserAccount other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((WindowsUserAccount)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}