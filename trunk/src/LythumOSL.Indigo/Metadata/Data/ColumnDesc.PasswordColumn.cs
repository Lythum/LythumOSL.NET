using System.Data;

using LythumOSL.Core.Metadata;
using LythumOSL.Security;

namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class PasswordColumn : TextColumn
		{
			public PasswordColumn (string name, string displayName)
				: base (name, displayName)
			{

			}

			public override string FieldSelect (IDatabaseInfo i)
			{
				return string.Empty;
			}

			public override void QueryPostProcess (DataTable table)
			{
				if (!table.Columns.Contains (this.Name))
				{
					table.Columns.Add (this.Name, typeof (string));
				}

				base.QueryPostProcess (table);
				
			}

			public override string ValueToString (object value)
			{
				string str = base.ValueToString (value);
				return Md5.Encrypt (str);
			}
		}
	}
}
