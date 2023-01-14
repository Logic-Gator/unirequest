using System.IO;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public class DownloadHandlerStream : DownloadHandlerScript
    {
        private static readonly byte[] SharedBuffer = new byte[1024];
        
        protected int received;
        public ulong ContentLength { get; private set; }
        protected readonly Stream stream;

        public DownloadHandlerStream(Stream stream) : base(SharedBuffer)
        {
            this.stream = stream;
        }

        protected override void ReceiveContentLengthHeader(ulong contentLength)
        {
            ContentLength = contentLength;
        }

        protected override float GetProgress()
        {
            return ContentLength > 0 ? (float)received / ContentLength : 0;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            stream.Write(data, 0, dataLength);
            received += dataLength;
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            stream.Dispose();
        }
    }
}
