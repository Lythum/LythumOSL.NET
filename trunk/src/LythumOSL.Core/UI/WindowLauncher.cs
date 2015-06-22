using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LythumOSL.Core.UI
{
	public class WindowLauncher
	{
		public static bool Launch (Window w, bool modal)
		{
			if (w != null)
			{
				if (modal)
				{
					bool? retVal = w.ShowDialog ();

					if (retVal.HasValue)
					{
						return retVal.Value;
					}
				}
				else
				{
					w.Show ();
				}
			}
			return false;
		}
	}
}
