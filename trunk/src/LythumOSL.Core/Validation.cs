using System;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core
{
	public class Validation : ILythumBase
	{
		#region Strings
		public static void RequireValidString(string parameter, string parameterName)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				throw new LythumException(string.Format(
					Resources.Errors.StringIsNotValid1,
					parameterName));
			}
		}

		public static void RequireValidLenString (
			string parameter, 
			int length, 
			string parameterName)
		{
			RequireValidString (parameter, parameterName);

			if (parameter.Length == length)
			{
				return;
			}

			throw new Exception (string.Format (
				Resources.Errors.StringWrongLenRequire2,
				parameterName,
				length.ToString ()));
		}

		public static void RequireValidStringNumeric (string parameter, string parameterName)
		{
			RequireValidString (parameter, parameterName);

			if (!LythumOSL.Core.Helpers.IsNumeric (parameter))
			{
				throw new LythumException (string.Format (
					Resources.Errors.StringMustBeNumeric1,
					parameterName));
			}

		}

		#endregion

		#region Object

		public static void RequireValid<T>(T parameter, string parameterName)
		{
			if(parameter == null)
			{
				throw new LythumException(string.Format(
					Resources.Errors.ObjectIsNotValid1,
					parameterName));
			}
		}

		#endregion

		public enum ValidationState
		{
			Ok,
			NullOrEmpty,
			Wrong,
		}

		#region Just validation
		/// <summary>
		/// Validates id:
		/// Check for id is NullOrEmpty
		/// Check for id equals 0 - means wrong id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static ValidationState JustValidateId (string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return ValidationState.NullOrEmpty;
			}
			else if (id.Equals ("0"))
			{
				return ValidationState.Wrong;
			}
			else
			{
				return ValidationState.Ok;
			}
		}

		#endregion

	}
}
