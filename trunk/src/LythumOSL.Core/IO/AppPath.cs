using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LythumOSL.Core.IO
{
	public class AppPath
	{
		public static string GetCreatePath (
				Environment.SpecialFolder systemFolder)
		{
			string retVal = Environment.GetFolderPath (systemFolder);

			retVal += "\\" + LythumOSL.Core.Helpers.ApplicationName + "\\";

			DirectoryInfo di = new DirectoryInfo (retVal);

			if (!di.Exists)
			{
				di.Create ();
			}

			return retVal;
		}
		public static string GetCreateDocumentsPath ()
		{
			return GetCreatePath (Environment.SpecialFolder.MyDocuments);
		}

		public static string GetCreateDocumentsPath (string subDir)
		{
			string retVal = GetCreatePath (Environment.SpecialFolder.MyDocuments);

			if (!string.IsNullOrEmpty (subDir))
			{
				retVal += subDir + "\\";
			}

			DirectoryInfo di = new DirectoryInfo (retVal);

			if (!di.Exists)
			{
				di.Create ();
			}

			return retVal;
		}



	}
}
