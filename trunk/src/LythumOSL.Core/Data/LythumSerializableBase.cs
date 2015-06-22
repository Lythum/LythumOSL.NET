using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	public abstract class LythumSerializableBase<T> : ILythumBase
		where T : LythumSerializableBase<T>, new()
	{
		public virtual string Serialize ()
		{
			return LythumOSL.Core.Data.Xml.Xml.Serialize<T> ((T)this);
		}

		public T Deserialize (string xml)
		{
			return LythumOSL.Core.Data.Xml.Xml.Deserialize<T> (xml);
		}

	}
}
