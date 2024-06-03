namespace Smurftown.Backend.Entity
{
    public class WindowsUserAccount
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? BattlenetEmail { get; set; }

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
