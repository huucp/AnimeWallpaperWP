namespace AnimeWallPaper.Request
{
    public class RequestComponents
    {
        public const string ServerUrl = "http://anime.oohapp.com/quick_ws.php?format=json";
        public const string CategoryID = "&cat_id=";
        public const string ImageDerivativesType = "&derivatives=";
        public const string ImageTypeToSmall = "2small";
        public const string ImageTypeThumb = "thumb";
        public const string MethodCatGetImgs = "&method=pwg.categories.getImages";
        public const string MethodGetCategory = "&method=pwg.categories.getList";
        public const string MethodGetImageInfo = "&method=pwg.images.getInfo&derivatives=nothing";
        public const string MethodRateImage = "&method=pwg.images.rate";
        public const string PageNumber = "&page=";
        public const string ParamsImageID = "&image_id=";
        public const string ParamsOrderPopular = "&order=hit%20desc";
        public const string ParamsRate = "&rate=5";
        public const string PerPage = "&per_page=60";

    }
}
