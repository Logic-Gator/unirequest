#if JSONNET
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class JsonNetRequestExtensions
    {
        public static T UploadWithJsonNet<T, TJson>(this T request, TJson body)
            where T : IRequest
        {
            string json = JsonConvert.SerializeObject(body);
            request.Request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            return request;
        }

        public static UniWebRequest<TJson> DownloadWithJsonNet<T, TJson>(this T request,
            TJson data = default, JsonSerializer serializer = null, bool ignoreHeader = false)
            where T : IRequest
        {
            var downloadHandler = new DownloadHandlerBuffer();
            request.DownloadWith(downloadHandler);
            return new UniWebRequest<TJson>(request.Request, (request) =>
            {
                return request.GetDataWithJsonNet(data, serializer, ignoreHeader);
            });
        }

        public static T GetDataWithJsonNet<T>(this UnityWebRequest request, T data = default,
            JsonSerializer serializer = null, bool ignoreHeader = false)
        {
            if (!ignoreHeader && !request.GetResponseHeader("Content-Type").StartsWith("application/json"))
                throw new System.InvalidOperationException("Response is not json");

            var text = request.downloadHandler.text;
            if (typeof(T).IsClass && data != null)
            {
                if (serializer != null)
                {
                    using var reader = new StringReader(text);
                    using var jsonReader = new JsonTextReader(reader);
                    serializer.Populate(jsonReader, data);
                }
                else
                {
                    JsonConvert.PopulateObject(text, data);
                }
                return data;
            }

            if (serializer != null)
            {
                using var reader = new StringReader(text);
                using var jsonReader = new JsonTextReader(reader);
                return serializer.Deserialize<T>(jsonReader); 
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(text);
            }
        }
    }
}
#endif
