using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Net.Http
{
	public class HttpAttributes
	{
		#region Attributes
		Dictionary<string, string> _Attributes;

		#endregion

		#region Properties

		public string this[string name]
		{
			get
			{
				if (_Attributes.ContainsKey (name))
				{
					return _Attributes[name];
				}

				return string.Empty;
			}

			set
			{
				if (_Attributes.ContainsKey (name))
				{
					_Attributes[name] = value;
				}
				else
				{
					_Attributes.Add (name, value);
				}
			}
		}

		#endregion

		#region ctor

		public HttpAttributes ()
		{
			_Attributes = new Dictionary<string, string> ();
		}

		#endregion

		#region Methods
		public string Render ()
		{
			string retVal = string.Empty;
			bool first = true;

			foreach (string key in _Attributes.Keys)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					retVal += "&";
				}

				retVal += key + "=" + _Attributes[key];
			}

			return retVal;
		}

		#endregion

	}
}
