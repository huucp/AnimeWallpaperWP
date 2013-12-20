using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using AnimeWallPaper.Request;

namespace AnimeWallPaper
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var request = new GetAllAnimeRequest();
            request.ProcessSuccessfully += (data) =>
            {
                var list = (List<AnimeCategory>)data;
                if (list == null) return;

                Dispatcher.BeginInvoke(() =>
                {                    
                    for (int i = 0; i <list.Count; i+=2)
                    {
                        var control = new ImageControl(list[i]);
                        LeftPanel.Children.Add(control);
                        if ((i+1)==list.Count) break;
                        var control2 = new ImageControl(list[i + 1]);
                        RightPanel.Children.Add(control2);                        
                    }
                    //for (int i = 20; i < 40; i++)
                    //{
                    //    var control = new ImageControl(list[i]);
                    //    RightPanel.Children.Add(control);
                    //}
                });

            };
            GlobalVariables.WorkerRequest.AddRequest(request);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}