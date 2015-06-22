using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Enums;
using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data.Info
{
	public class DatabasesInfo : IDatabaseInfo
	{
		#region IDatabaseInfo Members

		public DatabaseTypes Type { get; set; }

		public string FieldNamePrefix { get; protected set; }
		public string FieldNamePostfix { get; protected set; }
		public string FieldAliasPrefix { get; protected set; }
		public string FieldAliasPostfix { get; protected set; }
		public string TablePrefix { get; protected set; }
		public string TablePostfix { get; protected set; }

		public string FieldSeparator { get; protected set; }

		public string TokenSelect { get; protected set; }
		public string TokenUpdate { get; protected set; }
		public string TokenInsert { get; protected set; }
		public string TokenDelete { get; protected set; }
		public string TokenAs { get; protected set; }
		public string TokenFrom { get; set; }
		public string TokenWhere { get; protected set; }
		public string TokenGroup { get; protected set; }
		public string TokenOrder { get; protected set; }
		public string TokenIn { get; protected set; }

		public string ValueNull { get; protected set; }
		public string FunctionNow { get; protected set;  }

		public string FormatSelect { get; protected set; }

		public string JoinInner { get; protected set; }
		public string JoinLeft { get; protected set; }

		public string QueryTerminator { get; protected set;  }


		#endregion

		public DatabasesInfo (DatabaseTypes type)
		{
			this.Type = type;

			Init ();
		}

		void Init ()
		{
			InitTokens ();

			switch (Type)
			{
				case DatabaseTypes.MySql:
					TablePrefix = FieldNamePrefix = FieldAliasPrefix = "`";
					TablePostfix = FieldNamePostfix = FieldAliasPostfix = "`";

					FunctionNow = "NOW()";
					break;

				case DatabaseTypes.MsSql:
					TablePrefix = FieldNamePrefix = "[";
					TablePostfix = FieldNamePostfix = "]";

					FieldAliasPostfix = FieldAliasPrefix = "\"";

					FunctionNow = "GETDATE()";
					break;

				default:
					break;
			}
		}

		void InitTokens ()
		{
			TokenSelect = "SELECT";
			TokenUpdate = "UPDATE";
			TokenInsert = "INSERT INTO";
			TokenDelete = "DELETE FROM";
			TokenAs = "AS";
			TokenFrom = "FROM";
			TokenWhere = "WHERE";
			TokenOrder = "ORDER BY";
			TokenGroup = "GROUP BY";
			TokenIn = "IN";

			ValueNull = "NULL";

			FieldSeparator = ", ";
			QueryTerminator = ";";

			JoinInner = "INNER JOIN {0} ON {1}";
			JoinLeft = "LEFT JOIN {0} ON {1}";
		}

		public string GetTableName (string table)
		{
			if (!string.IsNullOrEmpty (table))
			{
				return TablePrefix + table + TablePostfix;
			}
			else
			{
				return string.Empty;
			}
		}

		public string GetTableName (string table, string alias)
		{
			return GetTableName (table) + " " + TokenAs + " " +
				TablePrefix + alias + TablePostfix;
		}


		public string GetFieldName (string fieldName)
		{
			return FieldNamePrefix + fieldName + FieldNamePostfix;
		}

		public string GetFieldName (string table, string fieldName)
		{
			string tableName = GetTableName (table);

			return (string.IsNullOrEmpty (tableName) ? "" : tableName + ".") +
				GetFieldName (fieldName);
		}

		public string GetFieldName (string table, string field, string alias)
		{
			return GetFieldName (table, field) + " " + TokenAs + " " +
				FieldAliasPrefix + alias + FieldAliasPostfix;
		}

		public string GetFormula (string formula, string alias)
		{
			return formula + " " + TokenAs + " " +FieldAliasPrefix + alias + FieldAliasPostfix;
		}
	}
}
