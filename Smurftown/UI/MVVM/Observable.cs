using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Smurftown.UI.MVVM
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? callerMemberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerMemberName));
        }
    }
}