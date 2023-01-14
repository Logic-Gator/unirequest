using System.Threading.Tasks;
using GatOR.Logic.Web.Constants;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class DownloadBuilderExtensions
    {
        public static UniRequest DownloadNothing(this UniRequest request)
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


        public static UniRequest<TData> DownloadWith<T, TData>(this T request, DataGetter<TData> dataGetter)
            where T : IRequest
        {
            return new UniRequest<TData>(request.Request, dataGetter);
        }

        public static UniRequest<TData> DownloadWith<T, TData>(this T request, DataGetter<TData> dataGetter, DownloadHandler downloadHandler)
            where T : IRequest
        {
            request.Request.downloadHandler = downloadHandler;
            return new UniRequest<TData>(request.Request, dataGetter);
        }

        public static T DownloadWith<T>(this T request, DownloadHandler downloadHandler)
            where T : IRequest
        {
            request.Request.downloadHandler = downloadHandler;
            return request;
        }

        public static UniRequest<TData> DownloadWithBuffer<T, TData>(this T request, DataGetter<TData> dataGetter)
            where T : IRequest
        {
            return request.DownloadWith(dataGetter, new DownloadHandlerBuffer());
        }

        public static UniRequest<string> DownloadText<T>(this T request)
            where T : IRequest
        {
            return request.DownloadWithBuffer(r => r.downloadHandler.text);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static UniRequest<TJson> DownloadWithJsonUtility<T, TJson>(this T request, TJson data, bool overwriteData = true)
#pragma warning restore IDE0060
            where T : IRequest
        {
            request.SetAcceptHeader(MimeTypes.Json);
            return request.DownloadWithBuffer(GetData);

            TJson GetData(UnityWebRequest result)
            {
                var json = result.downloadHandler.text;
                if (!overwriteData || data == null)
                    return JsonUtility.FromJson<TJson>(json);
                
                JsonUtility.FromJsonOverwrite(json, data);
                return data;

            }
        }

        public static UniRequest<Texture2D> DownloadTexture<T>(this T request, bool readable = false) where T : IRequest
        {
            request.DownloadWith(new DownloadHandlerTexture(readable));
            return new UniRequest<Texture2D>(request.Request, DownloadHandlerTexture.GetContent);
        }
    }
}
