using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public struct UniWebRequest<T> : IRequest<T>
    {
        private UnityWebRequest request;
        public UnityWebRequest Request => request ??= new UnityWebRequest();
        public DataGetter<T> DataGetter { get; }
        
        public UniWebRequest(UnityWebRequest request, DataGetter<T> dataGetter)
        {
            this.request = request;
            DataGetter = dataGetter;
        }
    }
}
