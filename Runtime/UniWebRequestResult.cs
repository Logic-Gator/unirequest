using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public struct UniWebRequestResult<T> where T : DownloadHandler
    {
        public UnityWebRequest Request { get; }
        public T DownloadHandler => (T)Request.downloadHandler;

        public long Status => Request.responseCode;
        public string Error => Request.error;


        public UniWebRequestResult(UnityWebRequest request)
        {
            Request = request;
        }

        public UniWebRequestResult(IRequest<T> request)
        {
            Request = request.Request;
        }
    }
}
