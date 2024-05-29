using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Smurftown.UI.MVVM
{
    class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? callerMemberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerMemberName));
        }
    }
 }
