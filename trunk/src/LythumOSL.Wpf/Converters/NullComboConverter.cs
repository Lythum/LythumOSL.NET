using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace LythumOSL.Wpf.Converters
{
	public class NullComboConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return -1;

			if (value is DBNull || value == DBNull.Value)
			{
				return -1;
			}

			return value;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return DBNull.Value;
			}

			else if (value is Int32)
			{
				if ((int)value == -1)
					return DBNull.Value;
			}

			return value;
		}


		#endregion
	}
}
