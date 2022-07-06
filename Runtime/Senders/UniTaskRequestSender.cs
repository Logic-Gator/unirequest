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
            var operation = request.DownloadNothingIfNotSet().SendWebRequest();
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

        public static async UniTask<UniWebRequestResult> SendAsUniTask(this UniWebRequest request,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await Send(request.Request, progress, timing, cancellationToken);
            request.ThrowIfError();
            return new(request);
        }

        public static async UniTask<UniWebRequestResult<T>> SendAsUniTask<T>(this UniWebRequest<T> request,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await Send(request.Request, progress, timing, cancellationToken);
            return new(request);
        }

        public static async UniTask<UniWebRequestResult> SendWith(this UniWebRequest request, Func sender,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await sender(request.Request, progress, timing, cancellationToken);
            return new(request);
        }
        
        public static async UniTask<UniWebRequestResult<T>> SendWith<T>(this UniWebRequest<T> request, Func sender,
            IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await sender(request.Request, progress, timing, cancellationToken);
            return new(request);
        }
    }
}
#endif
