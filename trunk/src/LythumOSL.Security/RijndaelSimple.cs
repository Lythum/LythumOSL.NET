///////////////////////////////////////////////////////////////////////////////
// SAMPLE: Symmetric key encryption and decryption using Rijndael algorithm.
// 
// To run this sample, create a new Visual C# project using the Console
// Application template and replace the contents of the Class1.cs file with
// the code below.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// 
// Copyright (C) 2002 Obviex(TM). All rights reserved.
// 
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using LythumOSL.Core;
using LythumOSL.Security.Metadata;

namespace LythumOSL.Security
{

	/// <summary>
	/// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and 
	/// decrypt data. As long as encryption and decryption routines use the same
	/// parameters to generate the keys, the keys are guaranteed to be the same.
	/// The class uses static functions with duplicate code to make it easier to
	/// demonstrate encryption and decryption logic. In a real-life application, 
	/// this may not be the most efficient way of handling encryption, so - as
	/// soon as you feel comfortable with it - you may want to redesign this class.
	/// </summary>
	public class RijndaelSimple : IDecryption
	{
		#region Enums
		public enum SupportedKeySize : int
		{
			Key128 = 128,
			Key192 = 192,
			Key256 = 256
		}

		public enum HashAlgorythms
		{
			SHA1,
			MD5,
		}

		#endregion

		string _PassPhrase = "Pas5pr@se";        // can be any string
		string _SaltValue = "s@1tValue";        // can be any string
		string _HashAlgorythm = "SHA1";             // can be "MD5"
		int _PasswordIterations = 2;                  // can be any number
		string _InitVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
		int _KeySize = 256;                // can be 192 or 128


		/// <summary>
		/// Inititalize constructor accurate
		/// </summary>
		/// <param name="passPhrase">can be any string</param>
		/// <param name="saltValue">can be any string</param>
		/// <param name="hashAlgorythm"></param>
		/// <param name="passwordIterations">can be any number</param>
		/// <param name="initVector">must be 16 bytes</param>
		/// <param name="keySize"></param>
		public RijndaelSimple(
			string passPhrase,
			string saltValue,
			HashAlgorythms hashAlgorythm,
			int passwordIterations,
			string initVector,
			SupportedKeySize keySize)
		{
			Validation.RequireValidString (passPhrase, "passPhrase");
			Validation.RequireValidString (saltValue, "saltValue");
			Validation.RequireValidLenString (initVector, 16, "initVector");

			_PassPhrase = passPhrase;
			_SaltValue = saltValue;
			_HashAlgorythm = hashAlgorythm.ToString ();
			_PasswordIterations = passwordIterations;
			_InitVector = initVector;
			_KeySize = (int)keySize;
			
		}


		#region IDecryption Members

		public string Decrypt (string text)
		{
			return StaticDecrypt (
				text,
				_PassPhrase,
				_SaltValue,
				_HashAlgorythm,
				_PasswordIterations,
				_InitVector,
				_KeySize);
		}

		#endregion

		#region IEncryption Members

		public string Encrypt (string text)
		{
			return StaticEncrypt(
				text,
				_PassPhrase,
				_SaltValue,
				_HashAlgorythm,
				_PasswordIterations,
				_InitVector,
				_KeySize);
		}

		#endregion


		#region Static methods
		/// <summary>
		/// Encrypts specified plaintext using Rijndael symmetric key algorithm
		/// and returns a base64-encoded result.
		/// </summary>
		/// <param name="plainText">
		/// Plaintext value to be encrypted.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be 
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Encrypted value formatted as a base64-encoded string.
		/// </returns>
		public static string StaticEncrypt (string plainText,
									 string passPhrase,
									 string saltValue,
									 string hashAlgorithm,
									 int passwordIterations,
									 string initVector,
									 int keySize)
		{
			// Convert strings into byte arrays.
			// Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes (initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes (saltValue);

			// Convert our plaintext into a byte array.
			// Let us assume that plaintext contains UTF8-encoded characters.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes (plainText);

			// First, we must create a password, from which the key will be derived.
			// This password will be generated from the specified passphrase and 
			// salt value. The password will be created using the specified hash 
			// algorithm. Password creation can be done in several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes (
															passPhrase,
															saltValueBytes,
															hashAlgorithm,
															passwordIterations);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes (keySize / 8);

			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged ();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Generate encryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor (
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream ();

			// Define cryptographic stream (always use Write mode for encryption).
			CryptoStream cryptoStream = new CryptoStream (memoryStream,
														 encryptor,
														 CryptoStreamMode.Write);
			// Start encrypting.
			cryptoStream.Write (plainTextBytes, 0, plainTextBytes.Length);

			// Finish encrypting.
			cryptoStream.FlushFinalBlock ();

			// Convert our encrypted data from a memory stream into a byte array.
			byte[] cipherTextBytes = memoryStream.ToArray ();

			// Close both streams.
			memoryStream.Close ();
			cryptoStream.Close ();

			// Convert encrypted data into a base64-encoded string.
			string cipherText = Convert.ToBase64String (cipherTextBytes);

			// Return encrypted string.
			return cipherText;
		}

		/// <summary>
		/// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
		/// </summary>
		/// <param name="cipherText">
		/// Base64-formatted ciphertext value.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Decrypted string value.
		/// </returns>
		/// <remarks>
		/// Most of the logic in this function is similar to the Encrypt
		/// logic. In order for decryption to work, all parameters of this function
		/// - except cipherText value - must match the corresponding parameters of
		/// the Encrypt function which was called to generate the
		/// ciphertext.
		/// </remarks>
		public static string StaticDecrypt (string cipherText,
									 string passPhrase,
									 string saltValue,
									 string hashAlgorithm,
									 int passwordIterations,
									 string initVector,
									 int keySize)
		{
			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes (initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes (saltValue);

			// Convert our ciphertext into a byte array.
			byte[] cipherTextBytes = Convert.FromBase64String (cipherText);

			// First, we must create a password, from which the key will be 
			// derived. This password will be generated from the specified 
			// passphrase and salt value. The password will be created using
			// the specified hash algorithm. Password creation can be done in
			// several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes (
															passPhrase,
															saltValueBytes,
															hashAlgorithm,
															passwordIterations);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes (keySize / 8);

			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged ();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Generate decryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor (
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream (cipherTextBytes);

			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream cryptoStream = new CryptoStream (memoryStream,
														  decryptor,
														  CryptoStreamMode.Read);

			// Since at this point we don't know what the size of decrypted data
			// will be, allocate the buffer long enough to hold ciphertext;
			// plaintext is never longer than ciphertext.
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			// Start decrypting.
			int decryptedByteCount = cryptoStream.Read (plainTextBytes,
													   0,
													   plainTextBytes.Length);

			// Close both streams.
			memoryStream.Close ();
			cryptoStream.Close ();

			// Convert decrypted data into a string. 
			// Let us assume that the original plaintext string was UTF8-encoded.
			string plainText = Encoding.UTF8.GetString (plainTextBytes,
													   0,
													   decryptedByteCount);

			// Return decrypted string.   
			return plainText;
		}

		#endregion

	}

}