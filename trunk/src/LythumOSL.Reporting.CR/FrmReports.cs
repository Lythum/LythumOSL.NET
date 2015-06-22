using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using LythumOSL.Core;
using LythumOSL.UI;
using LythumOSL.Reporting.CR.Metadata;

namespace LythumOSL.Reporting.CR
{
	public partial class FrmReports : Form
	{
		#region Nested export type class
		public class ReportExportFormat
		{
			public ExportFormatType Type;
			public string Name;
			public string Ext;

			public ReportExportFormat (
				ExportFormatType type, 
				string name,
				string ext)
			{
				Type = type;
				Name = name;
				Ext = ext;
			}

			public override string ToString ()
			{
				return Name;
			}
		}

		#endregion

		List<IReportCR> _Reports;
		int _Copies;

		IReportCR _LastReport = null;

		public FrmReports (
				string name,
				List<IReportCR> reports, 
				int copies)
		{
			InitializeComponent ();

			Text = name;

			Validation.RequireValid (reports, "reports");

			_Reports = reports;
			_Copies = copies;

			TxtCopiesCount.Text = _Copies.ToString ();

			Init ();
			InitExport ();

		}

		void Init ()
		{
			foreach (IReportCR r in _Reports)
			{
				LbxReports.Items.Add (r);

				//ListViewItem i = new ListViewItem (r.Name);
				//i.Tag = r;
				//LvwList.Items.Add (i);

				////CmbPrinter.Text = r.Report.PrintOptions.PrinterName;

				//if (first == null)
				//{
				//    first = i;
				//}
			}

			if (LbxReports.Items.Count > 0)
			{
				LbxReports.SelectedIndex = 0;
			}

			//if (first != null)
			//{
			//    first.Selected = true;
			//}
		}

		void InitExport ()
		{
			CmbExportFormat.Items.Clear ();
			CmbExportFormat.Items.Add (new ReportExportFormat (
				ExportFormatType.WordForWindows, Properties.Resources.FrmReport_ExportFormat_DOC, "doc"));
			CmbExportFormat.Items.Add (new ReportExportFormat (
				ExportFormatType.Excel, Properties.Resources.FrmReport_ExportFormat_Excel, "xls"));
			CmbExportFormat.Items.Add (new ReportExportFormat (
				ExportFormatType.PortableDocFormat, Properties.Resources.FrmReport_ExportFormat_PDF, "pdf"));
			CmbExportFormat.Items.Add (new ReportExportFormat (
				ExportFormatType.HTML40, Properties.Resources.FrmReport_ExportFormat_HTML, "html"));
			CmbExportFormat.Items.Add (new ReportExportFormat (
				ExportFormatType.RichText, Properties.Resources.FrmReport_ExportFormat_RTF, "rtf"));

			if (CmbExportFormat.Items.Count > 0)
			{
				CmbExportFormat.SelectedIndex = 0;
			}
		}

		void Print (ReportDocument rpt)
		{
			//rpt.PrintOptions.PrinterName = CmbPrinter.Text;
			rpt.PrintToPrinter (_Copies, false, 0, 0);
		}

		private void LvwList_SelectedIndexChanged (object sender, EventArgs e)
		{
			//if (LvwList.SelectedItems.Count > 0)
			if (LbxReports.SelectedItems.Count > 0)
			{
				if (_LastReport != null)
				{
					_LastReport.DestroyReport ();
				}

				//_LastReport = (IReportCR)LvwList.SelectedItems[0].Tag;
				_LastReport = LbxReports.SelectedItem as IReportCR;

				CrvMain.ReportSource = _LastReport.GetReport ();
				//CrvMain.Refresh ();

				CrvMain.Enabled = CmdPrintAll.Enabled = CmdPrintOne.Enabled = true;

			}
			else
			{
				CrvMain.Enabled = CmdPrintAll.Enabled = CmdPrintOne.Enabled = false;
			}
		}

		private void CmdPrintOne_Click (object sender, EventArgs e)
		{
			//foreach (ListViewItem i in LvwList.SelectedItems)
			foreach (object o in LbxReports.SelectedItems)
			{
				IReportCR rpt = o as IReportCR;

				Print (rpt.GetReport ());

				rpt.DestroyReport ();
			}
		}

		private void CmdPrintAll_Click (object sender, EventArgs e)
		{
			foreach (object o in LbxReports.SelectedItems)
			{
				IReportCR rpt = o as IReportCR;

				Print (rpt.GetReport());

				rpt.DestroyReport ();
			}
		}

		private void FrmReports_Load (object sender, EventArgs e)
		{
			/*
			// Enum installed printers
			foreach (string printer in PrinterSettings.InstalledPrinters)
			{
				CmbPrinter.Items.Add (printer);
			}
			 */
		}

		private void FrmReports_Resize (object sender, EventArgs e)
		{
			//if(LbxReports.Columns.Count>0){
			//    LbxReports.Columns[0].Width = LvwList.Width - 10;
			//}
		}

		private void TxtCopiesCount_TextChanged (object sender, EventArgs e)
		{
			int copies = _Copies;
			try
			{
				_Copies = int.Parse (TxtCopiesCount.Text);
			}
			catch
			{
				_Copies = copies;
			}
		}

		#region EXPORT

		private void CmdExport_Click (object sender, EventArgs e)
		{
			try
			{
				this.Enabled = false;

				ExportFormatType exportFormat = ((ReportExportFormat)CmbExportFormat.SelectedItem).Type;
				string exportPath = "c:\\";
				string exportExt = "." + ((ReportExportFormat)CmbExportFormat.SelectedItem).Ext;

				FolderBrowserDialog d = new FolderBrowserDialog ();
				d.ShowNewFolderButton = true;
				d.Description = Properties.Resources.FrmReport_ExportFolderTitle;
				d.RootFolder = Environment.SpecialFolder.MyComputer;
				if (d.ShowDialog () == DialogResult.OK)
				{
					if (Directory.Exists (d.SelectedPath))
					{
						exportPath = d.SelectedPath;
					}
				}
				else
				{
					return;
				}


				if (LbxReports.SelectedItems.Count > 0)
				{
					foreach (object o in LbxReports.SelectedItems)
					{
						IReportCR rpt = o as IReportCR;

						ReportDocument rep = rpt.GetReport ();
						rep.ExportToDisk (
							exportFormat,
							exportPath + "\\" +
							LythumOSL.Core.IO.File.FixFileName (rpt.Name)
								.Replace ('\\', '-')
								.Replace ('/', '-') +
								exportExt);
					}

					Messages.Warning (
						Properties.Resources.FrmReport_ExportFinished);

				}
			}
			catch (Exception ex)
			{
				Messages.Error (ex.Message, Text);
			}
			finally
			{
				this.Enabled = true;
			}
		}
		#endregion

		private void TxtCopiesCount_Validating (object sender, CancelEventArgs e)
		{
			int result;
			e.Cancel = int.TryParse (TxtCopiesCount.Text, out result);
		}

	}
}