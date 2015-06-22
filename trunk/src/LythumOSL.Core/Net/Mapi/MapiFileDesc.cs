using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LythumOSL.Core.Net.Mapi
{
	[StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public class MapiFileDesc
	{
		public int reserved;
		public int flags;
		public int position;
		public string path;
		public string name;
		public IntPtr type;
	}
}
