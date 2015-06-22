using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Indigo.Metadata;

namespace LythumOSL.Indigo.Classification
{
	public class ClassificatorCredentials : IClassificatorCredentials
	{
		#region IClassificatorCredentials Members

		public string UserId { get; set; }
		public bool CanDelete { get; set; }
		public bool CanModify { get; set; }
		public bool CanCreate { get; set; }
		public bool CanSelect { get; set; }

		#endregion

		public ClassificatorCredentials (string userId)
		{
			Validation.RequireValid (userId, "userId");

			this.UserId = userId;

			CanCreate = true;
			CanModify = true;
			CanDelete = true;
			CanSelect = false;
		}
	}
}
