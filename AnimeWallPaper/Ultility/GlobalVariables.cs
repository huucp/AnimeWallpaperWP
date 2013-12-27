using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnimeWallPaper.Ultility;

namespace AnimeWallPaper
{
    public static class GlobalVariables
    {
        public static RequestWorker WorkerRequest = RequestWorker.Instance;
        public static ImageDownloader WorkerImage = ImageDownloader.Instance;
        public static LimitedItemImageDictionary ImageDictionary = new LimitedItemImageDictionary();
        public static string AdId = "a152b86315e5016";
    }
}
