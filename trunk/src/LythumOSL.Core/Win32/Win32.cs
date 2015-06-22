//#define ADVANCED_WIN32

using System;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace LythumOSL.Core.Win32
{

#if ADVANCED_WIN32
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RECT(int left, int right, int top, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
		public Rectangle ToRectangle()
		{
			return new Rectangle(Left, Top, Right - Left, Bottom - Top);
		}
		public static RECT FromRectangle(Rectangle rectangle)
		{
			if (rectangle == null)
				return new RECT();
			return new RECT(rectangle.Left, rectangle.Right, rectangle.Top, rectangle.Bottom);
		}
		public override string ToString()
		{
			return string.Format("{{ X = {0} Y = {1} Width = {2} Height = {3} }}",
			   Left, Top, Right - Left, Bottom - Top);
		}
	}
#else
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}
#endif

	public struct POINT
	{
		public int x;
		public int y;
	}
	public struct SIZE
	{
		public int cx;
		public int cy;
	}
	public struct FILETIME
	{
		public int dwLowDateTime;
		public int dwHighDateTime;
	}
	public struct SYSTEMTIME
	{
		public short wYear;
		public short wMonth;
		public short wDayOfWeek;
		public short wDay;
		public short wHour;
		public short wMinute;
		public short wSecond;
		public short wMilliseconds;
	}
}