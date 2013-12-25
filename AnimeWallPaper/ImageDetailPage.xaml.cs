using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AnimeWallPaper.Ultility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;

namespace AnimeWallPaper
{
    public partial class ImageDetailPage : PhoneApplicationPage
    {
        public ImageDetailPage()
        {
            InitializeComponent();
        }

        private bool _canPress = false;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string url;
            if (NavigationContext.QueryString.TryGetValue("url", out url))
            {
                var imageRequest = new ImageRequest(url);
                imageRequest.DownloadCompleted += (image) => Dispatcher.BeginInvoke(() =>
                {
                    ImageContainer.Source = image;
                    SaveEditingLockscreen(image);
                    Dispatcher.BeginInvoke(() =>
                                               {
                                                   Loading.Visibility = Visibility.Collapsed;
                                               });
                    _canPress = true;
                });
                GlobalVariables.WorkerImage.AddDownload(imageRequest);
            }
        }

        private void SaveButton_OnClick(object sender, EventArgs e)
        {
            if (!_canPress) return;
            string path = EditingLockscreenFilename + ".jpg";
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var stream = storage.OpenFile(path, FileMode.Open);
                var library = new MediaLibrary();
                library.SavePicture("test", stream);
                stream.Close();
            }
        }

        private void LockscreenButton_OnClick(object sender, EventArgs e)
        {
            if (_canPress)
            {
                NavigationService.Navigate(new Uri("/LockscreenEditingPage.xaml", UriKind.Relative));
            }
        }

        private void SaveEditingLockscreen(BitmapImage bitmapImage)
        {
            string path = EditingLockscreenFilename + ".jpg";
            // Create virtual store and file stream. Check for duplicate tempJPEG files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(path))
                {
                    myIsolatedStorage.DeleteFile(path);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(path);

                // Encode WriteableBitmap object to a JPEG stream.
                var image = new WriteableBitmap(bitmapImage);
                image.SaveJpeg(fileStream, image.PixelWidth, image.PixelHeight, 0, 100);

                fileStream.Close();
            }
        }

        public static string EditingLockscreenFilename
        {
            get { return "EditingLockscreen"; }
        }
    }
}