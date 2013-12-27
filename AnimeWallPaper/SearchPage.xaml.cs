using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AnimeWallPaper.Request;
using GoogleAds;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AnimeWallPaper
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private Dictionary<string,AnimeCategory> _animeDict= new Dictionary<string, AnimeCategory>();
        public SearchPage()
        {
            InitializeComponent();
            SearchControl.Search += SearchControl_OnSearch;
            CopyDict();
            LoadAd();
        }

        private void CopyDict()
        {
            foreach(var info in MainPage.ListCategory)
            {
                string name = RemoveAllSpace(info.Name).ToLower();
                if (_animeDict.ContainsKey(name))
                {
                    name = name + " ";
                }
                _animeDict.Add(name,info);
            }
        }

        private void SearchControl_OnSearch(string query)
        {
            Loading.Visibility = Visibility.Visible;
            var queryNoSpace = RemoveAllSpace(query);
            foreach (var animeCategory in _animeDict)
            {
                var name = animeCategory.Key;
                if (name.Contains(queryNoSpace))
                {
                    var control = new ImageControl(animeCategory.Value);
                    ResultPanel.Children.Add(control);
                }
            }
            Focus();
            Loading.Visibility = Visibility.Collapsed;
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
            bannerAd.SetValue(Grid.RowProperty, 4);
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

        private string RemoveAllSpace(string s)
        {
            return s.Replace(" ", "").Trim();
        }
    }
}