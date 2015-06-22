using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Indigo.Metadata
{
	public interface IClassificatorCredentials
	{
		/// <summary>
		/// Working user's id for delete operation
		/// </summary>
		string UserId { get; set; }
		/// <summary>
		/// User can delete dictionary information
		/// </summary>
		bool CanDelete { get; set; }
		/// <summary>
		/// User can modify dictionary information
		/// </summary>
		bool CanModify { get; set; }
		/// <summary>
		/// User can create dictionary information
		/// </summary>
		bool CanCreate { get; set; }
		/// <summary>
		/// User can select dictionary information
		/// </summary>
		bool CanSelect { get; set; }
	}
}
