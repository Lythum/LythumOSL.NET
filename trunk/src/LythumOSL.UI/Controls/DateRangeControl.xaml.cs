using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LythumOSL.Core.Data;

namespace LythumOSL.UI.Controls
{
	/// <summary>
	/// Interaction logic for DateRange.xaml
	/// </summary>
	public partial class DateRangeControl : UserControl
	{
		public DateTime ValueFrom
		{
			get
			{
				return DtpFrom.SelectedDate.Value;
			}
			set
			{
				DtpFrom.SelectedDate = value;
			}
		}

		public DateTime ValueTo
		{
			get
			{
				return DtpTo.SelectedDate.Value;
			}
			set
			{
				DtpTo.SelectedDate = value;
			}
		}

		Dictionary<DateRange.DateRanges, string> sRanges = new Dictionary<DateRange.DateRanges, string> ()
		{
			{ DateRange.DateRanges.Today, Properties.Resources.DateRangeToday },
			{ DateRange.DateRanges.Tomorrow, Properties.Resources.DateRangeTomorrow},
			{ DateRange.DateRanges.Yesterday,  Properties.Resources.DateRangeYesterday},
			{ DateRange.DateRanges.ThisWeek,Properties.Resources.DateRangeThisWeek},
			{ DateRange.DateRanges.NextWeek, Properties.Resources.DateRangeNextWeek},
			{ DateRange.DateRanges.PreviousWeek, Properties.Resources.DateRangePreviousWeek},
			{ DateRange.DateRanges.ThisMonth, Properties.Resources.DateRangeThisMonth},
			{ DateRange.DateRanges.NextMonth, Properties.Resources.DateRangeNextMonth},
			{ DateRange.DateRanges.PreviousMonth,Properties.Resources.DateRangePreviousMonth},
			{ DateRange.DateRanges.ThisYear,Properties.Resources.DateRangeThisYear},
			{ DateRange.DateRanges.NextYear, Properties.Resources.DateRangeNextYear},
			{ DateRange.DateRanges.PreviousYear, Properties.Resources.DateRangePreviousYear},
		};


		public DateRangeControl ()
		{
			InitializeComponent ();
			InitUI ();
		}

		void InitUI ()
		{
			CmbDays.ItemsSource = sRanges;
			Reset ();
		}

		public void Reset (int index)
		{
			if (index < CmbDays.Items.Count && index > -1)
			{
				CmbDays.SelectedIndex = index;
			}
		}
		public void Reset ()
		{
			Reset (0);
		}

		private void CmbDays_SelectionChanged (object sender, SelectionChangedEventArgs e)
		{
			if (CmbDays.SelectedValue is KeyValuePair<DateRange.DateRanges, string>)
			{

				KeyValuePair<DateRange.DateRanges, string> pair =
					(KeyValuePair<DateRange.DateRanges, string>)CmbDays.SelectedValue;

				DateRange rangeValue = DateRange.GetDateRangeByEnum (DateTime.Now, pair.Key);

				ValueFrom = rangeValue.ValueFrom;
				ValueTo = rangeValue.ValueTo;
			}
		}


	}
}
