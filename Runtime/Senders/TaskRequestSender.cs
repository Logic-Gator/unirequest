using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class TaskRequestSender
    {
        public delegate Task<UniWebRequestResult> Delegate(IRequest request, CancellationToken cancellationToken = default);
        public delegate Task<UniWebRequestResult<T>> Delegate<T>(IRequest<T> request, CancellationToken cancellationToken = default);

        public static async Task<UniWebRequestResult> SendAsTask(this IRequest request, CancellationToken cancellationToken = default)
        {
            request.Request.downloadHandler ??= new DownloadHandlerBuffer();

            cancellationToken.Register(() => request.Request.Abort());
            await request.Request.SendWebRequest();
            cancellationToken.ThrowIfCancellationRequested();
            return new(request);
        }

        public static async Task<UniWebRequestResult<T>> SendAsTask<T>(this IRequest<T> request, CancellationToken cancellationToken = default)
        {
            await SendAsTask((IRequest)request, cancellationToken);
            return new(request);
        }

        public static Task<UniWebRequestResult> SendAsyncWith(this IRequest request, Delegate taskRequestSender,
            CancellationToken cancellationToken = default)
        {
            return taskRequestSender(request, cancellationToken);
        }

        public static Task<UniWebRequestResult<T>> SendAsyncWith<T>(this IRequest<T> request, Delegate<T> taskRequestSender,
            CancellationToken cancellationToken = default)
        {
            return taskRequestSender(request, cancellationToken);
        }
    }
}
