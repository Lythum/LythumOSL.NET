using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

using LythumOSL.Indigo.Metadata;

namespace LythumOSL.Indigo.Validators
{
	public class TableDescValidator : ValidationRule
	{
		ITableDesc _Desc;

		public TableDescValidator (ITableDesc desc)
		{
			LythumOSL.Core.Validation.RequireValid (desc, "desc");

			_Desc = desc;
		}

		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			ValidationResult positiveResult = new ValidationResult (true, null);

			try
			{

				string requiredFields = string.Empty;
				bool hasErrors = false;

				BindingGroup bg = value as BindingGroup;
				DataRowView drv = bg.Items[0] as DataRowView;
				DataRow row = drv.Row;

				foreach (IColumnDesc cd in _Desc.Columns)
				{
					if (cd.Required)
					{
						// making strings
						if (!string.IsNullOrEmpty (requiredFields))
						{
							requiredFields += ", ";
						}

						requiredFields += cd.DisplayName;

						// validating
						if (DBNull.Value.Equals (row[cd.Name]))
						{
							hasErrors = true;
						}
						else if (string.IsNullOrEmpty (row[cd.Name].ToString ()))
						{
							hasErrors = true;
						}
					}
				}

				if (hasErrors)
				{
					System.Diagnostics.Debug.Print ("Validation error!");

					return new ValidationResult (false,
						string.Format (Properties.Resources.VALIDATION_FIELD_CANT_BE_EMPTY,
						requiredFields));

				}

				return positiveResult;

			}
			catch
			{
				return positiveResult;
			}
		}
	}
}
