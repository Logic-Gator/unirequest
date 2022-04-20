using System;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public struct UniWebRequest : IRequest<DownloadHandler>
    {
        private UnityWebRequest request;
        public UnityWebRequest Request => request ??= new UnityWebRequest();
        public DownloadHandler DownloadHandler => Request.downloadHandler;

        public UniWebRequest(UnityWebRequest request)
        {
            this.request = request;
        }

        public UniWebRequest(Uri uri, string method)
        {
            request = new UnityWebRequest(uri, method, null, null);
        }

        public UniWebRequest(string url, string method)
        {
            request = new UnityWebRequest(url, method, null, null);
        }

        public static UniWebRequest Get(string url) => new(url, UnityWebRequest.kHttpVerbGET);
        public static UniWebRequest Get(Uri uri) => new(uri, UnityWebRequest.kHttpVerbGET);

        public static UniWebRequest Post(string url) => new(url, UnityWebRequest.kHttpVerbPOST);
        public static UniWebRequest Post(Uri uri) => new(uri, UnityWebRequest.kHttpVerbPOST);

        public static UniWebRequest Put(string url) => new(url, UnityWebRequest.kHttpVerbPUT);
        public static UniWebRequest Put(Uri uri) => new(uri, UnityWebRequest.kHttpVerbPUT);

        public static UniWebRequest Delete(string url) => new(url, UnityWebRequest.kHttpVerbDELETE);
        public static UniWebRequest Delete(Uri uri) => new(uri, UnityWebRequest.kHttpVerbDELETE);

        public static UniWebRequest Head(string url) => new(url, UnityWebRequest.kHttpVerbHEAD);
        public static UniWebRequest Head(Uri uri) => new(uri, UnityWebRequest.kHttpVerbHEAD);

        public static UniWebRequest Options(string url) => new(url, "OPTIONS");
        public static UniWebRequest Options(Uri uri) => new(uri, "OPTIONS");

        public static UniWebRequest Trace(string url) => new(url, "PATCH");
        public static UniWebRequest Patch(Uri uri) => new(uri, "PATCH");

        public static implicit operator UnityWebRequest(UniWebRequest request) => ((IRequest)request).Request;
        public static implicit operator UniWebRequest(UnityWebRequest request) => new(request);
    }
}
