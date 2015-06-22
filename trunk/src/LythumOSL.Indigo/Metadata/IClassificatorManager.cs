using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Indigo.Metadata
{
	public interface IClassificatorManager
	{
		IDataAccess DataAccess { get; }
		ITableDesc TableDescription { get; }
		IClassificatorCredentials Credentials { get; }

		DataTable Search ();

		void InitUI (DataGrid grid);
		void PopulateResult (DataGrid grid, DataTable table);

		bool Save ();
		void Delete (DataRow[] rows);
	}
}
