using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	/// <summary>
	/// Interface thats supports messaging and progress indication
	/// </summary>
	public interface IProgressMessenger : IMessenger, IProgressIterator
	{
	}
}
