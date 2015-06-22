using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Indigo.Enums;

namespace LythumOSL.Indigo.Metadata
{
	public interface IMetadataDbItem : IMetadataItem
	{
		string Alias { get; set; }
		DataColumnType ColumnType { get; set; }
	}
}
