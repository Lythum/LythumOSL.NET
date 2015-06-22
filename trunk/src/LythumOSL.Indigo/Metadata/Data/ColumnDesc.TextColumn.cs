
namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc
	{
		public class TextColumn : ColumnDesc
		{
			public TextColumn (string name, string displayName)
				: base (name, displayName)
			{
				base.DefaultValue = string.Empty;
			}
		}
	}
}
