using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IErrorInfo : ILythumBase
	{
		bool HasError { get; }
		string ErrorText { get; }
		Exception ErrorException { get; }

		/// <summary>
		/// Clear error
		/// </summary>
		void Error();
		/// <summary>
		/// Error initialization with message
		/// </summary>
		/// <param name="message"></param>
		void Error(string message);
		/// <summary>
		/// Error state initialization with exception
		/// </summary>
		/// <param name="ex"></param>
		void Error(Exception ex);
	}
}
