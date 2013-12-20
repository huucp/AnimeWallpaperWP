using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using AnimeWallPaper.Request;
using AnimeWallPaper.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AnimeWallPaper
{
    public partial class ImageControl : UserControl
    {
        private ImageControlViewModel _viewModel;
        private AnimeCategory _info;
        private ImageControl()
        {
            InitializeComponent();
            _viewModel = (ImageControlViewModel)DataContext;
        }

        private bool hasName;

        public ImageControl(AnimeCategory info)
            : this()
        {
            _info = info;
            _viewModel.GetImage(info.Tn_Url);
            _viewModel.Name = info.Name;
            hasName = true;
        }

        public ImageControl(ImageInfo info)
        {
            _viewModel.GetImage(info.Derivatives.XSmall.Url);
            hasName = false;
            NameBorder.Visibility=Visibility.Collapsed;
        }

        private void LayoutRoot_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (hasName)
            {
                var phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (phoneApplicationFrame != null)
                    phoneApplicationFrame.Navigate(new Uri("/CategoryPage.xaml?id=" + _info.ID, UriKind.Relative));
            }
        }
    }
}
