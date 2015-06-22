using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Shell
{
	public class Helpers
	{
		public static void ShellExecute (string file)
		{
			Process proc =
				new System.Diagnostics.Process ();

			proc.EnableRaisingEvents = false;
			proc.StartInfo.FileName = file;
			proc.Start ();
		}
	}
}
