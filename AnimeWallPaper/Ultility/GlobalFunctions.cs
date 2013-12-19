using AnimeWallPaper.Ultility;

namespace AnimeWallPaper
{
    public class GlobalFunctions
    {
        public static string GenerateNameFromUrl(string url)
        {
            var generator = new FileNameFromURL(url);
            return generator.Convert();
        }
    }
}
