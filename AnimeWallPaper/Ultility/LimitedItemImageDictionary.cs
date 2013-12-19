using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace AnimeWallPaper.Ultility
{
    public class LimitedItemImageDictionary
    {
        private List<KeyValuePair<string, BitmapImage>> _imageDictionay = new List<KeyValuePair<string, BitmapImage>>();
        private const int Limited = 40;

        public void Add(string key, BitmapImage image)
        {
            // If the list exceed its limit
            if (_imageDictionay.Count >= Limited)
            {
                _imageDictionay.RemoveAt(0);
            }
            // Already in list
            if (Contain(key)) return;
            _imageDictionay.Add(new KeyValuePair<string, BitmapImage>(key, image));
        }

        public bool Contain(string key)
        {
            var dictionay = new List<KeyValuePair<string, BitmapImage>>(_imageDictionay);
            return dictionay.Any(item => item.Key == key);
        }

        public BitmapImage GetImage(string key)
        {
            foreach (var item in _imageDictionay)
            {
                if (item.Key == key) return item.Value;
            }
            return null;
        }
    }
}
