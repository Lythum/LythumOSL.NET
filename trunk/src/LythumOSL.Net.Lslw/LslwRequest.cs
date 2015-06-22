using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using LythumOSL.Core;

namespace LythumOSL.Net.Lslw
{
	[Serializable]
	public class LslwRequest
	{
		#region Const
		const string GUID_QUERY_SEPARATOR = "{133C72CC-0655-41d6-A062-CAF0C67E75A0}";

		#endregion

		#region Attributes

		DateTime _TimeStamp;
		LslwRequestType _Type;
		string _Command;
		string[] _Data;

		#endregion

		#region properties

		public DateTime TimeStamp
		{
			get { return _TimeStamp; }
			set { _TimeStamp = value; }
		}

		public LslwRequestType Type
		{
			get { return _Type; }
			set { _Type = value; }
		}

		public string Command
		{
			get { return _Command; }
			set { _Command = value; }
		}

		public string[] Data
		{
			get { return _Data; }
			set { _Data = value; }
		}

		#endregion

		#region STATIC
		public static string PrepareMultiqueryRequest (List<string> queries)
		{
			Validation.RequireValid (queries, "queries");

			return PrepareMultiqueryRequest (queries.ToArray ());
		}

		public static string PrepareMultiqueryRequest (string[] queries)
		{
			Validation.RequireValid (queries, "queries");

			string retVal = string.Join (GUID_QUERY_SEPARATOR + "\r\n", queries);

			System.Diagnostics.Debug.Print (retVal);

			return retVal;
		}

		#endregion
	}
}
