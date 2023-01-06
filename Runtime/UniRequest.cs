using System;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public struct UniRequest : IRequest
    {
        private UnityWebRequest request;
        public UnityWebRequest Request => request ??= new UnityWebRequest();
        public DataGetter<UnityWebRequest> DataGetter => GetRequest;

        public UniRequest(UnityWebRequest request)
        {
            this.request = request;
        }

        public UniRequest(Uri uri, string method)
        {
            request = new UnityWebRequest(uri, method, null, null);
        }

        public UniRequest(string url, string method)
        {
            request = new UnityWebRequest(url, method, null, null);
        }

        public static UniRequest Get(string url) => new(url, UnityWebRequest.kHttpVerbGET);
        public static UniRequest Get(Uri uri) => new(uri, UnityWebRequest.kHttpVerbGET);

        public static UniRequest Post(string url) => new(url, UnityWebRequest.kHttpVerbPOST);
        public static UniRequest Post(Uri uri) => new(uri, UnityWebRequest.kHttpVerbPOST);

        public static UniRequest Put(string url) => new(url, UnityWebRequest.kHttpVerbPUT);
        public static UniRequest Put(Uri uri) => new(uri, UnityWebRequest.kHttpVerbPUT);

        public static UniRequest Delete(string url) => new(url, UnityWebRequest.kHttpVerbDELETE);
        public static UniRequest Delete(Uri uri) => new(uri, UnityWebRequest.kHttpVerbDELETE);

        public static UniRequest Head(string url) => new(url, UnityWebRequest.kHttpVerbHEAD);
        public static UniRequest Head(Uri uri) => new(uri, UnityWebRequest.kHttpVerbHEAD);

        public static UniRequest Options(string url) => new(url, "OPTIONS");
        public static UniRequest Options(Uri uri) => new(uri, "OPTIONS");

        public static UniRequest Patch(string url) => new(url, "PATCH");
        public static UniRequest Patch(Uri uri) => new(uri, "PATCH");

        private static UnityWebRequest GetRequest(UnityWebRequest request) => request;

        public static implicit operator UnityWebRequest(UniRequest request) => request.Request;
        public static implicit operator UniRequest(UnityWebRequest request) => new(request);
    }

    public static class UniRequestExtensions
    {
        public static UniRequest AsUniRequest(this UnityWebRequest request) => new(request);
        public static UniRequest<T> AsUniRequest<T>(this UnityWebRequest request, DataGetter<T> dataGetter) =>
            new(request, dataGetter);
    }
}
