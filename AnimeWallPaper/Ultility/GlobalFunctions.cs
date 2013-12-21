using System.IO;
using System.IO.IsolatedStorage;
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
    }
}
