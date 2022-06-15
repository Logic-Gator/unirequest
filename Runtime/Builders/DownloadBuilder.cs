using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class DownloadBuilderExtensions
    {
        public static UniWebRequest DownloadNothing(this UniWebRequest request)
        {
            request.Request.DownloadNothing();
            return request;
        }

        public static UnityWebRequest DownloadNothing(this UnityWebRequest request)
        {
            request.downloadHandler = NoDownloadHandler.Instance;
            return request;
        }

        public static UnityWebRequest DownloadNothingIfNotSet(this UnityWebRequest request)
        {
            if (request.downloadHandler == null)
                request.DownloadNothing();
            return request;
        }


        public static UniWebRequest<TData> DownloadWith<T, TData>(this T request, DataGetter<TData> dataGetter)
            where T : IRequest
        {
            return new UniWebRequest<TData>(request.Request, dataGetter);
        }

        public static UniWebRequest<TData> DownloadWith<T, TData>(this T request, DataGetter<TData> dataGetter, DownloadHandler downloadHandler)
            where T : IRequest
        {
            request.Request.downloadHandler = downloadHandler;
            return new UniWebRequest<TData>(request.Request, dataGetter);
        }

        public static T DownloadWith<T>(this T request, DownloadHandler downloadHandler)
            where T : IRequest
        {
            request.Request.downloadHandler = downloadHandler;
            return request;
        }

        public static UniWebRequest<TData> DownloadWithBuffer<T, TData>(this T request, DataGetter<TData> dataGetter)
            where T : IRequest
        {
            return request.DownloadWith(dataGetter, new DownloadHandlerBuffer());
        }

        public static UniWebRequest<string> DownloadText<T>(this T request)
            where T : IRequest
        {
            return request.DownloadWithBuffer(r => r.downloadHandler.text);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static UniWebRequest<TJson> DownloadWithJsonUtility<T, TJson>(this T request, TJson bodyType)
#pragma warning restore IDE0060
            where T : IRequest
        {
            request.SetAcceptHeader("application/json");
            return request.DownloadWithBuffer(r => JsonUtility.FromJson<TJson>(r.downloadHandler.text));
        }

        public static UniWebRequest<Texture2D> DownloadTexture<T>(this T request, bool readable = false) where T : IRequest
        {
            request.DownloadWith(new DownloadHandlerTexture(readable));
            return new UniWebRequest<Texture2D>(request.Request, DownloadHandlerTexture.GetContent);
        }
    }
}
