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
}
