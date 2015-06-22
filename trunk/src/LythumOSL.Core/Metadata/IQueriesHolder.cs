using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IQueriesHolder : ILythumBase
	{
		string QuerySelect { get; }
		string QueryInsert { get; }
		string QueryUpdate { get; }
		string QueryDelete { get; }

		void MetadataInit ();
		void QueriesInit ();

	}
}
