using System.Linq;
using System.Text.RegularExpressions;

namespace AnimeWallPaper.Ultility
{
    public class FileNameFromURL
    {
        string Url = "";
        public FileNameFromURL(string convertingURL)
        {
            Url = convertingURL;
        }

        public string Convert()
        {
            var r = new Regex(@"[a-zA-Z0-9]", RegexOptions.IgnoreCase);
            var x = (from Match m in r.Matches(Url) select m.Value).ToList();
            return x.Aggregate("", (current, t) => current + t);
        }
    }
}
