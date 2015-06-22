using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.Core.Net.Http;
using LythumOSL.Security;
using LythumOSL.Security.Encryptions;

namespace LythumOSL.Net.Lslw
{
	/// <summary>
	/// Low, raw level LSLW webservice client implementation
	/// </summary>
	public class LslwRawConnection : IMessenger
	{
		#region Const
		const string LslwOperationName = "op";

		#endregion

		#region Attributes
		bool _Connected;
		Guid _SessionId;
		LslwSettings _Settings;
		Rsa _Rsa;
		Aes _Aes;
		HttpAccess _Access;
		// UI stuff
		IMessenger _Messenger;

		#endregion

		#region Properties

		protected Rsa Rsa
		{
			get { return _Rsa; }
		}

		protected Aes Aes
		{
			get { return _Aes; }
		}

		/// <summary>
		/// Shows connection state
		/// </summary>
		public bool Connected
		{
			get { return _Connected; }
		}

		static Dictionary<LslwRawOperation, string> _sLslwCommands = null;
		public static Dictionary<LslwRawOperation, string> sLslwCommands
		{
			get
			{
				if (_sLslwCommands == null)
				{
					_sLslwCommands = new Dictionary<LslwRawOperation, string> ();
					_sLslwCommands.Add (LslwRawOperation.RsaGetPublicKey, LslwRawCommands.RsaGetPublicKey);
					_sLslwCommands.Add (LslwRawOperation.AesReceiveKey, LslwRawCommands.AesReceiveKey);
					_sLslwCommands.Add (LslwRawOperation.AesData, LslwRawCommands.AesData);
					_sLslwCommands.Add (LslwRawOperation.Disconnect, LslwRawCommands.Disconnect);
				}

				return _sLslwCommands;

			}
		}

		/// <summary>
		/// Overridable server encoding. Used to process data from and to server.
		/// </summary>
		protected virtual Encoding ServerEncoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		#endregion

		#region Ctor

		protected LslwRawConnection ()
		{
			_Connected = false;
			_SessionId = Guid.NewGuid ();
			_Rsa = new Rsa ();
			_Aes = new Aes (ServerEncoding);
			_Access = new HttpAccess ();
			_Messenger = null;
		}

		public LslwRawConnection (LslwSettings settings)
			: this()
		{
			Validation.RequireValid (settings, "settings");

			_Settings = settings;
		}

		public LslwRawConnection (LslwSettings settings, IMessenger messenger)
			: this (settings)
		{
			_Messenger = messenger;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Low level request method
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="postData"></param>
		/// <returns></returns>
		protected LslwResult RawRequest (LslwRawOperation operation, HttpAttributes postData)
		{
			string result = _Access.Request (
				_Settings.WebserviceUrl + "?" + LslwOperationName + "=" + sLslwCommands[operation],
				postData);

			if (postData != null)
			{
				Debug.Print (postData["data"]);
			}

			return new LslwResult (operation, _Aes, result);
		}

		/// <summary>
		/// This method must be called before any encrypting and etc...
		/// Eslewhere data will be encrypted with random keys which is just for initialization of objects
		/// And later replaced with good keys
		/// </summary>
		/// <returns></returns>
		public bool Connect ()
		{
			// checking if already connected
			if (_Connected)
			{
				return _Connected;
			}
			string errorStage = string.Empty;

			//try
			//{
				errorStage = "requesting RSA public key";	// RSA
				LslwResult result = RawRequest( LslwRawOperation.RsaGetPublicKey, null);
				if (result.HasErrors)
				{
					return false;
				}

				errorStage = "loading RSA public key"; // RSA
				_Rsa.LoadCertificateFromString (result.Result);

				errorStage = "generating AES keys"; // AES
				//DumpKeys ("Keys before generate");
				_Aes.GenerateRandomKeys ();
				//DumpKeys ("Keys after generate");

				HttpAttributes postData = PreparePostData ();
				postData["key"] = Base64.Encode( _Rsa.Encrypt (_Aes.Key), true);
				postData["iv"] = Base64.Encode (_Rsa.Encrypt (_Aes.IV), true);

				errorStage = "sending AES keys"; // AES
				result = RawRequest (LslwRawOperation.AesReceiveKey, postData);

				errorStage = "parsing AES result"; 
				if (!result.HasErrors)
				{
					string[] nodes = result.DecryptedResult.Split (' ');
					_Connected = nodes[0].Equals("000");
				}
			//}
			//catch (Exception ex)
			//{
			//    Error ("Error " + errorStage + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);
			//}

			return _Connected;
		}

		public void Disconnect ()
		{
			if (Connected)
			{
				LslwResult result = RawRequest (LslwRawOperation.Disconnect, null);
				Debug.Assert (!result.HasErrors);
			}
		}

		#endregion

		#region Helpers
		HttpAttributes PreparePostData ()
		{
			HttpAttributes attributes = new HttpAttributes ();
			attributes["guid"] = _SessionId.ToString ();

			return attributes;
		}

		void DumpKeys (string text)
		{
			Debug.Print (text + ": Key=" + _Aes.KeyString + ", IV=" + _Aes.IVString);
		}

		#endregion

		#region IMessenger Members

		public void Warning (string msg)
		{
			if (_Messenger != null)
			{
				_Messenger.Warning (msg);
			}
		}

		public void Error (string msg)
		{
			if (_Messenger != null)
			{
				_Messenger.Error (msg);
			}
		}

		public bool Question (string msg)
		{
			if (_Messenger != null)
			{
				return _Messenger.Question (msg);
			}

			return false;
		}

		#endregion
	}
}
