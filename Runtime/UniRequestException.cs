using System;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public abstract class UniRequestException : Exception
    {
        public UnityWebRequest Request { get; }
        public UnityWebRequest.Result Result => Request.result;
        public long StatusCode => Request.responseCode;
        public string Error => Request.error;
        public string Text => Request.downloadHandler.text;
        public byte[] Bytes => Request.downloadHandler.data;

        private string message;
        public override string Message
        {
            get
            {
                if (message != null)
                    return message;
                
                string text = Text;
                if (text != null && text.Length > 100)
                    text = text[..100] + "...";

                message = string.IsNullOrEmpty(text) ? Error : $"{Error}{Environment.NewLine}{text}";
                return message;
            }
        }

        public UniRequestException(UnityWebRequest request)
        {
            Request = request;
        }
    }

    public static class UniWebExceptionExtensions
    {
        public static bool TryGetException(this UnityWebRequest request, out UniRequestException exception)
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    exception = new UniRequestConnectionException(request);
                    return true;
                case UnityWebRequest.Result.ProtocolError:
                    exception = new UniRequestHttpException(request);
                    return true;
                case UnityWebRequest.Result.DataProcessingError:
                    exception = new UniRequestDataProcessingException(request);
                    return true;
                case UnityWebRequest.Result.Success:
                case UnityWebRequest.Result.InProgress:
                default:
                    exception = null;
                    return false;
            }
        }

        public static void ThrowIfError(this UnityWebRequest request)
        {
            if (TryGetException(request, out var exception))
                throw exception;
        }

        public static void ThrowIfError(this UniRequest request) => request.Request.ThrowIfError();
        public static void ThrowIfError<T>(this UniRequest<T> request) => request.Request.ThrowIfError();
        
        public static T ThrowIfError<T>(this T result) where T : UniRequestResult
        {
            result.Request.ThrowIfError();
            return result;
        }
    }

    public class UniRequestHttpException : UniRequestException
    {
        public UniRequestHttpException(UnityWebRequest request) : base(request)
        {
        }
    }

    public class UniRequestConnectionException : UniRequestException
    {
        public UniRequestConnectionException(UnityWebRequest request) : base(request)
        {
        }
    }

    public class UniRequestDataProcessingException : UniRequestException
    {
        public UniRequestDataProcessingException(UnityWebRequest request) : base(request)
        {
        }
    }
}
