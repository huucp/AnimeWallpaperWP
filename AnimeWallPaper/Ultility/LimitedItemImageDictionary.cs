using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace AnimeWallPaper.Ultility
{
    public class LimitedItemImageDictionary
    {
        private List<KeyValuePair<string, BitmapImage>> _imageDictionary = new List<KeyValuePair<string, BitmapImage>>();
        private const int Limited = 40;

        public void Add(string key, BitmapImage image)
        {
            lock(_imageDictionary)
            {
                // If the list exceed its limit
                if (_imageDictionary.Count >= Limited)
                {
                    _imageDictionary.RemoveAt(0);
                }
                // Already in list
                if (Contain(key)) return;
                _imageDictionary.Add(new KeyValuePair<string, BitmapImage>(key, image));
            }
            
        }

        public bool Contain(string key)
        {
            lock (_imageDictionary)
            {
                return _imageDictionary.Any(item => item.Key == key);
            }                    
        }

        public BitmapImage GetImage(string key)
        {
            lock(_imageDictionary)
            {
                return (from item in _imageDictionary where item.Key == key select item.Value).FirstOrDefault();
            }
        }
    }
}
