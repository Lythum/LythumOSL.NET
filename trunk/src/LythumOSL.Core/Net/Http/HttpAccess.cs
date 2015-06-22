using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Net.Http
{
	public class HttpAccess : ILythumBase
	{
		#region Constants and enums
		const string MethodGet = "GET";
		const string MethodPost = "POST";

		public enum HttpMethods
		{
			Post,
			Get,
		}

		#endregion

		#region Attributes
		CookieContainer _Cookies;
		HttpProxySettings _Settings;

		#endregion

		#region Properties

		public CookieContainer Cookies
		{
			get { return _Cookies; }
			set { _Cookies = value; }
		}

		#endregion

		#region ctor

		public HttpAccess ()
		{
			_Cookies = new CookieContainer ();
			_Settings = new HttpProxySettings ();
		}

		public HttpAccess (HttpProxySettings settings)
		{
			_Cookies = new CookieContainer ();
			_Settings = settings;
		}

		#endregion

		#region Methods

		public string Request (string url)
		{
			return Request (url, (byte[])null);
		}

		public string Request (string url, HttpAttributes postAttributes)
		{
			if (postAttributes != null)
			{
				return Request (url, postAttributes.Render ());
			}
			else
			{
				return Request (url, (byte[])null);
			}
		}

		public string Request (string url, string postData)
		{
			if (string.IsNullOrEmpty (postData))
			{
				return Request (url, (byte[])null);
			}
			else
			{
				return Request (
					url, Encoding.UTF8.GetBytes (postData));
			}
		}

		public string Request (string url, byte[] postData)
		{
			string retVal = string.Empty;

			HttpWebRequest request = InitWebRequest (
				url, (postData == null ? HttpMethods.Get : HttpMethods.Post));
			
			request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322)";

			if (postData != null)
			{
				request.ContentType = @"application/x-www-form-urlencoded";
				request.ContentLength = postData.Length;

				Stream postStream = request.GetRequestStream ();
				postStream.Write (postData, 0, postData.Length);
				postStream.Close ();

			}

			WebResponse response = request.GetResponse ();
			Stream answerStream = response.GetResponseStream ();
			StreamReader answerReader = new StreamReader (answerStream);
			retVal = answerReader.ReadToEnd ();

			answerReader.Close ();
			answerStream.Close ();
			response.Close ();

			return retVal;
		}

		#endregion

		#region Helpers
		HttpWebRequest InitWebRequest (string url, HttpMethods method)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);

			if (_Settings.UseProxy)
			{
				WebProxy proxy = new WebProxy ();
				Uri uri = new Uri (_Settings.ProxyAddress);

				proxy.Address = uri;
				proxy.Credentials = new NetworkCredential (
					_Settings.ProxyUsername, _Settings.ProxyPassword);
				request.Proxy = proxy;
			}

			switch (method)
			{
				// post
				case HttpMethods.Post:
					request.Method = MethodPost;
					break;

				// get
				default:
					request.Method = MethodGet;
					break;
			}

			request.CookieContainer = Cookies;

			return request;

		}


		#endregion

	}
}
