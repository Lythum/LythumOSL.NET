using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Extensions
{
	public static class DataTableExt
	{
		public enum TableState
		{
			Null,
			NoRows,
			HasRows,
		}

		public static TableState GetTableState (this DataTable data)
		{
			if (data == null)
			{
				return TableState.Null;
			}

			if (data.Rows.Count < 1)
			{
				return TableState.NoRows;
			}
			else
			{
				return TableState.HasRows;
			}
		}
	}
}
