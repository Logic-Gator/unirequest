using System.Text;

namespace GatOR.Logic.Web.Constants
{
	public static class MimeTypes
	{
		public const string PlainText = "text/plain";
		public const string Html = "text/html";
		public const string Json = "application/json";
		public const string MultipartForm = "multipart/form-data";

		public static string MultipartFormWithBoundary(byte[] boundary)
		{
			return $"{MultipartForm}; boundary= {Encoding.UTF8.GetString(boundary, 0, boundary.Length)}";
		}
	}
}