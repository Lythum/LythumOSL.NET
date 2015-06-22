using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Enums;
using LythumOSL.Core.Data.Info;

namespace LythumOSL.Core.Metadata
{
	public interface IDataAccess : ILythumBase
	{
		DataTable Query (string sql);
		DataSet QueryDataSet (string sql);
		string QueryScalar (string sql);
		int Execute (string sql);
		// db info
		DatabaseTypes DatabaseType { get; }
		DatabasesInfo Info { get; }
	}
}
