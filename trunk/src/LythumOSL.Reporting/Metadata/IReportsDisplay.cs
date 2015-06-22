using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Reporting.Metadata
{
	public interface IReportsDisplay
	{
		List<IReport> Reports { get; }
	}
}
