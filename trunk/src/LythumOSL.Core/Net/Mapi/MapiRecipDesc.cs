using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LythumOSL.Core.Net.Mapi
{
	[StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public class MapiRecipDesc
	{
		public int reserved;
		public int recipClass;
		public string name;
		public string address;
		public int eIDSize;
		public IntPtr entryID;
	}
}
