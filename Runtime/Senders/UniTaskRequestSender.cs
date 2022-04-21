#if UNITASK
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class UniTaskRequestSender
    {
        public delegate UniTask<UniWebRequestResult> Delegate(IRequest request, CancellationToken cancellationToken = default);
        public delegate UniTask<UniWebRequestResult<T>> Delegate<T>(IRequest<T> request, CancellationToken cancellationToken = default);

        public static async UniTask<UniWebRequestResult> SendAsUniTask(this IRequest request, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            request.Request.downloadHandler ??= new DownloadHandlerBuffer();

            await request.Request.SendWebRequest().ToUniTask(progress, timing, cancellationToken);
            return new(request);
        }

        public static async UniTask<UniWebRequestResult<T>> SendAsUniTask<T>(this IRequest<T> request, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            await SendAsUniTask((IRequest)request, progress, timing, cancellationToken);
            return new(request);
        }

        public static UniTask<UniWebRequestResult> SendAsyncWith(this IRequest request, Delegate taskRequestSender,
            CancellationToken cancellationToken = default)
        {
            return taskRequestSender(request, cancellationToken);
        }

        public static UniTask<UniWebRequestResult<T>> SendAsyncWith<T>(this IRequest<T> request, Delegate<T> taskRequestSender,
            CancellationToken cancellationToken = default)
        {
            return taskRequestSender(request, cancellationToken);
        }
    }
}
#endif
