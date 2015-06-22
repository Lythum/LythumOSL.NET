
using LythumOSL.Indigo.Metadata;

namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class DictionaryColumn : ColumnDesc, IColumnDictionaryDesc
		{
			#region ctor


			public DictionaryColumn (
				string name,
				string displayName,
				ITableDesc tableDesc,
				int idColumn,
				int nameColumn)
				: base (name, displayName)
			{
				this.TableDesc = tableDesc;
				this.IdColumn = idColumn;
				this.NameColumn = nameColumn;
			}

			/// <summary>
			/// Dictionary initialization with default id and name columns (0 - id column, 1 - name column)
			/// </summary>
			/// <param name="name"></param>
			/// <param name="displayName"></param>
			/// <param name="tableDesc"></param>
			public DictionaryColumn (
				string name,
				string displayName,
				ITableDesc tableDesc)
				: this (name, displayName, tableDesc, 0, 1)
			{
			}

			#endregion

			#region IColumnDictionaryDesc Members

			public ITableDesc TableDesc { get; protected set; }

			public int IdColumn { get; protected set; }
			public int NameColumn { get; protected set; }

			#endregion
		}
	}
}
