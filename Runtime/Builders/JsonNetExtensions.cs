#if JSONNET
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GatOR.Logic.Web.Constants;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class JsonNetExtensions
    {
        private static JsonSerializer DefaultSerializer { get; } = JsonSerializer.CreateDefault();
        
        public static UniRequest<TJson> DownloadWithJsonNet<T, TJson>(this T request,
            TJson data = default, JsonSerializer serializer = null, bool ignoreHeader = false)
            where T : IRequest
        {
            var downloadHandler = new DownloadHandlerBuffer();
            request.DownloadWith(downloadHandler);
            return new UniRequest<TJson>(request.Request, (result) =>
                result.GetDataWithJsonNet(data, serializer, ignoreHeader));
        }

        public static T GetDataWithJsonNet<T>(this UnityWebRequest request, T data = default,
            JsonSerializer serializer = null, bool ignoreHeader = false)
        {
            if (!ignoreHeader && !request.GetResponseHeader(Headers.ContentType).StartsWith(MimeTypes.Json))
                throw new System.InvalidOperationException("Response is not json");

            serializer ??= DefaultSerializer;
            var text = request.downloadHandler.text;
            using var reader = new StringReader(text);
            using var jsonReader = new JsonTextReader(reader);
            
            if (!typeof(T).IsClass || data == null)
                return serializer.Deserialize<T>(jsonReader);
            
            serializer.Populate(jsonReader, data);
            return data;

        }

        public static T UploadWithJsonNet<T, TJson>(this T request, TJson body, JsonSerializer serializer = null)
            where T : IRequest
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            (serializer ?? DefaultSerializer).Serialize(writer, body);
            
            return request.UploadBytes(stream.ToArray());
        }
    }
}
#endif
