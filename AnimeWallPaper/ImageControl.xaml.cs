using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AnimeWallPaper.Request;
using AnimeWallPaper.ViewModels;
using Microsoft.Phone.Controls;

namespace AnimeWallPaper
{
    public partial class ImageControl : UserControl
    {
        private ImageControlViewModel _viewModel;
        private AnimeCategory _categoryInfo;
        private ImageInfo _imageInfo;
        private ImageControl()
        {
            InitializeComponent();
            _viewModel = (ImageControlViewModel)DataContext;
        }

        private bool hasName;

        public ImageControl(AnimeCategory info)
            : this()
        {
            _categoryInfo = info;
            _viewModel.GetImage(info.Tn_Url);
            _viewModel.Name = info.Name;
            hasName = true;
        }

        public ImageControl(ImageInfo info)
            : this()
        {
            _viewModel.GetImage(info.Derivatives.XSmall.Url);
            hasName = false;
            NameBorder.Visibility = Visibility.Collapsed;
            _imageInfo = info;
        }

        private void LayoutRoot_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (phoneApplicationFrame == null) return;
            if (hasName)
            {
                phoneApplicationFrame.Navigate(new Uri("/CategoryPage.xaml?id=" + _categoryInfo.ID, UriKind.Relative));
            }
            else
            {
                phoneApplicationFrame.Navigate(new Uri("/ImageDetailPage.xaml?url=" + _imageInfo.Derivatives.Large.Url,
                                                       UriKind.Relative));
            }
        }
    }
}
