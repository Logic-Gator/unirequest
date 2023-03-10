#if UNITASK
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class UniTaskRequestSender
    {
        public delegate UniTask Func(UnityWebRequest request, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default);

        public static Func Sender { get; } = Send;

        public static async UniTask Send(UnityWebRequest request, IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            // Done this way to let UniRequest to throw errors instead of UniTask
            var operation = request.DefaultDownloadIfNotSet().SendWebRequest();
            while (!operation.isDone)
            {
                progress?.Report(operation.progress);
                if (cancellationToken.IsCancellationRequested)
                {
                    request.Abort();
                    throw new OperationCanceledException(cancellationToken);
                }
                await UniTask.Yield(timing);
            }
        }

        public static async UniTask<UniRequestResult> SendAsUniTask(this UniRequest request,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await Send(request.Request, progress, timing, cancellationToken);
            request.Request.ThrowIfError();
            return new UniRequestResult(request);
        }

        public static async UniTask<UniRequestResult<T>> SendAsUniTask<T>(this UniRequest<T> request,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await Send(request.Request, progress, timing, cancellationToken);
            return new UniRequestResult<T>(request);
        }

        public static async UniTask<UniRequestResult> SendUsing(this UniRequest request, Func sender,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await sender(request.Request, progress, timing, cancellationToken);
            return new UniRequestResult(request);
        }
        
        public static async UniTask<UniRequestResult<T>> SendUsing<T>(this UniRequest<T> request, Func sender,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await sender(request.Request, progress, timing, cancellationToken);
            return new UniRequestResult<T>(request);
        }
    }
}
#endif
