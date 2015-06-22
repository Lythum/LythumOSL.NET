using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace LythumOSL.Core.Data.Converters
{
	public abstract class StateToBrushConverter : IValueConverter
	{
		public StateToBrushConverter ()
		{
		}

		#region IValueConverter Members

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Brush retVal = Brushes.WhiteSmoke;

			Dictionary<int, Brush> dic = GetWorkingDictionary ();

			if (value != null)
			{
				int intVal;
				if (int.TryParse (value.ToString (), out intVal))
				{
					if (dic.ContainsKey (intVal))
					{
						retVal = dic[intVal];
					}
				}
			}

			return retVal;
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int retVal = 10;

			Dictionary<int, Brush> dic = GetWorkingDictionary ();

			if (value != null && value is Brush)
			{
				Brush brush = (Brush)value;

				if (dic.ContainsValue (brush))
				{
					foreach(KeyValuePair<int, Brush> pair in dic)
					{
						if (brush.Equals (pair.Value))
						{
							retVal = pair.Key;
							break;
						}
					}
				}
			}

			return retVal;
		}

		#endregion

		protected abstract Dictionary<int, Brush> GetWorkingDictionary ();
	}
}
