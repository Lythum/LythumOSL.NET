using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Extensions
{
	public static class DecimalExtensions
	{
		public static string ToSqlSafeText (this decimal v)
		{
			if (v == null)
			{
				return Constants.NullValue;
			}
			else
			{
				return v.ToString ().Replace (",", ".");
			}
		}

		public static string ToSqlField (this decimal v)
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
	}
}
