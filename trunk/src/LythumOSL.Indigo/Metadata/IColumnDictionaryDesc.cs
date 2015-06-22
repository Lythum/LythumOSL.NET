using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Indigo.Metadata
{
	public interface IColumnDictionaryDesc
	{
		ITableDesc TableDesc { get; }
		int IdColumn { get; }
		int NameColumn { get; }
	}
}
