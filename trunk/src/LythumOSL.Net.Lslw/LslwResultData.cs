using System;
using System.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

using LythumOSL.Core.Data.Xml;

namespace LythumOSL.Net.Lslw
{
	public class LslwResultData
	{
		public string ErrorText { get; set; }
		public bool Error { get; set; }
		public DataSet DataSet { get; set; }

		public LslwResultData ()
		{
			Error = false;
			ErrorText = string.Empty;
		}
	}
}
