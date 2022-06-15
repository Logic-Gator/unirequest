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
            request.DownloadNothingIfNotSet();

            cancellationToken.Register(request.Abort);
            await request.SendWebRequest();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public static async Task<UniWebRequestResult> SendAsTask(this UniWebRequest request,
            CancellationToken cancellationToken = default)
        {
            await Send(request, cancellationToken);
            request.ThrowIfException();
            return new(request);
        }

        public static async Task<UniWebRequestResult<T>> SendAsTask<T>(this UniWebRequest<T> request,
            CancellationToken cancellationToken = default)
        {
            await Send(request.Request, cancellationToken);
            request.ThrowIfException();
            return new(request);
        }

        public static async Task<UniWebRequestResult> SendAsyncWith(this UniWebRequest request, Func sender,
            CancellationToken cancellationToken = default)
        {
            await sender(request.Request, cancellationToken);
            request.ThrowIfException();
            return new(request);
        }

        public static async Task<UniWebRequestResult<T>> SendAsyncWith<T>(this UniWebRequest<T> request,
            Func sender, CancellationToken cancellationToken = default)
        {
            await sender(request.Request, cancellationToken);
            request.ThrowIfException();
            return new(request);
        }
    }
}
