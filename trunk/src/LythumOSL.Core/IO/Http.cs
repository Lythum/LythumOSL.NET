using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

using LythumOSL.Core;

namespace LythumOSL.Core.IO
{
	#region Enum
	public enum HttpMethod
	{
		Post,
		Get
	}

	#endregion

	public class Http : ErrorInfo
	{
		#region Methods
		/// <summary>
		/// Simple file download without any progress processing
		/// </summary>
		/// <param name="url"></param>
		/// <param name="saveLocation"></param>
		public bool DownloadFile(string url, ref string saveLocation)
		{
			Validation.RequireValidString(url, "url");
			File.FileSolution(ref saveLocation);

			try
			{
				Error();

				WebClient client = new WebClient();
				client.DownloadFile(url, saveLocation);
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return IsError;
		}

		public bool UploadFile(string url, string fileToUpload)
		{
			Validation.RequireValidString(url, "url");
			Validation.RequireValidString(fileToUpload, "fileToUpload");

			if (!File.IsExist(fileToUpload))
			{
				throw new Exception(string.Format(
					Properties.Resources.IO_ERROR_FILE_NOT_EXIST,
					fileToUpload));
			}

			try
			{
				Error();

				WebClient client = new WebClient();
				client.UploadFile(url, fileToUpload);
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return IsError;
		}

		public bool UploadFile(string url, string fileToUpload, HttpMethod method)
		{
			Validation.RequireValidString(url, "url");
			Validation.RequireValidString(fileToUpload, "fileToUpload");

			if (!File.IsExist(fileToUpload))
			{
				throw new Exception(string.Format(
					Properties.Resources.IO_ERROR_FILE_NOT_EXIST,
					fileToUpload));
			}

			try
			{
				Error();

				WebClient client = new WebClient();
				client.UploadFile(
					url,
					method == HttpMethod.Get ? "GET" : "POST",
					fileToUpload);
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return IsError;
		}

		#endregion

		#region Helpers
		public static bool GetSize(string url, out long size)
		{
			WebClient client = new WebClient();
			Stream strm = client.OpenRead(url);

			if (strm.CanSeek)
			{
				size = (strm.Seek((long)0, SeekOrigin.End) + 1);
				return true;
			}
			else
			{
				size = -1;
				return false;
			}
		}

		#endregion
	}
}
