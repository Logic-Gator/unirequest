using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
	public static class UploadHandlerExtensions
	{
		public static T UploadWith<T>(this T request, UploadHandler handler)
			where T : IRequest
		{
			request.Request.uploadHandler = handler;
			return request;
		}
		
		public static T UploadBytes<T>(this T request, byte[] bytes)
			where T : IRequest
		{
			return request.UploadWith(new UploadHandlerRaw(bytes));
		}
		
		public static T UploadBytes<T>(this T request, NativeArray<byte>.ReadOnly bytes)
			where T : IRequest
		{
			return request.UploadWith(new UploadHandlerRaw(bytes));
		}
		
		public static T UploadBytes<T>(this T request, NativeArray<byte> bytes, bool transferOwnership)
			where T : IRequest
		{
			return request.UploadWith(new UploadHandlerRaw(bytes, transferOwnership));
		}
		
		public static T UploadWithJsonUtility<T, TJson>(this T request, TJson body, Encoding encoding = null)
			where T : IRequest
		{
			request.SetJsonContentTypeHeader();
			string json = JsonUtility.ToJson(body);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(json);
			return request.UploadBytes(bytes);
		}
	}
}