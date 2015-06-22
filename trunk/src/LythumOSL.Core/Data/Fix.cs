using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Data
{
	public class Fix
	{
		/// <summary>
		/// Function fixes all \n or \r tags 
		/// and will make from group of these tags
		/// one windows new line \r\n
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string FixNewLines (string input)
		{
			string retVal = string.Empty;
			bool newLineFound = false;

			foreach (char c in input)
			{
				if (c.Equals ('\r') ||
					c.Equals ('\n'))
				{
					newLineFound = true;
				}
				else
				{
					if (newLineFound)
					{
						newLineFound = false;

						retVal += "\r\n";
					}

					retVal += c;
				}
			}

			return retVal;
		}

		/// <summary>
		/// DEPREACED! Use DateTime.ToFrom(), from Lythum.Core.Extensions 
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string FixDateFrom (DateTime? date)
		{
			if (date.HasValue)
			{
				return date.Value.ToString ("yyyy-MM-dd") + " 00:00:00";
			}

			return string.Empty;
		}

		public static string FixDateFrom (string date)
		{
			return FixDateFrom (DateTime.Parse (date));
		}


		/// <summary>
		/// DEPREACED! Use DateTime.ToTo(), from Lythum.Core.Extensions 
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string FixDateTo (DateTime? date)
		{
			if (date.HasValue)
			{
				return date.Value.ToString ("yyyy-MM-dd") + " 23:59:59";
			}

			return null;
		}

		public static string FixDateTo (string date)
		{
			return FixDateTo (DateTime.Parse (date));
		}

		/// <summary>
		/// DEPREACED! Use String.ToSqlField(), from Lythum.Core.Extensions 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string EmptyToNull (string text)
		{
			if (string.IsNullOrEmpty (text.Trim ()))
			{
				return "null";
			}
			else
			{
				return "'" + Fix.FixString (text) + "'";
			}
		}

		/// <summary>
		/// DEPREACED! Use Boolean.ToInt(), from Lythum.Core.Extensions
		/// Converting bool to int
		/// </summary>
		/// <param name="value"></param>
		/// <returns>true = 1, false = 0</returns>
		public static string FixBool (bool value)
		{
			return (value ? "1" : "0");
		}

		/// <summary>
		/// DEPREACED! Use Object.ToValueIfEmpty(), from Lythum.Core.Extensions
		/// 
		/// Check object is null if null return isNullValue parameter as value
		/// if not null then value.ToString
		/// </summary>
		/// <param name="value"></param>
		/// <param name="isNullValue"></param>
		/// <returns></returns>
		public static string FixNull (object value, string isNullValue)
		{
			return (value == null ? isNullValue : value.ToString ());
		}

		/// <summary>
		/// DEPREACED! Use String.Fix(), from Lythum.Core.Extensions
		/// 
		/// Anti injection stuff
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FixString (string text)
		{
			if (string.IsNullOrEmpty (text))
			{
				return text;
			}
			else
			{
				return text.Replace ("'", "''")
					.TrimEnd (' ');
			}
		}

		public static string[] FixArray (string[] array)
		{
			string[] retVal = null;

			if (array != null)
			{
				if (array.Length > 0)
				{
					retVal = new string[array.Length];

					for (int i = 0; i < array.Length; i++)
					{
						retVal[i] = Fix.FixString (array[i]);
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Fix money data from database to 2 digits after comma and rouding
		/// </summary>
		/// <param name="money"></param>
		/// <returns></returns>
		public static string FixDbMoney (object money)
		{
			return Math.Round (Convert.ToDecimal (money), 2, MidpointRounding.AwayFromZero).ToString ();
		}

		/// <summary>
		/// Fixes money to database, e.g.:
		/// 
		/// decimal 1,40 => 1.40
		/// 
		/// </summary>
		/// <param name="money"></param>
		/// <returns></returns>
		public static string FixMoneyToDb (decimal money)
		{
			return money.ToString ().Replace (",", ".");
		}

	}
}
