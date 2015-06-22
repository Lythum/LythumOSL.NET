using System;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Data
{
	public class LythumDictionaryEntry<TKey, TValue>
	{
		TKey _Key;
		TValue _Value;

		public TKey Key
		{
			get { return _Key; }
			set { _Key = value; }
		}

		public TValue Value
		{
			get { return _Value; }
			set { _Value = value; }
		}

		public LythumDictionaryEntry ()
		{
		}

		public LythumDictionaryEntry (TKey key, TValue value)
		{
			_Key = key;
			_Value = value;
		}
	}
}
