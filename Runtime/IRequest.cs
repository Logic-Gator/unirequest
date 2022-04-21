using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public interface IRequest
    {
        UnityWebRequest Request { get; }
    }

    public interface IRequest<T> : IRequest
    {
        DataGetter<T> DataGetter { get; }
    }
}
