using System.IO;
using System.Text;
using UnityEngine;

namespace GatOR.Logic.Web
{
    public class DownloadHandlerJsonUtility<T> : DownloadHandlerStream
    {
        private readonly MemoryStream memoryStream;

        public DownloadHandlerJsonUtility() : base(new MemoryStream())
        {
            memoryStream = (MemoryStream)stream;
        }

        protected override byte[] GetData()
        {
            return memoryStream.GetBuffer();
        }

        public T GetData(Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            string json = encoding.GetString(base.GetData());
            return JsonUtility.FromJson<T>(json);
        }
    }
}
