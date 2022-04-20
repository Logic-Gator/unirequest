

namespace GatOR.Logic.Web
{
    public static class HeaderExtensions
    {
        public static T SetHeader<T>(this T request, string name, string value) where T : IRequest
        {
            request.Request.SetRequestHeader(name, value);
            return request;
        }

        public static T SetContentTypeHeader<T>(this T request, string contentType) where T : IRequest
        {
            request.Request.SetRequestHeader("Content-Type", contentType);
            return request;
        }

        public static T SetJsonContentTypeHeader<T>(this T request) where T : IRequest
        {
            request.Request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        public static T SetAcceptHeader<T>(this T request, string accept) where T : IRequest
        {
            request.Request.SetRequestHeader("Accept", accept);
            return request;
        }


        public static T SetAuthorizationHeader<T>(this T request, string type, string value) where T : IRequest
        {
            return SetHeader(request, "Authorization", $"{type} {value}");
        }
    }
}
