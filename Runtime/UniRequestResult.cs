using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public class UniRequestResult
    {
#if JSONNET
        [Newtonsoft.Json.JsonIgnore]
#endif
        public UnityWebRequest Request { get; }
        public long Status => Request.responseCode;
        public string Error => Request.error;

        public string GetHeaderValue(string headerName)
        {
            return Request.GetResponseHeader(headerName);
        }

        public UniRequestResult(UnityWebRequest request)
        {
            Request = request;
        }

        public UniRequestResult(UniRequest request) : this(request.Request)
        {
        }

#if JSONNET
        public string ToJson(Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.Indented)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, formatting);
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
