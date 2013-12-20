using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnimeWallPaper.Request
{
    public class GetImagesOfAnimeRequest : BaseRequest
    {
        private string _id;
        public GetImagesOfAnimeRequest(string id)
        {
            _id = id;
        }
        public override string BuildRequest()
        {
            return RequestComponents.ServerUrl + RequestComponents.MethodCatGetImgs + RequestComponents.CategoryID + _id
                + RequestComponents.PerPage + "500";
        }
        public override object ParseJson(string json)
        {
            var result = JsonConvert.DeserializeObject<AnimeAllImagesJson>(json);
            if (result == null || result.Stat != "ok")
            {
                OnProcessError(null, "GetAllAnimeRequest failed!");
                return null;
            }
            return result.Result.Images;
        }
    }
}
