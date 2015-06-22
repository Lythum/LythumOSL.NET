using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Indigo.Metadata
{
	public interface IMetadataItem : INamedItem
	{
		string DisplayName { get; set; }
		string Description { get; set; }

		bool ReadOnly { get; set; }
	}
}
