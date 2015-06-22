using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Metadata;

namespace Lythum.Core.Data.DataTypes
{
	public class LoginInfo : ILoginInfo
	{
		#region ILoginInfo Members

		public string Username { get; set; }
		public string Password { get; set; }
		public bool Success { get; set; }

		public ILythumBase ConnectionInfo { get; set; }

		#endregion

		public LoginInfo ()
		{
			Username = string.Empty;
			Password = string.Empty;
			Success = false;
			ConnectionInfo = null; 
		}
	}
}
