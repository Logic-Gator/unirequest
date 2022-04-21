using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public readonly struct UniWebRequestResult
    {
        public UnityWebRequest Request { get; }
        public long Status => Request.responseCode;
        public string Error => Request.error;

        public UniWebRequestResult(UnityWebRequest request)
        {
            Request = request;
        }

        public UniWebRequestResult(IRequest request) : this(request.Request)
        {
        }
    }

    public readonly struct UniWebRequestResult<T>
    {
        public UnityWebRequest Request { get; }
        public long Status => Request.responseCode;
        public string Error => Request.error;

        private readonly DataGetter<T> dataGetter;
        
        public T GetData()
        {
            return dataGetter(Request);
        }


        public UniWebRequestResult(UnityWebRequest request, DataGetter<T> dataGetter)
        {
            Request = request;
            this.dataGetter = dataGetter;
        }

        public UniWebRequestResult(IRequest<T> request) : this(request.Request, request.DataGetter)
        {
        }

        public static implicit operator UniWebRequestResult(UniWebRequestResult<T> result)
        {
            return new UniWebRequestResult(result.Request);
        }
    }
}
