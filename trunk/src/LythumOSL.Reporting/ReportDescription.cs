using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Reporting.Metadata;

namespace LythumOSL.Reporting
{
	public class ReportDescription : IReportDescription
	{
		#region IReportDescription Members
		
		public string Name { get; set; }
		public string Description { get; set; }

		public bool IsDateNeeded { get; set; }
		public bool IsClientNeeded { get; set; }
		public bool IsUserNeeded { get; set;  }

		#endregion

	}
}
