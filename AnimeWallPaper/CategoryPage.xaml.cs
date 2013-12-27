using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                string name;
                if(NavigationContext.QueryString.TryGetValue("name", out name))
                {
                    Label.Text = name;
                }
                
                var request = new GetImagesOfAnimeRequest(id);
                request.ProcessSuccessfully += (data) =>
                {
                    var images = (ImagesJson)data;
                    if (data == null || images == null) return;
                    Dispatcher.BeginInvoke(() =>
                    {
                        List<ImageControl> lList= new List<ImageControl>();
                        List<ImageControl> rList= new List<ImageControl>();
                        for (int i = 0; i < images.Count; i += 2)
                        {
                            var lControl = new ImageControl(images._content[i]);
                            lList.Add(lControl);
                            if ((i + 1) == images.Count) break;
                            var rControl = new ImageControl(images._content[i + 1]);
                            rList.Add(rControl);
                        }
                        LeftPanel.ItemsSource = lList;
                        RightPanel.ItemsSource = rList;
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
                AdUnitID = GlobalVariables.AdId
            };
            bannerAd.ReceivedAd += OnAdReceived;
            bannerAd.FailedToReceiveAd += OnFailedToReceiveAd;
            bannerAd.SetValue(Grid.RowProperty, 3);
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

        private void LeftPanel_OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
        }
    }
}