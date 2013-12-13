using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeWallPaper
{
    public class RequestToServer
    {
        private const string CategoryID = "&cat_id=";
        private const string ImageDerivativesType = "&derivatives=";
        private const string ImageTypeToSmall = "2small";
        private const string ImageTypeThumb = "thumb";
        private const string MethodCatGetImgs = "&method=pwg.categories.getImages";
        private const string MethodGetCategory = "&method=pwg.categories.getList";
        private const string MethodGetImageInfo = "&method=pwg.images.getInfo&derivatives=nothing";
        private const string MethodRateImage = "&method=pwg.images.rate";
        private const string PageNumber = "&page=";
        private const string ParamsImageID = "&image_id=";
        private const string ParamsOrderPopular = "&order=hit%20desc";
        private const string ParamsRate = "&rate=5";
        private const string PerPage = "&per_page=60";

    }
}
