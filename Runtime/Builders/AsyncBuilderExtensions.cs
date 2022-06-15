using System.Threading;

namespace GatOR.Logic.Web
{
    public static class AsyncBuilderExtensions
    {
        public static T WithCancellation<T>(this T request, CancellationToken cancellationToken) where T : IRequest
        {
            cancellationToken.Register(request.Request.Abort);
            return request;
        }
    }
}
