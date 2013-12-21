using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AnimeWallPaper.Ultility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AnimeWallPaper
{
    public partial class ImageDetailPage : PhoneApplicationPage
    {
        public ImageDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string url;
            if (NavigationContext.QueryString.TryGetValue("url", out url))
            {
                var imageRequest = new ImageRequest(url);
                imageRequest.DownloadCompleted += (image) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        ImageContainer.Source = image;
                    });
                };
                GlobalVariables.WorkerImage.AddDownload(imageRequest);
            }
        }
    }
}