using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GoogleAds;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AnimeWallPaper.Request;

namespace AnimeWallPaper
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        public CategoryPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string id;
            if (e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.TryGetValue("id", out id))
            {
                LoadAd();
                var request = new GetImagesOfAnimeRequest(id);
                request.ProcessSuccessfully += (data) =>
                {
                    var images = (ImagesJson)data;
                    if (data == null || images == null) return;
                    Dispatcher.BeginInvoke(() =>
                    {
                        for (int i = 0; i < images.Count; i += 2)
                        {
                            var lControl = new ImageControl(images._content[i]);
                            LeftPanel.Children.Add(lControl);
                            if ((i + 1) == images.Count) break;
                            var rControl = new ImageControl(images._content[i + 1]);
                            RightPanel.Children.Add(rControl);
                        }
                        Loading.Visibility = Visibility.Collapsed;
                    });

                };
                GlobalVariables.WorkerRequest.AddRequest(request);
            }
        }

        private void LoadAd()
        {
            var bannerAd = new AdView
            {
                Format = AdFormats.SmartBanner,
                AdUnitID = "a152b86315e5016"
            };
            bannerAd.ReceivedAd += OnAdReceived;
            bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
            bannerAd.SetValue(Grid.RowProperty, 2);
            LayoutRoot.Children.Add(bannerAd);
            var adRequest = new AdRequest();
            bannerAd.LoadAd(adRequest);
        }

        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("Failed to receive ad with error " + e.ErrorCode);
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            Debug.WriteLine("Received ad successfully");
        }
    }
}