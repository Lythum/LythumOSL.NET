using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LythumOSL.Core.Data.Info
{
	public enum DbFieldUpdateMode
	{
		IgnoreField,
		Value,
		ValueEmptyToNull,
		ValueEmptyToZero,
		NowEverytime,
		NowOnInsert,
		NowOnUpdate,
		NewGuidOnInsert,
		NewGuidOnUpdate,
	}
}
