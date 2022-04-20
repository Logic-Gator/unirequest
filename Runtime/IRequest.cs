using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public interface IRequest
    {
        UnityWebRequest Request { get; }
    }

    public interface IRequest<TDownloadHandler> : IRequest where TDownloadHandler : DownloadHandler
    {
        TDownloadHandler DownloadHandler { get; }
    }
}
