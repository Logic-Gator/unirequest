using System.Threading;
using System.Threading.Tasks;
#if UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace GatOR.Logic.Web
{
    public static class AsyncBuilderExtensions
    {
        public static T WithCancellation<T>(this T request, CancellationToken cancellationToken) where T : IRequest
        {
            cancellationToken.Register(request.Request.Abort);
            return request;
        }

        public static async Task<T> ThrowOnError<T>(this Task<T> task, bool throwOnHttpError = true)
            where T : UniRequestResult
        {
            var result = await task;
            result.ThrowIfError(throwOnHttpError);
            return result;
        }
        
        #if UNITASK
        public static async UniTask<T> ThrowOnError<T>(this UniTask<T> task, bool throwOnHttpError = true)
            where T : UniRequestResult
        {
            var result = await task;
            result.ThrowIfError(throwOnHttpError);
            return result;
        }
        #endif
    }
}
