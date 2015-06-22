
namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class HiddenColumn : ColumnDesc
		{
			public HiddenColumn (string name)
				: base (name, string.Empty)
			{
				this.Visible = false;
			}
		}
	}
}
