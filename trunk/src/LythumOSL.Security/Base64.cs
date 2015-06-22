using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Security
{
	public class Base64
	{
		#region Encode
		/// <summary>
		/// Encode string as BASE64 UTF8 encoding string
		/// </summary>
		/// <param name="str">Data</param>
		/// <returns>Encoded BASE64 string</returns>
		public static string Encode (string str)
		{
			return Encode (str, Encoding.UTF8, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string Encode (string str, Encoding encoding)
		{
			return Encode (str, encoding, false);
		}

		/// <summary>
		/// Encode string with specified encoding and url safe option
		/// </summary>
		/// <param name="str">Data</param>
		/// <param name="encoding">Destination encoding</param>
		/// <param name="urlSafe">True if to convert to url safe BASE64 string. WARNING! Url safe mode is incompatible with regular BASE64 encoded string format.</param>
		/// <returns>Encoded BASE64 string</returns>
		public static string Encode (string str, Encoding encoding, bool urlSafe)
		{
			byte[] encbuff = encoding.GetBytes (str);
			return Encode (encbuff, urlSafe);
		}

		/// <summary>
		/// Encoding binary bytes to BASE64
		/// </summary>
		/// <param name="data">Binary data</param>
		/// <returns></returns>
		public static string Encode (byte[] data)
		{
			return Encode (data, false);
		}

		/// <summary>
		/// Encoding binary bytes to BASE64 with url safe option
		/// </summary>
		/// <param name="data">Binary data</param>
		/// <param name="urlSafe">True if to convert to url safe BASE64 string. WARNING! Url safe mode is incompatible with regular BASE64 encoded string format.</param>
		/// <returns></returns>
		public static string Encode (byte[] data, bool urlSafe)
		{
			if (urlSafe)
			{
				return Convert.ToBase64String (data).Replace ("+", "-").Replace ("/", "_");
			}
			else
			{
				return Convert.ToBase64String (data);
			}
		}

		#endregion

		#region Decode

		/// <summary>
		/// Correct BASE64 decoding with specified string encoding
		/// </summary>
		/// <param name="str">Encoded BASE64 string.</param>
		/// <param name="encoding">Encoding</param>
		/// <param name="urlSafe">True if to convert to url safe BASE64 string. WARNING! Url safe mode is incompatible with regular BASE64 encoded string format.</param>
		/// <returns>UTF16 value</returns>
		public static string Decode (string str, Encoding encoding, bool urlSafe)
		{
			byte[] decbuff = DecodeToBytes(str, urlSafe);
			return encoding.GetString (decbuff);
		}

		/// <summary>
		/// Simple BASE64 decoding, decodes BASE64 encoded UTF8 string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Decode (string str)
		{
			return Decode (str, Encoding.UTF8, false);
		}

		/// <summary>
		/// Decode to bytes
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static byte[] DecodeToBytes (string str, bool urlSafe)
		{
			if (urlSafe)
			{
				return Convert.FromBase64String (str.Replace ("-", "+").Replace ("_", "/"));
			}
			else
			{
				return Convert.FromBase64String (str);
			}
		}

		#endregion
	}
}
