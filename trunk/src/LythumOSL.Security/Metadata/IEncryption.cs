using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Security.Metadata
{
	/// <summary>
	/// Only encryption provider
	/// </summary>
	public interface IEncryption
	{
		string Encrypt (string text);
	}
}
