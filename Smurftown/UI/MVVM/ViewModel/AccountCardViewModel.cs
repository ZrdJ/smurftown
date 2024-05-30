using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Smurftown.UI.MVVM.ViewModel
{
    class AccountCardViewModel: Observable
    {
        private ImageSource _imageSource;
        private string _cardText;

        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public string CardText
        {
            get => _cardText;
            set
            {
                _cardText = value;
                OnPropertyChanged();
            }
        }
    }
}
