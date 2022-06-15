using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
    public class NoDownloadHandler : DownloadHandlerScript
    {
        public static NoDownloadHandler Instance { get; } = new NoDownloadHandler();

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            return false;
        }
    }
}
