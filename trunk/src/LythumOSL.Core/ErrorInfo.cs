using System;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core
{
	public class ErrorInfo : IErrorInfo
	{
		#region Attributes

		Exception _ErrorException = null;

		#endregion

		#region Properties

		#endregion

		#region Construction

		public ErrorInfo()
		{
			Error();
		}

		#endregion

		#region IErrorInfo Members

		public bool HasError
		{
			get { return _ErrorException!=null; }
		}

		public string ErrorText
		{
			get
			{
				if (HasError)
				{
					return _ErrorException.Message;
				}
				else
				{
					return string.Empty;
				}
			}
		}

		public Exception ErrorException
		{
			get
			{
				return _ErrorException;
			}
		}

		/// <summary>
		/// Cleanup error state
		/// </summary>
		public void Error()
		{
			if (HasError)
			{
				_ErrorException = null;
			}
		}

		public void Error(string message)
		{
			_ErrorException = new Exception(message);
		}

		public void Error(Exception ex)
		{
			_ErrorException = ex;
		}

		#endregion
	}
}
