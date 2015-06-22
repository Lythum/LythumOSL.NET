using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using CrystalDecisions.CrystalReports.Engine;

using LythumOSL.Core;
using LythumOSL.Core.Data;
using LythumOSL.Reporting;

using LythumOSL.Reporting.CR.Metadata;

namespace LythumOSL.Reporting.CR
{
	public class ReportCR<T> : Report, IReportCR
		where T : ReportDocument, new ()
	{

		#region Attributes

		T _Rpt = null;

		#endregion

		#region CTOR

		public ReportCR (string name)
			: base ()
		{
			Validation.RequireValidString (name, "name");

			this.Name = name;
		}

		public ReportCR (
			string name,
			object dataSource)
			: this (name)
		{
			this.DataSource = dataSource;
		}

		public ReportCR (
			string name,
			object dataSource,
			Dictionary<string, object> parameters)
			: this (name, dataSource)
		{
			Validation.RequireValidString (name, "name");

			this.Parameters = parameters;
		}

		#endregion

		#region IReportCR Members

		public virtual ReportDocument GetReport ()
		{
			_Rpt = new T ();

			SetDataSource (_Rpt);

			SetParameters (_Rpt);

			return _Rpt;
		}

		protected void SetDataSource (ReportDocument rpt)
		{
			// Init table/datasource
			if (DataSource != null)
			{
				rpt.SetDataSource (DataSource);
			}
		}

		protected void SetParameters (ReportDocument rpt)
		{
			// init params
			foreach (string n in Parameters.Keys)
			{
				//Debug.Print ("Passed parameter " + n + ", value " + _Parameters[n]);
				rpt.SetParameterValue (n, Parameters[n]);
			}

		}


		public void DestroyReport ()
		{
			if (_Rpt != null)
			{
				_Rpt.Close ();
				_Rpt.Dispose ();
				_Rpt = null;
			}
			
		}

		#endregion

		public override string ToString ()
		{
			return Name;
		}
	}
}
