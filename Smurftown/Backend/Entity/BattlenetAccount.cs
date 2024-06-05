namespace Smurftown.Backend.Entity
{
    public class BattlenetAccount : IComparable<BattlenetAccount>
    {
        private string _email;
        private string _name;

        public required string Name
        {
            get => _name;
            set => _name = value.ToUpper();
        }

        public required string Discriminator { get; set; }

        public required string Email
        {
            get => _email;
            set => _email = value.ToLower();
        }

        public required string Password { get; set; }
        public required Boolean Overwatch { get; set; }
        public required Boolean Hots { get; set; }
        public required Boolean Wow { get; set; }
        public required Boolean Diablo { get; set; }

        public int CompareTo(BattlenetAccount? other)
        {
            return other == null ? 1 : string.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }

        public string Battletag()
        {
            return (Name + "#" + Discriminator).ToUpper();
        }

        private bool Equals(BattlenetAccount other)
        {
            return _email == other._email;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((BattlenetAccount)obj);
        }

        public override int GetHashCode()
        {
            return _email.GetHashCode();
        }
    }
}