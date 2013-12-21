using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnimeWallPaper.Request
{
    public class GetCategoriesRequest : BaseRequest
    {
        public override string BuildRequest()
        {
            return RequestComponents.ServerUrl + RequestComponents.MethodGetCategory;
        }
        public override object ParseJson(string json)
        {
            return json;
            //var result = JsonConvert.DeserializeObject<GetAllAnimeJson>(json);
            //if (result == null || result.Stat != "ok")
            //{
            //    OnProcessError(null, "GetCategoriesRequest failed!");
            //    return null;
            //}            
            //return result.Result.Categories;
        }
    }
}
