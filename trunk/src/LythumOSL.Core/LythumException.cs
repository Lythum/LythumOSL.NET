using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core
{
	public class LythumException : Exception
	{
		public LythumException (string message)
			: base (message)
		{
		}

		public LythumException (string message, Exception innerException)
			: base (message, innerException)
		{

		}

		public LythumException (Exception innerException)
			: base (innerException.Message, innerException)
		{
		}
	}
}
