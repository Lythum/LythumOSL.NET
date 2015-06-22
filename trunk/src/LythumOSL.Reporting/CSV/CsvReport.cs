using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Core.IO;

namespace LythumOSL.Reporting.CSV
{
	public class CsvReport
	{
		string _FileName;
		string _FullFilePath;

		public CsvReport ()
			: this (Guid.NewGuid ().ToString () + ".csv")
		{

		}

		public CsvReport (
			string fileName)
		{
			Validation.RequireValidString (fileName, "fileName");

			_FileName = fileName;

			InitPath ();
		}

		void InitPath ()
		{
			string path = Environment.GetFolderPath (
				Environment.SpecialFolder.MyDocuments);

			_FullFilePath = path + "\\" + _FileName + "_" + DateTime.Now.ToString (@"yyyy-MM-dd_hh-mm-ss") + ".csv";
		}

		public void Write (string text)
		{
			File.Write (_FullFilePath, text, true);
		}

		public void Write (DataTable table, string separator, bool withColumnNames, bool skipUnderscores)
		{
			File.Write (_FullFilePath, table, separator, true, withColumnNames, skipUnderscores);
		}

		public void WriteLine (string text)
		{
			File.Write (_FullFilePath, text + "\r\n", true);
		}

		public void WriteLine ()
		{
			WriteLine ("\r\n");
		}

		public void Populate ()
		{
			try
			{
				LythumOSL.Core.Shell.Helpers.ShellExecute (_FullFilePath);
			}
			catch
			{
			}
		}
	}
}
