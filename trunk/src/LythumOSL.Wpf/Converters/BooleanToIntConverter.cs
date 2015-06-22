using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LythumOSL.Wpf.Converters
{
	public class BooleanToIntConverter : IValueConverter
	{
			object _TrueValue;
			object _FalseValue;

			public BooleanToIntConverter ()
			{
				_TrueValue = 1;
				_FalseValue = 0;
			}

			#region IValueConverter Members

			public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				if (value != null)
				{
					if (value.Equals (_TrueValue))
						return true;
					else
						return false;
				}
				else
				{
					return false;
				}
			}

			public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				if (value != null && value is bool)
				{
					if ((bool)value == true)
						return _TrueValue;
					else
						return _FalseValue;
				}
				else
				{
					return _FalseValue;
				}
			}

			#endregion

	}
}
