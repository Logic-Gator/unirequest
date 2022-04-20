using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public static class AsyncExtensions
    {
        public struct UnityWebRequestOperationAwaiter : INotifyCompletion
        {
            private readonly UnityWebRequestAsyncOperation operation;

            public bool IsCompleted => operation.isDone;

            public void GetResult() { }

            public UnityWebRequestOperationAwaiter(UnityWebRequestAsyncOperation operation)
            {
                this.operation = operation;
            }

            public void OnCompleted(Action continuation)
            {
                operation.completed += _ => continuation();
            }
        }

        public static UnityWebRequestOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation operation) =>
            new(operation);
    }
}
