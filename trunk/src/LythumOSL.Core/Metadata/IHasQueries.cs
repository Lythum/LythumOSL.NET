using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IHasQueries
	{
		List<string> Queries { get; }
	}
}
