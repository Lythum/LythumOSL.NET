using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Controls;

namespace LythumOSL.Indigo.Validators
{
	public class EmptyValidator : ValidationRule
	{
		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			// default positive value
			ValidationResult positiveResult = new ValidationResult (true, null);
			ValidationResult negativeResult = new ValidationResult (false, Properties.Resources.VALIDATION_ERROR_EMPTY);

			if (value == null)
			{
				return negativeResult;
			}
			else if (string.IsNullOrEmpty (value.ToString ()))
			{
				return negativeResult;
			}

			return positiveResult;
		}
	}
}
