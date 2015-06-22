using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace LythumOSL.Core.Extensions
{
	public static class DateTimeExtensions
	{
		public class DateRange
		{
			public DateTime DateFrom { get; set; }
			public DateTime DateTo { get; set; }
		}

		public static DateTime Yesterday (this DateTime date)
		{
			return date.AddDays (-1);
		}

		public static DateTime Tomorrow (this DateTime date)
		{
			return date.AddDays (1);
		}

		/// <summary>
		/// Returns same date, just with zero time, used for search system
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime ToFrom (this DateTime date)
		{
			return new DateTime (date.Year, date.Month, date.Day, 0, 0, 0);
		}

		/// <summary>
		/// Returns same date, just with 23:59:59 time, used for search system
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime ToTo (this DateTime date)
		{
			return new DateTime (date.Year, date.Month, date.Day, 23, 59, 59);
		}

		public static DateRange ToRangeMonth (this DateTime date)
		{
			DateRange retVal = new DateRange ();

			// calculating first month day with 00 tim
			retVal.DateFrom = new DateTime (date.Year, date.Month, 1, 0, 0, 0);

			// calculating last month day
			DateTime lastDay = retVal.DateFrom.AddMonths (1).AddDays (-1);

			// creating last month day with last hour, mins and seconds
			retVal.DateTo = new DateTime (lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);

			return retVal;
		}

		public static DateRange ToRangeYear (this DateTime date)
		{
			DateRange retVal = new DateRange ();

			retVal.DateFrom = new DateTime (date.Year, 1, 1);

			DateTime lastDay = retVal.DateFrom.AddYears (1).AddDays (-1);

			retVal.DateTo = new DateTime (lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59); 

			return retVal;
		}
	}
}
