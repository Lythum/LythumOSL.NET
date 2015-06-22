using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IColumnListDesc
	{
		DataTable ListData { get; }
		int IdColumn { get; }
		int NameColumn { get; }
	}
}
