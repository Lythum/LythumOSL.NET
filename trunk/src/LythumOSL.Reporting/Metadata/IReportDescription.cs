using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Reporting.Metadata
{
	public interface IReportDescription
	{
		string Name { get; }
		string Description { get; }

		bool IsDateNeeded { get; }
		bool IsClientNeeded { get; }
		bool IsUserNeeded { get; }
	}
}
