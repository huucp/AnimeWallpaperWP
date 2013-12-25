using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Nokia.Graphics.Imaging;
using Windows.Foundation;
using Point = System.Windows.Point;
using Rect = Windows.Foundation.Rect;

namespace AnimeWallPaper
{
    public partial class LockscreenEditingPage : PhoneApplicationPage
    {
        public LockscreenEditingPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var path = ImageDetailPage.EditingLockscreenFilename + ".jpg";
            var image = new BitmapImage();
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = new IsolatedStorageFileStream(path, FileMode.Open, storage))
                {
                    image.SetSource(stream);
                    ImageContainer.Height = height = image.PixelHeight;
                    ImageContainer.Width = width = image.PixelWidth;
                }
            }
            ImageContainer.Source = image;
            Debug.WriteLine("{0}-{1}", ImageContainer.Width, ImageContainer.Height);
            ReshapeImageContainer();
            Debug.WriteLine("{0}-{1}", ImageContainer.Width, ImageContainer.Height);
            CalculateWindowRect();
        }

        private bool _horizontalMove = true;
        private void CheckImageDirectionMove()
        {
            if (ResolutionHelper.CurrentResolution == Resolutions.WVGA ||
                ResolutionHelper.CurrentResolution == Resolutions.WXGA)
            {
                _horizontalMove = ((ImageContainer.Height / 15) < (ImageContainer.Width / 9));
                return;
            }
            if (ResolutionHelper.CurrentResolution == Resolutions.HD)
            {
                _horizontalMove = ((ImageContainer.Height / 16) < (ImageContainer.Width / 9));
                return;
            }
        }

        private double height, width;


        private void ReshapeImageContainer()
        {
            CheckImageDirectionMove();
            if (ResolutionHelper.CurrentResolution == Resolutions.WVGA ||
                ResolutionHelper.CurrentResolution == Resolutions.WXGA)
            {
                if (_horizontalMove)
                {
                    ImageContainer.Height = 600;
                    ImageContainer.Width = (ImageContainer.Height / height) * width;
                }
                else
                {
                    ImageContainer.Width = 360;
                    ImageContainer.Height = (ImageContainer.Width / width) * height;
                }


            }
            if (ResolutionHelper.CurrentResolution == Resolutions.HD)
            {
                if (_horizontalMove)
                {
                    ImageContainer.Height = 640;
                    ImageContainer.Width = (ImageContainer.Height / height) * width;
                }
                else
                {
                    ImageContainer.Width = 360;
                    ImageContainer.Height = (ImageContainer.Width / width) * height;
                }
            }
            ImageContainer.Stretch = Stretch.Fill;
            ImageContainer.HorizontalAlignment = HorizontalAlignment.Left;
            if (!_horizontalMove)
            {
                var margin = ImageContainer.Margin;
                margin.Left = CropLayout.ColumnDefinitions[0].ActualWidth;
                ImageContainer.Margin = margin;
            }
        }

        private void CalculateWindowRect()
        {
            if (ResolutionHelper.CurrentResolution == Resolutions.WVGA ||
                ResolutionHelper.CurrentResolution == Resolutions.WXGA)
            {
                MainRect.Height = 600;
            }
            if (ResolutionHelper.CurrentResolution == Resolutions.HD)
            {
                MainRect.Height = 640;
            }
        }

        private Point _origPoint;
        private void ImageContainer_OnMouseMove(object sender, MouseEventArgs e)
        {

            var curPoint = e.GetPosition(LayoutRoot);
            var margin = ImageContainer.Margin;
            if (_horizontalMove)
            {
                margin.Left += curPoint.X - _origPoint.X;

                if (margin.Left > CropLayout.ColumnDefinitions[0].ActualWidth)
                {
                    margin.Left = CropLayout.ColumnDefinitions[0].ActualWidth;
                }
                if (margin.Left + ImageContainer.Width < CropLayout.ColumnDefinitions[0].ActualWidth + MainRect.Width)
                {
                    margin.Left = CropLayout.ColumnDefinitions[0].ActualWidth + MainRect.Width - ImageContainer.Width;
                }
            }
            else
            {
                margin.Top += curPoint.Y - _origPoint.Y;
                if (margin.Top > CropLayout.RowDefinitions[0].ActualHeight)
                {
                    margin.Top = CropLayout.RowDefinitions[0].ActualHeight;
                }
                if (margin.Top + ImageContainer.Height < CropLayout.RowDefinitions[0].ActualHeight + MainRect.Height)
                {
                    margin.Top = CropLayout.RowDefinitions[0].ActualHeight + MainRect.Height - ImageContainer.Height;
                }
            }

            ImageContainer.Margin = margin;
            _origPoint = curPoint;
        }

        private void ImageContainer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _origPoint = e.GetPosition(LayoutRoot);
        }
        private Rect CalculateCropRect()
        {
            return new Rect(CropLayout.ColumnDefinitions[0].ActualWidth - ImageContainer.Margin.Left, 0, MainRect.Width, MainRect.Height);
        }
        private Rect ReframeRect(Rect rect, double para)
        {
            Rect r;
            r.X = rect.X * para;
            r.Y = rect.Y * para;
            r.Width = rect.Width * para;
            r.Height = rect.Height * para;
            return r;
        }
        private async void CropButton_OnClick(object sender, EventArgs e)
        {
            var cropRect = CalculateCropRect();
            cropRect = ReframeRect(cropRect, height / ImageContainer.Height);
            var path = ImageDetailPage.EditingLockscreenFilename + ".jpg";
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = new IsolatedStorageFileStream(path, FileMode.Open, storage))
                {
                    var imageStream = new StreamImageSource(stream);
                    var effect = new FilterEffect(imageStream)
                    {
                        Filters = new[] { new ReframingFilter(cropRect, 0) }
                    };
                    var cartoonImageBitmap = new WriteableBitmap((int)MainRect.Width, (int)MainRect.Height);
                    var renderer = new WriteableBitmapRenderer(effect, cartoonImageBitmap);
                    cartoonImageBitmap = await renderer.RenderAsync();

                    // preview
                    //ImageContainer.Width = MainRect.Width;
                    //ImageContainer.Height = MainRect.Height;
                    //ImageContainer.Margin = new Thickness(CropLayout.ColumnDefinitions[0].Width.Value, CropLayout.RowDefinitions[0].Height.Value, 0, 0);
                    //ImageContainer.Source = cartoonImageBitmap;

                    WriteImageToLocal(cartoonImageBitmap);

                    SetLockScreen();

                    if(NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    }
                }
            }


        }

        private string LockscreenFilename
        {
            get
            {
                string name;
                if (!AppSettings.TryGetSetting("LockscreenFilename", out name))
                {
                    return "lockscreen";
                }
                if (name == "lockscreen")
                {
                    return "lockscreen1";
                }
                return "lockscreen";
            }
        }

        private void WriteImageToLocal(WriteableBitmap image)
        {
            string path = LockscreenFilename + ".jpg";
            // Create virtual store and file stream. Check for duplicate tempJPEG files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(path))
                {
                    myIsolatedStorage.DeleteFile(path);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(path);

                // Encode WriteableBitmap object to a JPEG stream.
                image.SaveJpeg(fileStream, image.PixelWidth, image.PixelHeight, 0, 100);

                fileStream.Close();
            }
        }

        private async void SetLockScreen()
        {
            try
            {
                var isProvider = Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication;

                if (!isProvider)
                {
                    var permission = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();

                    isProvider = permission == Windows.Phone.System.UserProfile.LockScreenRequestResult.Granted;
                }

                if (isProvider)
                {
                    var uri = new Uri("ms-appdata:///Local/" + LockscreenFilename + ".jpg", UriKind.Absolute);

                    Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);
                    AppSettings.StoreSetting("LockscreenFilename", LockscreenFilename);
                }
                else
                {
                    MessageBox.Show("Couldn't update the lock screen picture.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the lock screen picture: " + ex.Message);
            }
        }
    }
}