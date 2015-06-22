using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LythumOSL.Core
{
	public partial class Helpers
	{

		public static string DefaultDateFormat
		{
			get
			{
				return Resources.Defaults.DefaultDateFormat;
			}
		}

		public static string DefaultShortDateFormat
		{
			get
			{
				return Resources.Defaults.DefaultShortDateFormat;
			}
		}

		public static string[] GetEnumNames (Type enumType, bool sort)
		{
			string[] retVal = Enum.GetNames (enumType);

			return retVal;
		}

		public static bool IsNumeric (string text)
		{
			try
			{
				double value = 0;

				return double.TryParse (text, out value);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Removes everything wat is not number
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string RemoveNonNumeric (string text)
		{
			string retVal = string.Empty;

			foreach (char c in text)
			{
				if (c >= '0' && c <= '9')
				{
					retVal += c;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Function removes more then 1 space symbol in string
		/// leave only 1 space 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string TrimMultipleSpaces (string data)
		{
			string retVal = string.Empty;
			bool spaceFound = false;

			foreach (char c in data)
			{
				if (c == ' ')
				{
					if (!spaceFound)
					{
						retVal += c;
						spaceFound = true;
					}
				}
				else
				{
					retVal += c;
					spaceFound = false;
				}
			}

			return retVal;
		}

		public static bool RangeIntersect (int start1, int end1, int start2, int end2)
		{
			return (end2 >= start1) && (start2 <= end1); 
		}

		public static string ApplicationName
		{
			get
			{
				if (Application.Current == null)
				{
					throw new LythumException ("Application.Current is NULL, possibly WPF application is not active!");
				}

				return Application.Current.ToString ();
			}
		}
	}
}
