using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToValueIfEmpty (this object value, string isNullValue)
		{
			return (value == null ? isNullValue : value.ToString ());
		}

		public static bool TryConvertToDecimal (this object value, out decimal result)
		{
			if (value == null)
			{
				result = -1;
				return false;
			}
			else
			{
				return decimal.TryParse (value.ToString (), out result);
			}
		}

		public static string ToSqlField (this object value)
		{
			if (value == null || DBNull.Value.Equals (value))
			{
				return Constants.NullValue;
			}
			else if (value is decimal)
			{
				return ((decimal)value).ToSqlField ();
			}
			else if (value is double)
			{
				return ((double)value).ToSqlField ();
			}
			else if (value is float)
			{
				return ((float)value).ToSqlField ();
			}
			else
			{
				return value.ToString().ToSqlField ();
			}
		}

		public static bool IsNumericNonZero (this object value)
		{
			double valueDouble;
			if (value == null || DBNull.Value.Equals (value))
			{
				return false;
			}
			else if (!double.TryParse (value.ToString (), out valueDouble))
			{
				return false;
			}
			else if (valueDouble > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
