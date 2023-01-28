using System;
using System.Collections.Generic;
using GatOR.Logic.Web.Constants;
using UnityEngine;
using UnityEngine.Networking;

namespace GatOR.Logic.Web
{
	public static class FormExtensions
	{
		private static readonly List<IMultipartFormSection> FormSectionBuffer = new();

		public static T UploadFormData<T>(this T request, WWWForm formData) where T : IRequest
		{
			byte[] data = formData.data;
			if (data.Length == 0)
				data = null;
			if (data != null)
				request.Request.uploadHandler = new UploadHandlerRaw(data);
			foreach (var header in formData.headers)
				request.Request.SetRequestHeader(header.Key, header.Value);
			return request;
		}

		
		public static T UploadMultipartFormData<T>(this T request,
			params IMultipartFormSection[] multipartFormSections)
			where T : IRequest
		{
			return request.UploadMultipartFormData(UnityWebRequest.GenerateBoundary(),
				(IEnumerable<IMultipartFormSection>) multipartFormSections);
		}
		
		public static T UploadMultipartFormData<T>(this T request,
			IEnumerable<IMultipartFormSection> multipartFormSections)
			where T : IRequest
		{
			return request.UploadMultipartFormData(UnityWebRequest.GenerateBoundary(), multipartFormSections);
		}

		public static T UploadMultipartFormData<T>(this T request, byte[] boundary,
			params IMultipartFormSection[] formSections)
			where T : IRequest
		{
			return request.UploadMultipartFormData(boundary, (IEnumerable<IMultipartFormSection>) formSections);
		}

		public static T UploadMultipartFormData<T>(this T request, byte[] boundary,
			IEnumerable<IMultipartFormSection> formSections)
			where T : IRequest
		{
			byte[] data = null;
			if (formSections != null)
			{
				FormSectionBuffer.Clear();
				FormSectionBuffer.AddRange(formSections);
				if (FormSectionBuffer.Count <= 0)
					return request;
				
				data = UnityWebRequest.SerializeFormSections(FormSectionBuffer, boundary);
			}

			if (data == null)
				throw new ArgumentException("FormSections empty", nameof(formSections));
			
			return request.UploadWith(new UploadHandlerRaw(data)
			{
				contentType = MimeTypes.MultipartFormWithBoundary(boundary)
			});
		}
	}
}