using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurftown.UI.MVVM
{
    class Dialogs
    {
        public static readonly IDialogService DialogService = new DialogService();
    }
}
