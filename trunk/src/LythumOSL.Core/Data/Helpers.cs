using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Data
{
	public class Helpers
	{
		/// <summary>
		/// Used for only typical dictionary datatables,
		/// where first column is id, and 2nd column is name
		/// 
		/// This method copies all DataTable and adds 1st column
		/// </summary>
		/// <param name="sourceTable"></param>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DataTable CraftDictionaryTable (
			DataTable sourceTable,
			object id,
			object name)
		{
			DataTable retVal = sourceTable.Copy ();
			DataRow r = retVal.NewRow ();

			r[0] = id;
			r[1] = name;

			retVal.Rows.InsertAt (r, 0);
			retVal.AcceptChanges ();

			return retVal;
		}

		/// <summary>
		/// Used for only typical dictionary datatables,
		/// where first column is id, and 2nd column is name
		/// 
		/// This method copies all DataTable and adds 1st column
		/// </summary>
		/// <param name="sourceTable"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DataTable CraftDictionaryTable (
			DataTable sourceTable,
			object name)
		{
			return CraftDictionaryTable (sourceTable, 0, name);
		}

		/// <summary>
		/// This method is designet for any datatable 
		/// which needs to add first row with any value and name.
		/// It also adds system column, where all items will be set to 0
		/// except newly added item, it will be set to 1
		/// </summary>
		/// <param name="sourceTable"></param>
		/// <param name="names"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static DataTable CraftTableWithSystem (
			DataTable sourceTable,
			string[] names,
			object[] values)
		{
			DataTable retVal = null;

			if (sourceTable != null)
			{
				retVal = sourceTable.Copy ();

				DataColumn col = new DataColumn("system");
				col.DefaultValue = "0";
				retVal.Columns.Add (col);

				if (names != null && values != null)
				{
					if (names.Length <= values.Length)
					{
						DataRow r = retVal.NewRow ();
						r["system"] = "1";

						for (int i = 0; i < names.Length; i++)
						{
							if (values[i] == null)
							{
								r[names[i]] = DBNull.Value;
							}
							else
							{
								r[names[i]] = values[i];
							}
						}

						retVal.Rows.InsertAt (r, 0);
					}
				}


			}

			return retVal;
		}
	}
}
