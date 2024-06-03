using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Smurftown.UI.MVVM.ViewModel
{
    class MainViewModel: Observable
    {
        public RelayCommand AccountsViewCommand { get; set; }
        public RelayCommand UsersViewCommand { get; set; }
        public AccountsViewModel AccountsVM { get; set; }
        public UsersViewModel UsersVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            AccountsVM = new AccountsViewModel();
            UsersVM = new UsersViewModel();
            CurrentView = AccountsVM;
            AccountsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountsVM;
            });
            UsersViewCommand = new RelayCommand(o =>
            {
                CurrentView = UsersVM;
            });
            merge();
        }

        private void merge()
        {
            // Load the images to tile (no need to specify PngBitmapDecoder, the correct decoder is automatically selected)
            BitmapFrame frame2 = BitmapDecoder.Create(new Uri("C:\\Users\\dammi\\source\\repos\\Smurftown\\Smurftown\\UI\\Images\\hots_full.png"), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
            BitmapFrame frame1 = BitmapDecoder.Create(new Uri("C:\\Users\\dammi\\source\\repos\\Smurftown\\Smurftown\\UI\\Images\\overwatch_full.png"), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();

            // Get the size of the images (assumes each image has the same size)
            int originalWidth = frame1.PixelWidth;
            int originalHeight = frame1.PixelHeight;
            int maxHeight = originalHeight; // Set the maximum height to the height of one image

            // Calculate the scaling factor to maintain aspect ratio
            double scaleFactor = Math.Min((double)maxHeight / originalHeight, 1.0);

            // Calculate the new dimensions of the images
            int newWidth = (int)(originalWidth * scaleFactor);
            int newHeight = (int)(originalHeight * scaleFactor);

            // Create a DrawingVisual component
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // Draw the first image at the top
                drawingContext.DrawImage(frame1, new Rect(0, 0, newWidth, newHeight));
                // Draw the second image below the first one
                drawingContext.DrawImage(frame2, new Rect(0, newHeight, newWidth, newHeight));
            }

            // The height of the combined image is the height of one scaled image
            int combinedHeight = newHeight * 2;

            // Convert the Visual (DrawingVisual) into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap(newWidth, combinedHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            // Create a PngBitmapEncoder and add the BitmapSource to the frames of the encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            // Save the image into a file using the encoder
            using (Stream stream = File.Create("C:\\Users\\dammi\\source\\repos\\Smurftown\\Smurftown\\UI\\Images\\overwatchhots_full.png"))
            {
                encoder.Save(stream);
            }
        }

        
    }
}
