using System.Threading;
using AnimeWallPaper.Request;

namespace AnimeWallPaper.Ultility
{
    public class RequestWorker
    {
        public static BlockingQueue<BaseRequest> ListRequest = new BlockingQueue<BaseRequest>(true);

        private static readonly RequestWorker _instance = new RequestWorker();
        public static RequestWorker Instance
        {
            get { return _instance; }
        }

        // Explicit static constructor to tell C# compiler not to mark type as BeforeFieldInit
        static RequestWorker() { }

        private readonly Thread _backgroundWorker;
        private bool _isClearWorker;
        private bool IsClearWorker
        {
            get { return _isClearWorker; }
            set
            {
                _isClearWorker = value;
                ListRequest.ClearQueue = value;
            }
        }

        private RequestWorker()
        {
            _backgroundWorker = new Thread(MainProcess)
                                    {
                                        IsBackground = true,
                                        Name = "Request Worker"
                                    };
            IsClearWorker = true;
            _backgroundWorker.Start();
        }

        public void AddRequest(BaseRequest request)
        {
            ListRequest.Add(request);
        }
        public void ForceAddRequest(BaseRequest request)
        {
            IsClearWorker = false;
            ListRequest.Add(request);
            IsClearWorker = true;
        }

        private static void MainProcess()
        {
            while (true)
            {
                var currentRequest = ListRequest.Get();
                if (currentRequest == null) continue;
                currentRequest.Process();
            }
        }
    }

}
