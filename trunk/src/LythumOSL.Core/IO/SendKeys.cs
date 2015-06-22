using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using LythumOSL.Core.Win32;

namespace LythumOSL.Core.IO
{
	public class SendKeys
	{
		//public static void SendString (IntPtr hWnd, string value)
		//{
		//    foreach (char c in value)
		//    {
		//        SendChar (hWnd, c);
		//    }
		//}

		public static void SendChar (IntPtr hWnd, int key)
		{
			User.PostMessage (hWnd, User.WM_KEYDOWN, (int)key, 0);
			User.PostMessage (hWnd, User.WM_CHAR, (int)key, 0);
			User.PostMessage (hWnd, User.WM_KEYUP, (int)key, 0);
		}
	}
}
