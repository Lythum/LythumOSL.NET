using System;
using System.Windows.Data;

using LythumOSL.Indigo.Enums;
using LythumOSL.Wpf.Converters;

namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class BooleanColumn : ColumnDesc
		{
			public object FalseValue { get; protected set; }
			public object TrueValue { get; protected set; }

			public BooleanColumn (string name, string displayName, object falseValue, object trueValue)
				: base (name, displayName)
			{
				this.FalseValue = falseValue;
				this.TrueValue = trueValue;

				ColumnType = DataColumnType.CheckBox;

				this.Converter = new BooleanToIntConverter();
			}
		}

	}
}
