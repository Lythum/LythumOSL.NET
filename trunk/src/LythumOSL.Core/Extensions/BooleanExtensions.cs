using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Extensions
{
	public static class BooleanExtensions
	{
		public static int ToInt (this Boolean value)
		{
			return (value ? 1 : 0);
		}
	}
}
