using GatOR.Logic.Web.Constants;

namespace GatOR.Logic.Web
{
    public static class HeaderExtensions
    {
        public static T SetHeader<T>(this T request, string name, string value) where T : IRequest
        {
            request.Request.SetRequestHeader(name, value);
            return request;
        }

        public static T SetContentTypeHeader<T>(this T request, string mimeType) where T : IRequest
        {
            return request.SetHeader(Headers.ContentType, mimeType);
        }

        public static T SetJsonContentTypeHeader<T>(this T request) where T : IRequest
        {
            return request.SetContentTypeHeader(MimeTypes.Json);
        }

        public static T SetAcceptHeader<T>(this T request, string mimeType) where T : IRequest
        {
            return request.SetHeader(Headers.Accept, mimeType);
        }

        public static T SetAuthorizationHeader<T>(this T request, string type, string value) where T : IRequest
        {
            return request.SetHeader(Headers.Authorization, $"{type} {value}");
        }
    }
}
