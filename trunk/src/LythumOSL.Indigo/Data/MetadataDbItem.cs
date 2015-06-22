
using System.Windows;
using LythumOSL.Indigo.Metadata;
using LythumOSL.Indigo.Enums;

namespace LythumOSL.Indigo.Data
{
	public class MetadataDbItem : IMetadataDbItem
	{
		#region IMetadataDbItem Members

		public string Alias { get; set; }

		public DataColumnType ColumnType { get; set; }

		#endregion

		#region IMetadataItem Members

		public string DisplayName { get; set; }

		public string Description { get; set; }

		public bool ReadOnly { get; set; }

		#endregion

		#region INamedItem Members

		public string Name { get; set; }

		#endregion

		#region Ctor

		public MetadataDbItem ()
		{
			Name = string.Empty;
			DisplayName = string.Empty;
			Description = string.Empty;
			Alias = string.Empty;
			ColumnType = DataColumnType.Text;
			ReadOnly = false;
		}

		#endregion
	}
}
