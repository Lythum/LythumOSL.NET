using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	/// <summary>
	/// Interface thats supports messaging
	/// </summary>
	public interface IMessenger : ILythumBase
	{
		void Warning (string msg);
		void Error (string msg);
		bool Question (string msg);
	}
}
