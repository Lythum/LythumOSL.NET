using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface ILoginInfo : ILythumBase
	{
		string Username { get; set; }
		string Password { get; set; }
		/// <summary>
		/// Success shows then user entered username and password
		/// and pressed ok
		/// </summary>
		bool Success { get; set; }
		ILythumBase ConnectionInfo { get; set; }
	}
}
