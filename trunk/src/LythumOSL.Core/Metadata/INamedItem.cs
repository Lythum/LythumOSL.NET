using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface INamedItem : ILythumBase
	{
		string Name { get; set;  }
	}
}
