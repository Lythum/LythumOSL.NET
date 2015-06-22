using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	public class QueriesHolder : IQueriesHolder
	{
		#region IQueriesHolder Members

		public string QuerySelect { get; protected set; }
		public string QueryInsert { get; protected set; }
		public string QueryUpdate { get; protected set; }
		public string QueryDelete { get; protected set; }

		/// <summary>
		/// Constructor first initializes:
		/// 1. MetadataInit
		/// 2. QueriesInit
		/// </summary>
		public QueriesHolder ()
		{
			MetadataInit ();

			QueriesInit ();
		}

		public virtual void MetadataInit ()
		{
		}

		public virtual void QueriesInit ()
		{
			QuerySelect = string.Empty;
			QueryInsert = string.Empty;
			QueryUpdate = string.Empty;
			QueryDelete = string.Empty;
		}



		#endregion
	}
}
