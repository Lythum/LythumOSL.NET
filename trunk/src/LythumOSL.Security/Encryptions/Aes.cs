using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using LythumOSL.Security.Metadata;

namespace LythumOSL.Security.Encryptions
{
	/// <summary>
	/// Simetrinis AES algoritmas, skirtingai nei asimetrinis (pvz. RSA), neturi dviejø raktø, o tik vienà raktà. 
	/// Su ðiuo raktu informacija yra ir atkoduojama ir uþkoduojama. 
	/// Dël to jis paprastai perduodamas uþkoduotas. 
	/// Faktas gal eitø viskam naudoti RSA algoritmà, bet jis yra gan lëtas ir stipriai lëtintø sistemos darbà, 
	/// dël to RSA paprastai naudojamas tik saugiai perduoti AES raktà.
	/// </summary>
	public sealed class Aes : IDecryption
	{
		#region Attributes
		byte[] _Key;
		/// <summary>
		/// AES initialization vector
		/// </summary>
		byte[] _IV; 
		bool _UrlSafe;
		Encoding _OponentEncoding;

		#endregion

		#region Properties
		/// <summary>
		/// Encryption key.
		/// </summary>
		public byte[] Key
		{
			get { return _Key; }
		}

		/// <summary>
		/// Initialization vector
		/// </summary>
		public byte[] IV
		{
			get { return _IV; }
		}

		/// <summary>
		/// AES key as a base64 encoded string.
		/// </summary>
		public string KeyString
		{
			get
			{
				return Base64.Encode (_Key, _UrlSafe);
			}
		}

		/// <summary>
		/// AES Initialization vector as a base64 encoded string.
		/// </summary>
		public string IVString
		{
			get
			{
				return Base64.Encode (_IV, _UrlSafe);
			}
		}


		#endregion

		#region Ctor

		public Aes ()
		{
			_OponentEncoding = Encoding.UTF8;
			_Key = new byte[256 / 8];
			_IV = new byte[128 / 8];
			_UrlSafe = true;

			GenerateRandomKeys ();
		}

		public Aes (Encoding oponentEncoding)
			: this ()
		{
			_OponentEncoding = oponentEncoding;
		}

		/// <summary>
		/// Constructor with key and initialization vector initialization
		/// </summary>
		/// <param name="key">AES Key</param>
		/// <param name="iv">AES Initialization vector</param>
		/// <param name="urlSafe">URL safe base64 encoding, required for LSLW system, set it to true if unsure</param>
		public Aes (Encoding oponentEncoding, string key, string iv, bool urlSafe)
		{
			_UrlSafe = urlSafe;
			_OponentEncoding = oponentEncoding;


			if (urlSafe)
			{
				_Key = Base64.DecodeToBytes (key, _UrlSafe);
				_IV = Base64.DecodeToBytes (iv, _UrlSafe);
			}
			else
			{
				_Key = Base64.DecodeToBytes (key, _UrlSafe);
				_IV = Base64.DecodeToBytes (iv, _UrlSafe);
			}


			if (_Key.Length * 8 != 256)
				throw new Exception (Properties.Resources.ENC_ERROR_KEY256_LENGTH);
			if (_IV.Length * 8 != 128)
				throw new Exception (Properties.Resources.ENC_ERROR_IV128_LENGTH);
		}

		#endregion

		#region Methods

		/// <summary>
        /// Generate the cryptographically secure random 256 bit Key and 128 bit IV for the AES algorithm.
        /// </summary>
        public void GenerateRandomKeys()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            random.GetBytes(_Key);
            random.GetBytes(_IV);
        }

        /// <summary>
        /// Encrypt a message and get the encrypted message in a URL safe form of base64.
        /// </summary>
        /// <param name="plainText">The message to encrypt.</param>
        public string Encrypt(string plainText)
        {
			return Base64.Encode (EncryptData (plainText), _UrlSafe);
		}

		/// <summary>
		/// Decrypt a message that is in a url safe base64 encoded string.
		/// </summary>
		/// <param name="cipherText">The string to decrypt.</param>
		public string Decrypt (string cipherText)
		{
			return DecryptData (Base64.DecodeToBytes (cipherText, _UrlSafe));
		}

		#endregion

		#region Helpers
		/// <summary>
		/// Encrypt a message using AES.
		/// </summary>
		/// <param name="plainText">The message to encrypt.</param>
		private byte[] EncryptData (string plainText)
		{
			try
			{
				RijndaelManaged aes = new RijndaelManaged ();
				aes.Padding = PaddingMode.PKCS7;
				aes.Mode = CipherMode.CBC;
				aes.KeySize = 256;
				aes.Key = Key;
				aes.IV = IV;

				ICryptoTransform encryptor = aes.CreateEncryptor (aes.Key, aes.IV);

				MemoryStream msEncrypt = new MemoryStream ();
				CryptoStream csEncrypt = new CryptoStream (msEncrypt, encryptor, CryptoStreamMode.Write);
				StreamWriter swEncrypt = new StreamWriter (csEncrypt, _OponentEncoding);

				swEncrypt.Write (plainText);

				swEncrypt.Close ();
				csEncrypt.Close ();
				aes.Clear ();

				return msEncrypt.ToArray ();
			}
			catch (Exception ex)
			{
				throw new CryptographicException (Properties.Resources.ENC_ERROR_ENCRYPT, ex);
			}
		}

		/// <summary>
		/// Decrypt a message that was AES encrypted.
		/// </summary>
		/// <param name="cipherText">The string to decrypt.</param>
		private string DecryptData (byte[] cipherText)
		{
			try
			{
				RijndaelManaged aes = new RijndaelManaged ();
				aes.Padding = PaddingMode.PKCS7;
				aes.Mode = CipherMode.CBC;
				aes.KeySize = 256;
				aes.Key = Key;
				aes.IV = IV;

				ICryptoTransform decryptor = aes.CreateDecryptor (aes.Key, aes.IV);

				MemoryStream msDecrypt = new MemoryStream (cipherText);
				CryptoStream csDecrypt = new CryptoStream (msDecrypt, decryptor, CryptoStreamMode.Read);
				StreamReader srDecrypt = new StreamReader (csDecrypt, _OponentEncoding, false);

				string plaintext = srDecrypt.ReadToEnd ();

				//string normal = Encoding.UTF8.GetString (Encoding.Default.GetBytes (plaintext));

				srDecrypt.Close ();
				csDecrypt.Close ();
				msDecrypt.Close ();
				aes.Clear ();

				return plaintext;
			}
			catch (Exception ex)
			{
				throw new CryptographicException (Properties.Resources.ENC_ERROR_DECRYPT, ex);
			}
		}
		#endregion
	}
}
