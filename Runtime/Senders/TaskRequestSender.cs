using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class TaskRequestSender
    {
        public delegate Task Func(UnityWebRequest request, CancellationToken cancellationToken = default);

        public static Func Sender { get; } = Send;

        public static async Task Send(UnityWebRequest request, CancellationToken cancellationToken = default)
        {
            request.DefaultDownloadIfNotSet();

            cancellationToken.Register(request.Abort);
            await request.SendWebRequest();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public static async Task<UniRequestResult> SendAsTask(this UniRequest request,
            CancellationToken cancellationToken = default)
        {
            await Send(request, cancellationToken);
            return new UniRequestResult(request);
        }

        public static async Task<UniRequestResult<T>> SendAsTask<T>(this UniRequest<T> request,
            CancellationToken cancellationToken = default)
        {
            await Send(request.Request, cancellationToken);
            return new UniRequestResult<T>(request);
        }

        public static async Task<UniRequestResult> SendUsing(this UniRequest request, Func sender,
            CancellationToken cancellationToken = default)
        {
            await sender(request.Request, cancellationToken);
            return new UniRequestResult(request);
        }

        public static async Task<UniRequestResult<T>> SendUsing<T>(this UniRequest<T> request,
            Func sender, CancellationToken cancellationToken = default)
        {
            await sender(request.Request, cancellationToken);
            return new UniRequestResult<T>(request);
        }
    }
}
