using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IDataRowItem : IDbNamedItem
	{
		[Browsable(false)]
		DataRow Row { get; }

		object this[string columnName] { get; set; }
	}
}
