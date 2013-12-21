using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using AnimeWallPaper.Request;
using Newtonsoft.Json;

namespace AnimeWallPaper
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var json = GlobalFunctions.GetListCategories();
            if (json != string.Empty)
            {
                AddCategory(json);
            }
            else
            {
                var request = new GetCategoriesRequest();
                request.ProcessSuccessfully += (data) =>
                {
                    var dataString = (string)data;
                    if (data == null || dataString == json) return;

                    Dispatcher.BeginInvoke(() => AddCategory(dataString));

                    GlobalFunctions.SaveListCategories((string)data);
                };
                GlobalVariables.WorkerRequest.AddRequest(request);
            }
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

        private void AddCategory(string data)
        {
            var list = ParseCategory(data);
            for (int i = 0; i < list.Count; i += 2)
            {
                var control = new ImageControl(list[i]);
                LeftPanel.Children.Add(control);
                if ((i + 1) == list.Count) break;
                var control2 = new ImageControl(list[i + 1]);
                RightPanel.Children.Add(control2);
            }
        }
    }
}