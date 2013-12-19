using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AnimeWallPaper.Request;
using AnimeWallPaper.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AnimeWallPaper
{
    public partial class AnimeCategoryControl : UserControl
    {
        private AnimeCategoryControlViewModel _viewModel;
        private AnimeCategoryControl()
        {
            InitializeComponent();
            _viewModel = (AnimeCategoryControlViewModel)DataContext;
        }

        public AnimeCategoryControl(AnimeCategory info)
            : this()
        {
            _viewModel.GetImage(info.Tn_Url);
            _viewModel.Name = info.Name;
        }
    }
}
