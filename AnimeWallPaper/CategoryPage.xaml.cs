using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
                    });

                };
                GlobalVariables.WorkerRequest.AddRequest(request);
            }
        }
    }
}