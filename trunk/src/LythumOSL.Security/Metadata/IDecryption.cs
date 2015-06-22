using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Security.Metadata
{
	/// <summary>
	/// Encryption and decryption provider
	/// </summary>
	public interface IDecryption : IEncryption
	{
		string Decrypt (string text);
	}
}
