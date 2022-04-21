using System.Threading.Tasks;
using UnityEngine.Networking;


namespace GatOR.Logic.Web
{
    public delegate T DataGetter<T>(UnityWebRequest request);
    
    public static class DataGetterExtensions
    {
        public static async Task<T> GetData<T>(this Task<UniWebRequestResult<T>> task)
        {
            var result = await task;
            return result.GetData();
        }

#if UNITASK
        public static async Cysharp.Threading.Tasks.UniTask<T> GetData<T>(this Cysharp.Threading.Tasks.UniTask<UniWebRequestResult<T>> task)
        {
            var result = await task;
            return result.GetData();
        }
#endif
    }
}