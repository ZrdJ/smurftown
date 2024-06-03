using Smurftown.Backend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel: Observable
    {

        public BattlenetAccount? Account { get; }
        public AccountCardViewModel(BattlenetAccount account) => Account = account;

        public AccountCardViewModel() { }
    }
}
