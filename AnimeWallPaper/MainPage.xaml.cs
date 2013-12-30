using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GoogleAds;
using Microsoft.Phone.Controls;
using AnimeWallPaper.Request;
using Newtonsoft.Json;

namespace AnimeWallPaper
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static List<AnimeCategory> ListCategory = new List<AnimeCategory>();
        private int _index = 0;
        private const int NumberOfCategoryAdded = 20;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private List<AnimeCategory> ParseCategory(string json)
        {
            var result = JsonConvert.DeserializeObject<GetAllAnimeJson>(json);
            if (result == null || result.Stat != "ok")
            {
                return null;
            }
            return result.Result.Categories;
        }

        private void AddCategory()
        {
            if (_index >= ListCategory.Count) return;
            int maxIndex = Math.Min(_index + NumberOfCategoryAdded, ListCategory.Count);
            for (int i = _index; i < maxIndex; i += 2)
            {
                var control = new ImageControl(ListCategory[i]);
                LeftPanel.Children.Add(control);
                if ((i + 1) == ListCategory.Count) break;
                var control2 = new ImageControl(ListCategory[i + 1]);
                RightPanel.Children.Add(control2);
            }
            _index = maxIndex;
        }


        private void MainScrollViewer_OnLayoutUpdated(object sender, EventArgs e)
        {
            if (LeftPanel.Children.Count > NumberOfCategoryAdded / 2)
            {
                int topIndex = GlobalFunctions.GetScrollViewerTopControlIndex(MainScrollViewer.VerticalOffset, 156);
                int botIndex = GlobalFunctions.GetScrollViewerBottomControlIndex(MainScrollViewer.VerticalOffset,
                    MainScrollViewer.ActualHeight, 156, LeftPanel.Children.Count - 1);
                VitualizeContent(topIndex, botIndex);
            }

            // Incremental loading
            if (Math.Abs(MainScrollViewer.ScrollableHeight - 0) > EPSILON && Math.Abs(MainScrollViewer.VerticalOffset - 0) > EPSILON &&
                (MainScrollViewer.ScrollableHeight - MainScrollViewer.VerticalOffset) <= 200)
            {
                Loading.Visibility = Visibility.Visible;                
                AddCategory();
                Loading.Visibility = Visibility.Collapsed;
            }
        }       

        private void VitualizeContent(int topIndex, int botIndex)
        {
            const int reservedIndex = 5;
            if (topIndex > reservedIndex) ReleaseBeforeTop(topIndex - reservedIndex);
            int minIndex = Math.Min(LeftPanel.Children.Count, RightPanel.Children.Count);
            if (botIndex < minIndex - reservedIndex) ReleaseAfterBot(botIndex + reservedIndex, minIndex);
            LoadOnViewportImages(topIndex - reservedIndex, botIndex + reservedIndex, minIndex);
        }

        private void LoadOnViewportImages(int topIndex, int bottomIndex, int maxIndex)
        {
            int top = topIndex < 0 ? 0 : topIndex;
            int bot = bottomIndex > maxIndex ? maxIndex - 1 : bottomIndex;
            Debug.WriteLine("load from {0} to {1} - maxIdex: {2}", top, bot, maxIndex);
            for (int i = top; i < bot; i++)
            {
                var leftControl = LeftPanel.Children[i] as ImageControl;
                if (leftControl != null && !leftControl.HasImage)
                {
                    leftControl.LoadImage();
                }
                var rightControl = RightPanel.Children[i] as ImageControl;
                if (rightControl != null && !rightControl.HasImage)
                {
                    rightControl.LoadImage();
                }
            }
        }

        private void ReleaseAfterBot(int index, int maxIndex)
        {
            for (int i = index; i < maxIndex; i++)
            {
                var leftControl = LeftPanel.Children[i] as ImageControl;
                if (leftControl != null && leftControl.HasImage)
                {
                    leftControl.UnloadImage();
                }
                var rightControl = RightPanel.Children[i] as ImageControl;
                if (rightControl != null && rightControl.HasImage)
                {
                    rightControl.UnloadImage();
                }
            }
        }

        private void ReleaseBeforeTop(int index)
        {
            for (int i = 0; i < index; i++)
            {
                var leftControl = LeftPanel.Children[i] as ImageControl;
                if (leftControl != null && leftControl.HasImage)
                {
                    leftControl.UnloadImage();
                }
                var rightControl = RightPanel.Children[i] as ImageControl;
                if (rightControl != null && rightControl.HasImage)
                {
                    rightControl.UnloadImage();
                }
            }
        }

        protected double EPSILON
        {
            get { return 0.001; }
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadAd();
            var json = GlobalFunctions.GetListCategories();
            if (json != string.Empty)
            {
                ListCategory = ParseCategory(json);
                AddCategory();
                Loading.Visibility = Visibility.Collapsed;
            }
            else
            {
                var request = new GetCategoriesRequest();
                request.ProcessSuccessfully += (data) =>
                {
                    var dataString = (string)data;
                    if (data == null || dataString == json) return;
                    ListCategory = ParseCategory(dataString);
                    Dispatcher.BeginInvoke(delegate()
                    {
                        AddCategory();
                        Loading.Visibility = Visibility.Collapsed;
                    });
                    GlobalFunctions.SaveListCategories(dataString);
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

        private void SearchButton_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }
    }
}