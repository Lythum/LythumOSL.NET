using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Reporting.Metadata
{
	public interface IReport
	{
		// properties
		string Name { get; set; }
		object DataSource { get; set; }
		Dictionary<string, object> Parameters { get; }
	}
}
