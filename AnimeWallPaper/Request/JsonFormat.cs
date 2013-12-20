using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnimeWallPaper.Request
{
    #region get all anime request
    public class AnimeCategory
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Nb_Images { get; set; }
        public string Total_Nb_Images { get; set; }
        public string Tn_Url { get; set; }
    }

    public class ResultGetAllAnime
    {
        public List<AnimeCategory> Categories { get; set; }
    }

    public class GetAllAnimeJson
    {
        public string Stat { get; set; }
        public ResultGetAllAnime Result { get; set; }
    }
    #endregion

    #region get images of anime
    public class DerivativeType
    {
        public string Url { get; set; }
    }
    public class LargeImageType
    {
        public string Url { get; set; }
    }
    public class DerivativesType
    {
        public DerivativeType XSmall { get; set; }
        public DerivativeType Large { get; set; }
    }

    public class ImageInfo
    {
        public string ID { get; set; }
        public string Hit { get; set; }
        public string Element_Url { get; set; }
        public DerivativesType Derivatives { get; set; }
    }

    public class ImagesJson
    {
        public int Page { get; set; }
        public int Per_Page { get; set; }
        public int Count { get; set; }
        public List<ImageInfo> _content { get; set; }
    }

    public class ResultAnimeAllImages
    {
        public ImagesJson Images { get; set; }
    }
    public class AnimeAllImagesJson
    {
        public string Stat { get; set; }
        public ResultAnimeAllImages Result { get; set; }
    }
    #endregion
}
