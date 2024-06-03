using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurftown.Backend.Entity
{
    public class WindowsUserAccount
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? BattlenetEmail { get; set; }
    }
}
