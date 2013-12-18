using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeWallPaper.Request
{
    public class BaseRequest
    {
        public delegate void SuccessfullyEventHandler(object sender);

        public event SuccessfullyEventHandler ProcessSuccessfully;

        public void OnProcessSuccessfully(object sender)
        {
            SuccessfullyEventHandler handler = ProcessSuccessfully;
            if (handler != null) handler(sender);
        }

        public delegate void ErrorEventHandler(object sender, string msg);

        public event ErrorEventHandler ProcessError;

        public void OnProcessError(object sender, string msg)
        {
            ErrorEventHandler handler = ProcessError;
            if (handler != null) handler(sender, msg);
        }

        public void Process()
        {
            string request = BuildRequest();
            if (request == string.Empty)
            {
                OnProcessError(null, "BuildRequest error");
                return;
            }
            DownloadContent(request);
        }

        private void DownloadContent(string request)
        {
            var client = new WebClient();
            client.DownloadStringCompleted += DownloadStringCallback;
            client.DownloadStringAsync(new Uri(request));
        }

        private void DownloadStringCallback(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnProcessError(null, e.Error.Message);
                return;
            }
            else if (!e.Cancelled)
            {
                var result = ParseJson(WebUtility.HtmlDecode(e.Result));
                OnProcessSuccessfully(result);
            }
        }

        public virtual string BuildRequest()
        {
            return string.Empty;
        }
        public virtual object ParseJson(string json)
        {
            return null;
        }
    }
}
