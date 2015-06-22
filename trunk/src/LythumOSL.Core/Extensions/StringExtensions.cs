using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Extensions
{
	public static class StringExtensions
	{
		public static string ToSqlSafeText (this string value)
		{
			if (string.IsNullOrEmpty (value))
			{
				return Constants.NullValue;
			}
			else
			{
				return value.Replace ("'", "''")
					.TrimEnd (' ');
			}
		}

		public static string ToSqlField (this string v)
		{
			string retVal = ToSqlSafeText (v);

			if (Constants.NullValue.Equals (retVal))
			{
				return retVal;
			}
			else
			{
				return Constants.FieldPrefix + retVal + Constants.FieldPostfix;
			}
		}

		/// <summary>
		/// Method checks for null, do this is int and for less than 1 value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsIntNonZero (this string value)
		{
			if (string.IsNullOrEmpty (value))
				return false;

			int result;
			if (int.TryParse (value, out result))
			{
				return result > 0;
			}
			else
			{
				return false;
			}
		}

		public static bool ToBoolean (this string value)
		{
			if (string.IsNullOrEmpty (value))
			{
				return false;
			}
			else if (value.Equals ("1") || value.Equals ("true", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}

			return false;
		}
	}
}
