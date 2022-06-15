using System;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public abstract class UniWebException : Exception
    {
        public UnityWebRequest Request { get; }
        public UnityWebRequest.Result Result => Request.result;
        public long StatusCode => Request.responseCode;
        public string Error => Request.error;
        public string Text => Request.downloadHandler.text;
        public byte[] Bytes => Request.downloadHandler.data;

        private string message = null;
        public override string Message
        {
            get
            {
                if (message == null)
                {
                    string text = Text;
                    if (text != null && text.Length > 100)
                        text = text[..100] + "...";

                    message = string.IsNullOrEmpty(text) ? Error : $"{Error}{Environment.NewLine}{text}";
                }
                return message;
            }
        }

        public UniWebException(UnityWebRequest request)
        {
            Request = request;
        }
    }

    public static class UniWebExceptionExtensions
    {
        public static bool TryGetException(this UnityWebRequest request, out UniWebException exception)
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    exception = new UniWebConnectionException(request);
                    return true;
                case UnityWebRequest.Result.ProtocolError:
                    exception = new UniWebHttpException(request);
                    return true;
                case UnityWebRequest.Result.DataProcessingError:
                    exception = new UniWebDataProcessingException(request);
                    return true;
                case UnityWebRequest.Result.Success:
                case UnityWebRequest.Result.InProgress:
                default:
                    exception = null;
                    return false;
            }
        }

        public static void ThrowIfException(this UnityWebRequest request)
        {
            if (TryGetException(request, out var exception))
                throw exception;
        }

        public static void ThrowIfException(this UniWebRequest request) => request.Request.ThrowIfException();
        public static void ThrowIfException<T>(this UniWebRequest<T> request) => request.Request.ThrowIfException();
    }

    public class UniWebHttpException : UniWebException
    {
        public UniWebHttpException(UnityWebRequest request) : base(request)
        {
        }
    }

    public class UniWebConnectionException : UniWebException
    {
        public UniWebConnectionException(UnityWebRequest request) : base(request)
        {
        }
    }

    public class UniWebDataProcessingException : UniWebException
    {
        public UniWebDataProcessingException(UnityWebRequest request) : base(request)
        {
        }
    }
}
