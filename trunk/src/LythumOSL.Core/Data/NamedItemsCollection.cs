using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	public class NamedItemsCollection<T> : ILythumBase, IEnumerable<T>, ICollection<T>
		 where T : INamedItem
	{
		#region Attributes

		Dictionary<string, T> _Items;

		/// <summary>
		/// For XML Serialization, to initialize this collection
		/// </summary>
		public T[] Items
		{
			get
			{
				return _Items.Values.ToArray ();
			}
			set
			{
				Clear ();

				if (value != null)
				{
					AddRange (value);
				}
			}
		}

		#endregion

		#region Properties
		public T this[string key] 
		{
			get 
			{
				return _Items[key];
			}
			set
			{
				Validation.RequireValid (key, "key");
				Validation.RequireValid (value, "value");

				if (_Items.ContainsKey (key))
				{
					_Items[key] = value;
				}
				else
				{
					_Items.Add (value.Name, value);
				}
			}
		}

		#endregion

		#region Ctor

		public NamedItemsCollection ()
		{
			_Items = new Dictionary<string, T> ();
		}

		public NamedItemsCollection (T[] items)
		{
			AddRange(items);
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator ()
		{
			return _Items.Values.GetEnumerator ();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return _Items.Values.GetEnumerator ();
		}

		#endregion

		#region ICollection<T> Members

		public void Add (T item)
		{
			Validation.RequireValid (item, "item");

			_Items.Add (item.Name, item);
		}

		public void AddRange (T[] items)
		{
			if(items != null)
			{
				foreach(T item in items)
				{
					Add(item);
				}
			}
		}

		public void Clear ()
		{
			_Items.Clear ();
		}

		public bool Contains (T item)
		{
			return _Items.Values.Contains (item);
		}

		public bool ContainsKey (string key)
		{
			return _Items.ContainsKey (key);
		}

		public void CopyTo (T[] array, int arrayIndex)
		{
			_Items.Values.CopyTo (array, arrayIndex);
		}

		public int Count
		{
			get { return _Items.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove (T item)
		{
			return _Items.Remove (item.Name);

		}

		#endregion
	}
}
