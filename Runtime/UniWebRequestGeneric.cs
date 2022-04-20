using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public struct UniWebRequest<TDownloadHandler> : IRequest<TDownloadHandler> where TDownloadHandler : DownloadHandler
    {
        private UnityWebRequest request;
        public UnityWebRequest Request => request ??= new UnityWebRequest();
        public TDownloadHandler DownloadHandler => (TDownloadHandler)Request.downloadHandler;

        public UniWebRequest(UnityWebRequest request, TDownloadHandler downloadHandler)
        {
            this.request = request;
            request.downloadHandler = downloadHandler;
        }
    }
}
