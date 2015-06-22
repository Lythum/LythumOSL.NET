using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Diagnostics
{
	public class Appendix
	{
		public static void DumpTable (DataTable table)
		{
			string colSeparator = "\t|\t";

			if (table == null)
			{
				Debug.Print ("Table is null!");
			}
			else
			{
				Debug.Print ("Dumping table: [" + table.TableName + "]");

				string output = string.Empty;

				foreach (DataColumn c in table.Columns)
				{
					output += c.ColumnName + colSeparator;
				}

				Debug.Print (output);

				foreach (DataRow r in table.Rows)
				{
					output = string.Empty;
					foreach (DataColumn c in table.Columns)
					{
						object value = r[c.ColumnName];

						if (value == null)
						{
							output += "<null>" + colSeparator;
						}
						else if (value == DBNull.Value)
						{
							output += "<DbNull>" + colSeparator;
						}
						else
						{
							output += value.ToString() + colSeparator;
						}
					}

					Debug.Print (output);
				}
			}
		}
	}
}
