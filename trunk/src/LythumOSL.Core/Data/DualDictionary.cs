using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#warning todo: DualDictionary is never used
namespace LythumOSL.Core.Data
{
	public class DualDictionary<K, V> : IDictionary<K, V>
	{
		IDictionary<K, V> _KeyValue;
		IDictionary<V, K> _ValueKey;

		public IDictionary<K, V> KeyValue
		{
			get { return _KeyValue; }
		}

		public IDictionary<V, K> ValueKey
		{
			get { return _ValueKey; }
		}

		public DualDictionary ()
		{
			_KeyValue = new Dictionary<K, V> ();
			_ValueKey = new Dictionary<V, K> ();
		}


		public bool ContainsValue (V value)
		{
			return _ValueKey.ContainsKey (value);
		}

		public K ByValue (V value)
		{
			return _ValueKey[value];
		}


		#region IDictionary<K,V> Members

		public void Add (K key, V value)
		{
			_KeyValue.Add (key, value);
			_ValueKey.Add (value, key);
		}

		public bool ContainsKey (K key)
		{
			return _KeyValue.ContainsKey (key);
		}

		public ICollection<K> Keys
		{
			get { return _KeyValue.Keys; }
		}

		public bool Remove (K key)
		{
			if (_KeyValue.ContainsKey (key))
			{
				_ValueKey.Remove (_KeyValue[key]);
				_KeyValue.Remove (key);

				return true;
			}
			return false;
		}

		public bool TryGetValue (K key, out V value)
		{
			return _KeyValue.TryGetValue (key, out value);
		}

		public ICollection<V> Values
		{
			get { return _KeyValue.Values; }
		}

		public V this[K key]
		{
			get
			{
				return _KeyValue[key];
			}
			set
			{
				// backing up valuekey value
				V old = _KeyValue[key];
				// assigning value for keyvalue
				_KeyValue[key] = value;

				// removing old valuekey
				_ValueKey.Remove (old);
				// adding new valuekey
				_ValueKey.Add (value, key);
			}
		}

		#endregion

		#region ICollection<KeyValuePair<K,V>> Members

		public void Add (KeyValuePair<K, V> item)
		{
			Add (item.Key, item.Value);
		}

		public void Clear ()
		{
			_KeyValue.Clear ();
			_ValueKey.Clear ();
		}

		public bool Contains (KeyValuePair<K, V> item)
		{
			return _KeyValue.Contains (item);
		}

		public void CopyTo (KeyValuePair<K, V>[] array, int arrayIndex)
		{
			_KeyValue.CopyTo (array, arrayIndex);
		}

		public int Count
		{
			get { return _KeyValue.Count; }
		}

		public bool IsReadOnly
		{
			get { return _KeyValue.IsReadOnly; }
		}

		public bool Remove (KeyValuePair<K, V> item)
		{
			if (_KeyValue.Contains (item))
			{
				_ValueKey.Remove (item.Value);
				_KeyValue.Remove (item.Key);

				return true;
			}
			return false;
		}

		#endregion

		#region IEnumerable<KeyValuePair<K,V>> Members

		public IEnumerator<KeyValuePair<K, V>> GetEnumerator ()
		{
			return _KeyValue.GetEnumerator ();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return _KeyValue.GetEnumerator ();
		}

		#endregion
	}
}
