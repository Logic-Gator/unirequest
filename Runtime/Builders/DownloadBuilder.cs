using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class DownloadBuilderExtensions
    {
        public static UniWebRequest<TDownloadHandler> DownloadWith<T, TDownloadHandler>(this T request, TDownloadHandler downloadHandler)
            where T : IRequest
            where TDownloadHandler : DownloadHandler
        {
            return new UniWebRequest<TDownloadHandler>(request.Request, downloadHandler);
        }

        public static UniWebRequest<DownloadHandlerTexture> DownloadTexture<T>(this T request, bool readable = false)
            where T : IRequest
        {
            return request.DownloadWith(new DownloadHandlerTexture(readable));
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static UniWebRequest<DownloadHandlerJsonUtility<TJson>> DownloadJsonWithJsonUtility<T, TJson>(this T request, TJson bodyType)
#pragma warning restore IDE0060
            where T : IRequest
        {
            request.SetAcceptHeader("application/json");
            return request.DownloadWith(new DownloadHandlerJsonUtility<TJson>());
        }

        public static async Task<Texture2D> GetText(this Task<UniWebRequestResult<DownloadHandlerTexture>> task)
        {
            var result = await task;
            return result.DownloadHandler.texture;
        }        

        public static async Task<string> GetText<T>(this Task<UniWebRequestResult<T>> task)
            where T : DownloadHandler
        {
            var result = await task;
            return result.DownloadHandler.text;
        }
    }
}
