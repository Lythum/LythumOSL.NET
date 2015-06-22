
namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class ForeignKeyColumn : ColumnDesc
		{
			public ForeignKeyColumn (string name)
				: base (name, string.Empty)
			{
				this.Visible = false;
				this.ForeignKey = true;
				this.ReadOnly = true;
			}
		}

	}
}
