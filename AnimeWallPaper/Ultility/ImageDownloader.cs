using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AnimeWallPaper.Ultility
{
    public class ImageRequest
    {
        public delegate void DownloadSuccessfullyEventHandler(BitmapImage sender);

        public event DownloadSuccessfullyEventHandler DownloadCompleted;        

        public void OnDownloadCompleted(BitmapImage sender)
        {
            DownloadSuccessfullyEventHandler handler = DownloadCompleted;
            if (handler != null) handler(sender);
        }


        public delegate void DownloadErrorEventHandler(object sender, string msg);

        public event DownloadErrorEventHandler DownloadFailed;

        public void OnDownloadFailed(object sender, string msg)
        {
            DownloadErrorEventHandler handler = DownloadFailed;
            if (handler != null) handler(sender, msg);
        }


        private string ImageUrl { get; set; }
        public ImageRequest(string url)
        {
            ImageUrl = url;
        }

        public void Process()
        {
            string filename = GlobalFunctions.GenerateNameFromUrl(ImageUrl);
            if (GlobalVariables.ImageDictionary.Contain(filename))
            {
                OnDownloadCompleted(GlobalVariables.ImageDictionary.GetImage(filename));
                return;
            }
            DownloadRemoteImage(ImageUrl, filename);
        }

        private void DownloadRemoteImage(string imageUrl, string filename)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                OnDownloadFailed(null, "URL invalid");
                return;
            }
            DownloadImageFromUrl(imageUrl, filename);
        }

        private void DownloadImageFromUrl(string url, string filename)
        {
            Uri urlUri;
            try
            {
                urlUri = new Uri(url);
            }
            catch (Exception e)
            {
                OnDownloadFailed(null, e.Message);
                return;
            }
            var client = new WebClient();

            client.OpenReadCompleted +=
                delegate(object o, OpenReadCompletedEventArgs args)
                {
                    if (args.Error != null || args.Cancelled)
                    {
                        if (args.Error != null) OnDownloadFailed(null, args.Error.Message);
                        else OnDownloadFailed(null, "Cancel download");
                        return;
                    }
                    try
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            var bi = new BitmapImage();
                            //bi.DecodePixelWidth = 480;
                            bi.SetSource(args.Result);
                            OnDownloadCompleted(bi);
                            GlobalVariables.ImageDictionary.Add(filename, bi);
                        });
                    }
                    catch (Exception e)
                    {
                        OnDownloadFailed(e, e.Message);
                        throw;
                    }
                };
            client.OpenReadAsync(urlUri);
        }
    }

    /// <summary>
    /// Worker for download image
    /// </summary>
    public class ImageDownloader
    {
        private Thread backgroundWorker;

        private BlockingQueue<ImageRequest> ListsJobs = new BlockingQueue<ImageRequest>();


        private static readonly ImageDownloader _instance = new ImageDownloader();
        public static ImageDownloader Instance
        {
            get { return _instance; }
        }

        // Explicit static constructor to tell C# compiler not to mark type as BeforeFieldInit
        static ImageDownloader()
        {

        }

        private ImageDownloader()
        {
            backgroundWorker = new Thread(MainProcess)
            {
                IsBackground = true,
                Name = "Image Download Worker",
            };

            backgroundWorker.Start();
        }

        private int count = 0;
        public void AddDownload(ImageRequest request)
        {
            ListsJobs.Add(request);
            count++;
        }

        public void ClearAll()
        {
            ListsJobs.ClearAll();
        }

        private void MainProcess()
        {
            while (true)
            {                                
                var currentJob = ListsJobs.Get();
                count--;
                Debug.WriteLine("Remain {0} image download",count);
                if (currentJob == null) continue;
                currentJob.Process();
            }
        }
    }
}
