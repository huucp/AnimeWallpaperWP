using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using AnimeWallPaper.Request;
using Newtonsoft.Json;

namespace AnimeWallPaper
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<AnimeCategory> _listCategory = new List<AnimeCategory>();
        private int _index = 0;
        private const int NumberOfCategoryAdded = 20;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var json = GlobalFunctions.GetListCategories();
            if (json != string.Empty)
            {
                _listCategory = ParseCategory(json);
                AddCategory();
            }
            else
            {
                var request = new GetCategoriesRequest();
                request.ProcessSuccessfully += (data) =>
                {
                    var dataString = (string)data;
                    if (data == null || dataString == json) return;
                    _listCategory = ParseCategory(json);
                    Dispatcher.BeginInvoke(AddCategory);              
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

        private void AddCategory()
        {
            if (_index >= _listCategory.Count) return;
            for (int i = _index; i < _index + NumberOfCategoryAdded; i += 2)
            {
                var control = new ImageControl(_listCategory[i]);
                LeftPanel.Children.Add(control);
                if ((i + 1) == _listCategory.Count) break;
                var control2 = new ImageControl(_listCategory[i + 1]);
                RightPanel.Children.Add(control2);
            }
            _index += NumberOfCategoryAdded;
        }

        private void MainScrollViewer_OnLayoutUpdated(object sender, EventArgs e)
        {
            if (Math.Abs(MainScrollViewer.ScrollableHeight - 0) > EPSILON && Math.Abs(MainScrollViewer.VerticalOffset - 0) > EPSILON && 
                (MainScrollViewer.ScrollableHeight - MainScrollViewer.VerticalOffset) <= 100)
            {
                AddCategory();
            }
        }

        protected double EPSILON
        {
            get { return 0.001; }
        }
    }
}