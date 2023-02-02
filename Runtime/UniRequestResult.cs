using System.IO;
using GatOR.Logic.Web.Constants;
using Unity.Collections;
using UnityEngine.Networking;

#if JSONNET
using Newtonsoft.Json;
#endif


namespace GatOR.Logic.Web
{
    public class UniRequestResult
    {
#if JSONNET
        [JsonIgnore]
#endif
        public UnityWebRequest Request { get; }

        public UnityWebRequest.Result Result => Request.result;
        public long Status => Request.responseCode;
        public string ErrorMessage => Request.error;
        public string ContentType => GetHeaderValue(Headers.ContentType);

        public string Text => Request.downloadHandler.text;
        public byte[] Bytes => Request.downloadHandler.data;
        public NativeArray<byte>.ReadOnly NativeBytes => Request.downloadHandler.nativeData;

        public UniRequestResult(UnityWebRequest request)
        {
            Request = request;
        }

        public UniRequestResult(UniRequest request) : this(request.Request)
        {
        }
        
        public string GetHeaderValue(string headerName)
        {
            return Request.GetResponseHeader(headerName);
        }

#if JSONNET
        public T GetJson<T>(JsonSerializer serializer = null)
        {
            serializer ??= JsonSerializer.CreateDefault();
            using var reader = new StringReader(Text);
            using var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<T>(jsonReader);
        }
        
        public string ToJson(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public override string ToString() => ToJson();
#endif
    }

    public class UniRequestResult<T> : UniRequestResult
    {
        private bool gotData;
        private T data;
        public T Data
        {
            get
            {
                if (gotData)
                    return data;
                
                data = dataGetter(Request);
                gotData = true;
                return data;
            }
        }

        private readonly DataGetter<T> dataGetter;


        public UniRequestResult(UnityWebRequest request, DataGetter<T> dataGetter) : base(request)
        {
            this.dataGetter = dataGetter;
        }

        public UniRequestResult(UniRequest<T> request) : this(request.Request, request.DataGetter)
        {
        }
    }
}
