using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using CrystalDecisions.CrystalReports.Engine;

using LythumOSL.Core.Data;
using LythumOSL.Reporting.Metadata;

namespace LythumOSL.Reporting.CR.Metadata
{
	public interface IReportCR : IReport
	{
		// properties
		// methods
		ReportDocument GetReport ();
		void DestroyReport ();
	}
}
