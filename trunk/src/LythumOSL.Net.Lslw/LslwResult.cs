using System;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Core.Data.Xml;
using LythumOSL.Security;
using LythumOSL.Security.Encryptions;

namespace LythumOSL.Net.Lslw
{
	public class LslwResult
	{
		#region Enum
		public enum ResultLevel : int
		{
			None = 0,
			Raw = 1,
			Decrypted = 2,
			Data = 3,
		}

		#endregion

		#region Attributes

		string _RawResult;
		string _Result;
		string _DecryptedResult;
		int _ErrorCode;
		Aes _Aes;

		// result
		LslwRawOperation _Operation;
		ResultLevel _Level;
		LslwResultData _Data;

		#endregion

		#region Properties
		/// <summary>
		/// Shows result level
		/// </summary>
		public ResultLevel Level
		{
			get { return _Level; }
		}

		/// <summary>
		/// 0 = No errors
		/// </summary>
		public int ErrorCode
		{
			get
			{
				if (!string.IsNullOrEmpty (_RawResult))
				{
					string[] nodes = _RawResult.Split (' ');

					if (!int.TryParse (nodes[0], out _ErrorCode))
					{
						_ErrorCode = -1;
					}
				}

				return _ErrorCode;
			}
		}

		public bool HasErrors
		{
			get
			{
				return ErrorCode != 0;
			}
		}

		/// <summary>
		/// Raw result without return status information, not decrypted or deserialized.
		/// </summary>
		public string Result
		{
			get
			{
				return _Result;
			}
		}

		public string RawResult
		{
			get { return _RawResult; }
		}

		public string DecryptedResult
		{
			get
			{
				return _DecryptedResult;
			}
		}

		public LslwResultData Data
		{
			get { return _Data; }
		}

		#endregion

		#region ctor

		public LslwResult ()
		{
			_ErrorCode = -1;
			_Result = string.Empty;
		}

		public LslwResult (LslwRawOperation operation, Aes aes, string result)
			: this()
		{
			Validation.RequireValid (aes, "aes");

			_Operation = operation;
			_Aes = aes;
			_RawResult = result;

			Init ();
		}

		#endregion

		#region Helpers

		void Init ()
		{
			// Level 1 validation - for empty string
			if (string.IsNullOrEmpty (_RawResult))
			{
				// raw result was empty, so result level is none
				_Level = ResultLevel.None;
				return;
			}

			// Level 2 validation
			// string was not empty
			// trying to cut value after \r\n where is begin of real data
			try
			{
				_Result = _RawResult.Substring ((_RawResult.IndexOf ("\r\n") + 2));
			}
			catch
			{
			}
			
			// checking for cutted value
			if (!string.IsNullOrEmpty (_Result))
			{
				// value found then we got raw result
				_Level = ResultLevel.Raw;
			}
			else
			{
				return;
			}

			// if it's an raw operation, like disconnect or just to get RSA public key
			// any other steps are not required
			if (_Operation == LslwRawOperation.RsaGetPublicKey ||
				_Operation == LslwRawOperation.Disconnect)
			{
				return;
			}

			// Level 3 validation
			// trying to decrypt result
			try
			{
				_DecryptedResult = _Aes.Decrypt (Result);
				_Level = ResultLevel.Decrypted;
			}
			catch
			{
				return;
			}

			// If it's an AES data, we need to deserialize it
			if (_Operation == LslwRawOperation.AesData && !string.IsNullOrEmpty(_DecryptedResult))
			{

				// Level 4 validation
				// trying to deserialize XML to LslwResultData
				try
				{
					_Data = Xml.Deserialize<LslwResultData> (_DecryptedResult);
				}
				catch (Exception ex)
				{
                    System.Diagnostics.Debug.WriteLine(ex.Message);
					return;
				}

				// validate data
				if (_Data != null && _Data is LslwResultData)
				{
					if (_Data.DataSet != null)
					{
						_Data.DataSet.AcceptChanges ();
					}

					_Level = ResultLevel.Data;
				}

			}

		}

		#endregion


	}
}
