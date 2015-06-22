using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core;

namespace LythumOSL.Core.Data
{
	public class SqlHelpers
	{
		Sql _Db;

		public SqlHelpers (Sql sql)
		{
			Validation.RequireValid (sql, "sql");

			_Db = sql;
		}

		public long GetCount (string table, string field, string whereCause)
		{
			string realField =
				(string.IsNullOrEmpty (field) ? "*" : field);

			string sql = "Select count(" + realField + ") from  " + table;

			if (!string.IsNullOrEmpty (whereCause))
			{
				sql += " Where " + whereCause;
			}

			string count = _Db.QueryScalar (sql);
			long retVal = 0;

			long.TryParse (count, out retVal);

			return retVal;
		}
	}
}
