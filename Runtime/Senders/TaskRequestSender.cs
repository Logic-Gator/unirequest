using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class TaskRequestSender
    {
        public delegate Task<UniWebRequestResult<T>> Delegate<T>(IRequest<T> request, CancellationToken cancellationToken = default)
            where T : DownloadHandler;

        public static async Task<UniWebRequestResult<T>> SendAsTask<T>(this IRequest<T> request, CancellationToken cancellationToken = default)
            where T : DownloadHandler
        {
            request.Request.downloadHandler ??= new DownloadHandlerBuffer();

            cancellationToken.Register(() => request.Request.Abort());
            await request.Request.SendWebRequest();
            cancellationToken.ThrowIfCancellationRequested();
            return new(request); 
        }

        public static Task<UniWebRequestResult<T>> SendAsyncWith<T>(this IRequest<T> request, Delegate<T> taskRequestSender,
            CancellationToken cancellationToken = default)
            where T : DownloadHandler
        {
            return taskRequestSender(request, cancellationToken);
        }
    }
}
