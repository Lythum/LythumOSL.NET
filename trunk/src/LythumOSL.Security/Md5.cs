using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Security
{
	public class Md5
	{
		// MD5 Hash generavimas
		public static string Encrypt (string text)
		{
			MD5CryptoServiceProvider provider = 
				new MD5CryptoServiceProvider ();

			byte[] data = 
				System.Text.Encoding.ASCII.GetBytes (text);

			data = provider.ComputeHash (data);

			string retVal = string.Empty;

			for (int i = 0;i < data.Length;i++)
			{
				retVal += data[i].ToString ("x2").ToLower ();
			}

			return retVal;
		}

	}
}
