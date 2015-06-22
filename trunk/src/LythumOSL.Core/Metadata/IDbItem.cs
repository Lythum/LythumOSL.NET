using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IDbItem : ILythumBase
	{
		string Id { get; }
		/// <summary>
		/// true - item from database
		/// false - item not saved to database
		/// </summary>
		bool IsNewItem { get;  }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// String.Empty - If all ok
		/// Error value if not
		/// </returns>
		string Validate ();
	}
}
