using System.Threading.Tasks;
using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public delegate T DataGetter<T>(UnityWebRequest request);
    
    public static class DataGetterExtensions
    {
        public static async Task<T> GetData<T>(this Task<UniRequestResult<T>> task)
        {
            var result = await task;
            var data = result.Data;
            result.Request.Dispose();
            return data;
        }

#if UNITASK
        public static async Cysharp.Threading.Tasks.UniTask<T> GetData<T>(this Cysharp.Threading.Tasks.UniTask<UniRequestResult<T>> task)
        {
            var result = await task;
            var data = result.Data;
            result.Request.Dispose();
            return data;
        }
#endif
    }
}
