using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurftown.Backend.Entity
{
    class BattlenetAccount
    {
        private string _email;
        private string _name;

        public required string Name { get => _name; set => _name = value.ToUpper(); }
        public required string Discriminator { get; set; }
        public required string Email { get => _email; set => _email = value.ToLower(); }
        public required string Password { get; set; }
        public required Boolean Overwatch { get; set; }
        public required Boolean Hots { get; set; }
    }
}
