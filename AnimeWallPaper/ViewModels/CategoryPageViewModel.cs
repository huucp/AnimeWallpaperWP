using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnimeWallPaper.Request;

namespace AnimeWallPaper.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        private ObservableCollection<ImageControl> _listImages = new ObservableCollection<ImageControl>();

        public ObservableCollection<ImageControl> ListImages
        {
            get { return _listImages; }
            set
            {
                _listImages = value;
                NotifyPropertyChanged("ListImages");
            }
        }

        public void GetImages(string id)
        {
            var request = new GetImagesOfAnimeRequest(id);
            request.ProcessSuccessfully += (data) =>
            {
                var images = (ImagesJson)data;
                if (data == null || images == null) return;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {                    
                    for (int i = 0; i <10; i ++)
                    {
                        var lControl = new ImageControl(images._content[i]);
                        _listImages.Add(lControl);                       
                    }                    
                    //Loading.Visibility = Visibility.Collapsed;
                });

            };
            GlobalVariables.WorkerRequest.AddRequest(request);
   
        }
    }
}
