using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Net.Http
{
	public class HttpProxySettings : ILythumBase
	{
		#region Attributes
		private bool _UseProxy;
		private string _ProxyAddress;
		private string _ProxyUsername;
		private string _ProxyPassword;

		#endregion

		#region Properties

		[Category("Proxy server"), DisplayName("Use proxy"), Description("Set it to True if to use proxy")]
		public bool UseProxy
		{
			get { return _UseProxy; }
			set { _UseProxy = value; }
		}

		[Category ("Proxy server"), DisplayName ("Proxy server"), Description ("Proxy server address")]
		public string ProxyAddress
		{
			get { return _ProxyAddress; }
			set { _ProxyAddress = value; }
		}

		[Category ("Proxy server"), DisplayName ("Username"), Description ("Proxy server username")]
		public string ProxyUsername
		{
			get { return _ProxyUsername; }
			set { _ProxyUsername = value; }
		}

		[Category ("Proxy server"), DisplayName ("Password"), Description ("Proxy server password")]
		public string ProxyPassword
		{
			get { return _ProxyPassword; }
			set { _ProxyPassword = value; }
		}

		#endregion

		#region ctor

		public HttpProxySettings ()
		{
			_UseProxy = false;
		}

		public HttpProxySettings (
			string address,
			string username,
			string password)
		{
			_UseProxy = true;
			_ProxyAddress = address;
			_ProxyUsername = username;
			_ProxyPassword = password;
		}

		#endregion
	}
}
