using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class OperationRequestSender
    {
        public delegate UnityWebRequestAsyncOperation Func(UnityWebRequest request);

        public static Func Sender { get; } = Send;

        public static UnityWebRequestAsyncOperation Send(UnityWebRequest request)
        {
            return request.SendWebRequest();
        }

        public static UnityWebRequestAsyncOperation SendAsOperation(this UniWebRequest request)
        {
            return Send(request.Request);
        }

        public static UnityWebRequestAsyncOperation SendAsyncWith(this UniWebRequest request, Func sender)
        {
            return sender(request.Request);
        }
    }
}
