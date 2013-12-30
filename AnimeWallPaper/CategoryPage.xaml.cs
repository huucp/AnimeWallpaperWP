using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using AnimeWallPaper.ViewModels;
using GoogleAds;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AnimeWallPaper.Request;

namespace AnimeWallPaper
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        private CategoryPageViewModel _viewModel;
        public CategoryPage()
        {
            InitializeComponent();
            _viewModel = (CategoryPageViewModel) DataContext;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string id;
            if (e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.TryGetValue("id", out id))
            {
                LoadAd();
                _viewModel.GetImages(id);
                string name;
                if (NavigationContext.QueryString.TryGetValue("name", out name))
                {
                    Label.Text = name;
                }
                
                
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

        int count =0;
        
        private void LongListSelector_OnItemUnrealized(object sender, ItemRealizationEventArgs e)
        {
            var control = e.Container.Content as ImageControl;
            if (control != null)
            {
                control.UnloadImage();
                Debug.WriteLine("unload");
            }
        }

        private void LongListSelector_OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var control = e.Container.Content as ImageControl;
            if (control != null)
            {
                control.LoadImage();
                Debug.WriteLine("load");
            }
        }
    }
}