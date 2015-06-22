using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Reporting.Metadata;

namespace LythumOSL.Reporting
{
	public class Report : IReport
	{
		#region IReport Members

		public string Name { get; set; }
		public object DataSource { get; set; }
		public Dictionary<string, object> Parameters { get; protected set; }

		#endregion

		public Report ()
		{
			Name = string.Empty;
			DataSource = null;
			Parameters = new Dictionary<string, object> ();
		}
	}
}
