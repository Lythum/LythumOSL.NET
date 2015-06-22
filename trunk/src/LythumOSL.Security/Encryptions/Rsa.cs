using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using LythumOSL.Security.Metadata;

namespace LythumOSL.Security.Encryptions
{
	/// <summary>
	/// Asimetrinis RSA algoritmas pasiþymi tuo, kad pilnai komunikacijai reikalingi du raktai:
	/// 1. Vieðas raktas (public key) – skirtas tik informacijai uþkoduoti, su juo po to ðios informacijos atkoduoti nëra ámanoma. Dël to net jei ir vieðas raktas ir pats algoritmas yra vieðai þinomi, tiek informacijos nëra pakankama atkoduoti uþkoduotus duomenis.
	/// 2. Privatus raktas (private key)  - skirtas atkoduoti informacijà, uþkoduotà su vieðu raktu, jis neturi bûti niekam þinomas ar prieinamas.
	/// </summary>
	public sealed class Rsa : IEncryption
	{
		#region Const
		const string DefaultRsaKeySeparator = "-----";

		#endregion

		#region Attributes
		X509Certificate2 _Certificate;
		bool _Initialized;
		/// <summary>
		/// Lythum Safe Legacy Webservice, URL safe encoding
		/// </summary>
		bool _UrlSafe;

		#endregion

		#region Ctor

		public Rsa ()
		{
			_Initialized = false;
			_UrlSafe = true;
		}

		/// <summary>
		/// Create a new RSA encryptor from a certificate.
		/// </summary>
		/// <param name="certificateFile"></param>
		/// <param name="lslwCompatible">
		/// Lythum Safe Legacy Webservice compatible, set it to true if unsure
		/// </param>
		public Rsa(string certificateFile, bool urlSafe)
			: this ()
        {
			_UrlSafe = urlSafe;
			LoadCertificateFromFile (certificateFile);
		}

		#endregion

		#region Methods

		/// <summary>
        /// Create a new RSA encryptor from a certificate file.
        /// </summary>
        /// <param name="certificateLocation">The file to load as a certificate.</param>
        public void LoadCertificateFromFile(string file)
        {
            try
            {
                _Certificate = GetCertificateFromFile(file);
                _Initialized = true;
            }
            catch (Exception ex)
            {
                _Initialized = false;
                throw new CryptographicException(
					Properties.Resources.ENC_ERROR_READ_CERT, ex);
            }

            // You should keep the private key on the server and only have the public key on the client side.
			if (_Certificate.HasPrivateKey)
			{
				throw new CryptographicException (
					Properties.Resources.ENC_ERROR_HAS_PRIVATE_KEY);
			}
        }

        /// <summary>
        /// Create a new RSA encryptor from a certificate string.
        /// </summary>
        /// <param name="certificateText">The base64 encoded value to load as a certificate.</param>
        public void LoadCertificateFromString(string certificateText)
        {
            try
            {
                _Certificate = GetCertificate(certificateText);
                _Initialized = true;
            }
            catch (Exception ex)
            {
                _Initialized = false;
                throw new CryptographicException(
					Properties.Resources.ENC_ERROR_READ_CERT, ex);
            }

            // You should keep the private key on the server and only have the public key on the client side.
			if (_Certificate.HasPrivateKey)
			{
				throw new CryptographicException (
					Properties.Resources.ENC_ERROR_HAS_PRIVATE_KEY);
			}
		}


		/// <summary>
		/// Encrypt a messages using the supplied public certificate.
		/// </summary>
		/// <param name="message">The message to encrypt.</param>
		public byte[] Encrypt (byte[] message)
		{
			if (_Initialized)
			{
				RSACryptoServiceProvider provider = 
					(RSACryptoServiceProvider)_Certificate.PublicKey.Key;
				return provider.Encrypt (message, false);
			}
			else
			{
				throw new Exception (Properties.Resources.ENC_ERROR_RSA_NOT_INITIALIZED);
			}
		}

		/// <summary>
		/// Encrypt a message using the supplied public certificate and returns the ciphertext as a base64 encoded string.
		/// </summary>
		/// <param name="message">The message to encrypt.</param>
		public string Encrypt (string message)
		{
			if (_Initialized)
			{
				byte[] encoded = Encrypt (ASCIIEncoding.ASCII.GetBytes (message));

				if (_UrlSafe)
				{
					// Lythum Safe Legacy Webservice, URL safe encoding
					return Base64.Encode (encoded, _UrlSafe);
				}
				else
				{
					return Base64.Encode (encoded);
				}
			}
			else
			{
				throw new Exception (Properties.Resources.ENC_ERROR_RSA_NOT_INITIALIZED);
			}
		}
		#endregion

		#region Helpers

		/// <summary>
        /// Load a public RSA key from a certificate string.
        /// </summary>
        /// <param name="key">The certificate value.</param>
        /// <exception cref="FormatException"></exception>
        private X509Certificate2 GetCertificate(string key)
        {
            try
            {
				if (key.Contains (DefaultRsaKeySeparator))
                {
                    // Get just the base64 encoded part of the file then trim off the beginning and ending -----KEY----- tags
					key = key.Split (
						new string[] { DefaultRsaKeySeparator },
						StringSplitOptions.RemoveEmptyEntries)[1];
                }

                // Convert the key to a certificate for encryption
                return new X509Certificate2(Convert.FromBase64String(key));
            }
            catch (Exception ex)
            {
                throw new FormatException(
					Properties.Resources.ENC_ERROR_INVALID_KEY_FORMAT, ex);
            }
        }

        /// <summary>
        /// Load a public RSA key from a certificate file.
        /// </summary>
        /// <param name="file">The certificate file.</param>
        /// <returns></returns>
        private X509Certificate2 GetCertificateFromFile(string file)
        {
            return GetCertificate(File.ReadAllText(file));
		}

		#endregion
	}
}
