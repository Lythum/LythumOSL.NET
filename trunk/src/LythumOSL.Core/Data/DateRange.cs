using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	public class DateRange : ILythumBase
	{
		public enum DateRanges
		{
			Today,
			Tomorrow,
			Yesterday,
			ThisWeek,
			NextWeek,
			PreviousWeek,
			ThisMonth,
			NextMonth,
			PreviousMonth,
			ThisYear,
			NextYear,
			PreviousYear,
		}


		public DateTime ValueFrom;
		public DateTime ValueTo;

		static Dictionary<DayOfWeek, int> DayMap = new Dictionary<DayOfWeek, int> ()
		{
			{DayOfWeek.Monday, 0},
			{DayOfWeek.Tuesday, -1},
			{DayOfWeek.Wednesday, -2},
			{DayOfWeek.Thursday, -3},
			{DayOfWeek.Friday, -4},
			{DayOfWeek.Saturday, -5},
			{DayOfWeek.Sunday, -6},
		};

		public DateRange ()
		{
		}

		public DateRange (DateTime from, DateTime to)
			: this ()
		{
		}

		#region Static stub

		#region Misc

		public static DateRange GetDateRangeByEnum (DateTime date, DateRanges range)
		{
			switch (range)
			{
				default:
				case DateRanges.Today:
					return ToRangeToday (date);
				case DateRanges.Tomorrow:
					return ToRangeTomorrow (date);
				case DateRanges.Yesterday:
					return ToRangeYesterday (date);

				case DateRanges.ThisWeek:
					return ToRangeThisWeek (date);
				case DateRanges.NextWeek:
					return ToRangeNextWeek (date);
				case DateRanges.PreviousWeek:
					return ToRangePreviousWeek (date);

				case DateRanges.ThisMonth:
					return ToRangeThisMonth (date);
				case DateRanges.NextMonth:
					return ToRangeNextMonth (date);
				case DateRanges.PreviousMonth:
					return ToRangePreviousMonth (date);

				case DateRanges.ThisYear:
					return ToRangeThisYear (date);
				case DateRanges.NextYear:
					return ToRangeNextYear (date);
				case DateRanges.PreviousYear:
					return ToRangePreviousYear (date);

			}
		}


		#endregion

		#region Days
		public static DateRange ToRangeToday (DateTime date)
		{
			DateRange retVal = new DateRange ();

			retVal.ValueFrom = ToDateFrom (date);
			retVal.ValueTo = ToDateTo (date);

			return retVal;
		}

		public static DateRange ToRangeTomorrow (DateTime date)
		{
			return ToRangeToday (date.AddDays (1));
		}

		public static DateRange ToRangeYesterday (DateTime date)
		{
			return ToRangeToday (date.AddDays (-1));
		}

		#endregion

		#region Weeks

		public static DateRange ToRangeThisWeek (DateTime date)
		{
			DateRange retVal = new DateRange ();

			retVal.ValueFrom = ToDateFrom (GetMonday (date));
			retVal.ValueTo = ToDateTo (retVal.ValueFrom.AddDays (6));

			return retVal;
		}

		public static DateRange ToRangeNextWeek (DateTime date)
		{
			return ToRangeThisWeek (date.AddDays (7));
		}

		public static DateRange ToRangePreviousWeek (DateTime date)
		{
			return ToRangeThisWeek (date.AddDays (-7));
		}

		#endregion

		#region Months

		public static DateRange ToRangeThisMonth (DateTime date)
		{
			DateRange retVal = new DateRange ();

			retVal.ValueFrom = ToDateFrom (new DateTime (date.Year, date.Month, 1));
			retVal.ValueTo = ToDateTo(retVal.ValueFrom.AddMonths(1).AddDays(-1));

			return retVal;
		}

		public static DateRange ToRangeNextMonth (DateTime date)
		{
			return ToRangeThisMonth (GetMonthFirstDay (date).AddMonths (1));
		}

		public static DateRange ToRangePreviousMonth (DateTime date)
		{
			return ToRangeThisMonth (GetMonthFirstDay (date).AddMonths (-1));
		}

		#endregion

		#region Year

		public static DateRange ToRangeThisYear (DateTime date)
		{
			DateRange retVal = new DateRange ();

			retVal.ValueFrom = ToDateFrom (GetYearFirstDay (date));
			retVal.ValueTo = ToDateTo (retVal.ValueFrom.AddYears (1).AddDays (-1));

			return retVal;
		}

		public static DateRange ToRangeNextYear (DateTime date)
		{
			return ToRangeThisYear (GetYearFirstDay (date).AddYears (1)); 
		}

		public static DateRange ToRangePreviousYear (DateTime date)
		{
			return ToRangeThisYear (GetYearFirstDay (date).AddYears (-1)); 
		}

		#endregion

		#endregion

		#region Helpers
		static DateTime ToDateFrom (DateTime date)
		{
			return new DateTime (date.Year, date.Month, date.Day, 0, 0, 0);
		}

		static DateTime ToDateTo (DateTime date)
		{
			return new DateTime (date.Year, date.Month, date.Day, 23, 59, 59);
		}

		static DateTime GetYearFirstDay (DateTime date)
		{
			return new DateTime (date.Year, 1, 1);
		}

		static DateTime GetMonthFirstDay (DateTime date)
		{
			return new DateTime (date.Year, date.Month, 1);
		}

		static DateTime GetMonday (DateTime date)
		{
			int offset = DayMap[date.DayOfWeek];
			if (offset != 0)
			{
				return date.AddDays (offset);
			}
			else
			{
				return date;
			}

		}

		#endregion
	}
}
