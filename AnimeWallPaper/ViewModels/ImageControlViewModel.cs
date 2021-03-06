﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using AnimeWallPaper.Ultility;

namespace AnimeWallPaper.ViewModels
{
    public class ImageControlViewModel : ViewModelBase
    {
        private BitmapImage _imageSource = null;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                NotifyPropertyChanged("ImageSource");
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public bool ImageLoaded = false;

        private bool isLoading = false;

        public void GetImage(string url)
        {
            //if (isLoading) return;
            if (ImageSource != null) return;
            var imageDownload = new ImageRequest(url);
            imageDownload.DownloadCompleted += ImageDownloadDownloadCompleted;
            imageDownload.DownloadFailed += imageDownload_DownloadFailed;
            GlobalVariables.WorkerImage.AddDownload(imageDownload);
            //isLoading = true;
        }

        public void UnloadImage()
        {
            if (ImageSource == null) return;            
            Deployment.Current.Dispatcher.BeginInvoke(() => ImageSource = null);
            ImageLoaded = false;
        }

        private void imageDownload_DownloadFailed(object sender, string msg)
        {
            Debug.Assert(false, msg);
        }

        private void ImageDownloadDownloadCompleted(BitmapImage sender)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => ImageSource = sender);
            ImageLoaded = true;
            //isLoading = false;
        }
    }
}
