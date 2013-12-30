using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;

namespace AnimeWallPaper
{
    public class LongListSelectorEx:LongListSelector
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            try
            {
                return base.MeasureOverride(availableSize);
            }
            catch (ArgumentException)
            {
                return this.DesiredSize;
            }
        }
    }
}
