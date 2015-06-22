using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using LythumOSL.Core.Net.Http;

namespace LythumOSL.Net.Lslw
{
	public class LslwSettings : HttpProxySettings
	{
		string _WebserviceUrl;

		[Category("Web service"), DisplayName("Webservice Url")]
		public string WebserviceUrl
		{
			get { return _WebserviceUrl; }
			set { _WebserviceUrl = value; }
		}
	}
}
