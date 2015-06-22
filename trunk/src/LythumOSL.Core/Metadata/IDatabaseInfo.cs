using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Enums;

namespace LythumOSL.Core.Metadata
{
	public interface IDatabaseInfo
	{
		DatabaseTypes Type { get; }

		string FieldNamePrefix { get; }
		string FieldNamePostfix { get; }
		string FieldAliasPrefix { get; }
		string FieldAliasPostfix { get; }
		string TablePrefix { get; }
		string TablePostfix { get; }

		string FieldSeparator { get; }

		string TokenSelect { get; }
		string TokenUpdate { get; }
		string TokenInsert { get; }
		string TokenDelete { get; }
		string TokenAs { get; }
		string TokenFrom { get; }
		string TokenWhere { get; }
		string TokenGroup { get; }
		string TokenOrder { get; }
		string TokenIn { get; }

		string ValueNull { get; }
		string FunctionNow { get; }

		string JoinInner { get; }
		string JoinLeft { get; }

		string QueryTerminator { get; }

		string GetTableName (string table);
		string GetTableName (string table, string alias);
		string GetFieldName (string field);
		string GetFieldName (string table, string field);
		string GetFieldName (string table, string field, string alias);

		string GetFormula (string formula, string alias);

	}
}
