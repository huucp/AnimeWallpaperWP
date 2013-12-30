using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using AnimeWallPaper.Ultility;

namespace AnimeWallPaper
{
    public class GlobalFunctions
    {
        private static string _categoryFile = "categories.txt";
        public static string GenerateNameFromUrl(string url)
        {
            var generator = new FileNameFromURL(url);
            return generator.Convert();
        }
        public static string GetListCategories()
        {
            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(_categoryFile))
            {
                var fileStream = storage.OpenFile(_categoryFile, FileMode.Open);
                using (var reader = new StreamReader(fileStream))
                {
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }

        public static void SaveListCategories(string file)
        {
            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            var fileStream = storage.OpenFile(_categoryFile, FileMode.CreateNew);
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(file);
            }
        }

        public static void DisposeImage(BitmapImage image)
        {
            var uri = new Uri("Assets/oneXone.png", UriKind.Relative);
            StreamResourceInfo sr = Application.GetResourceStream(uri);
            try
            {
                using (Stream stream = sr.Stream)
                {
                    image.DecodePixelWidth = 1; //This is essential!
                    image.SetSource(stream);
                }
            }
            catch { }
        }

        public static int GetScrollViewerTopControlIndex(double verticalOfsset, double controlHeight)
        {
            int index = (int) (verticalOfsset/controlHeight)-1;
            if (index < 0) return 0;
            return index;
        }

        public static int GetScrollViewerBottomControlIndex(double verticalOfsset, double scrollViewerHeight,
            double controlHeight, int maxIndex)
        {
            int index = (int) ((verticalOfsset + scrollViewerHeight)/controlHeight) + 1;
            if (index > maxIndex) return maxIndex;
            return index;
        }
    }
}
