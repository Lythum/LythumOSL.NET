using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Data;
using LythumOSL.Core.Metadata;

namespace LythumOSL.Indigo.Metadata
{
	/// <summary>
	/// Derived from named item, which has Name property
	/// </summary>

	public interface ITableDesc : IMetadataDbItem
	{
		/// <summary>
		/// Column Name, Column Data
		/// </summary>
		NamedItemsCollection<IColumnDesc> Columns { get; }
		bool Selectable { get; set; }

		IColumnDesc PrimaryKeyColumn { get; }
		IColumnDesc NameColumn { get; }

		void Prepare ();

		// Queries
		string QuerySelect (IDatabaseInfo i);
		string QueryUpdate (IDatabaseInfo i, DataRow r);
		string QueryInsert (IDatabaseInfo i, DataRow r);
		string QueryDelete (IDatabaseInfo i, DataRow r, string userId);

		void QueryPostProcess (DataTable t);
	}
}
