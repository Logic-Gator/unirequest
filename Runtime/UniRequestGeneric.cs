using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public struct UniRequest<T> : IRequest<T>
    {
        private UnityWebRequest request;
        public UnityWebRequest Request => request ??= new UnityWebRequest();
        public DataGetter<T> DataGetter { get; }
        
        public UniRequest(UnityWebRequest request, DataGetter<T> dataGetter)
        {
            this.request = request;
            DataGetter = dataGetter;
        }
    }
}
