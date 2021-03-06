﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Extensions
{
	public static class FloatExtensions
	{
		public static string ToSqlSafeText (this float v)
		{
			return v.ToString ().Replace (",", ".");
		}

		public static string ToSqlField (this float v)
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
