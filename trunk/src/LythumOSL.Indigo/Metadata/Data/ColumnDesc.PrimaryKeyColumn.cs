
namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class PrimaryKeyColumn : ColumnDesc
		{
			public const string DefaultColumnName = "_id";

			public PrimaryKeyColumn ()
				: base (DefaultColumnName, string.Empty)
			{
				this.Visible = false;
				this.PrimaryKey = true;
				this.ReadOnly = true;
				this.DefaultValue = "0";
			}

			public PrimaryKeyColumn (string name)
				: this ()
			{
				this.Name = name;
			}
		}
	}
}
