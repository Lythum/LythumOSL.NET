using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LythumOSL.Wpf.Converters
{
	public class NullToDbNullConverter : IValueConverter
	{

		const object NullValue = null;

		#region IValueConverter Members

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return DBNull.Value;
			}
			else
			{
				return value;
			}
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return DBNull.Value;
			}
			else
			{
				return value;
			}
		}

		#endregion

	}
}
