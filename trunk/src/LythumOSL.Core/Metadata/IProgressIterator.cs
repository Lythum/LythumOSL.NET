using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Metadata
{
	public interface IProgressIterator
	{
		double Minimum { get; set; }
		double Maximum { get; set; }
		double Value { get; set; }
	}
}
