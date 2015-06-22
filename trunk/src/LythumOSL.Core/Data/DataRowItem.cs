using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	public class DataRowItem : IDataRowItem
	{
		#region Constants
		public const string CategoryObjectInformation = "Objekto informacija";
		public const string CategorySystemInformation = "Sisteminë informacija";

		#endregion

		#region Attributes
		string _NameOfId = "_id";
		string _NameOfName = "name";
		DataRow _Row = null;
		Dictionary<string, object> _RowInfo = new Dictionary<string, object> ();
		bool _IsNewItem;

		#endregion

		#region Abstract

		#endregion

		#region Virtual

		/// <summary>
		/// Alpha.Indigo.DataTypes.DataRowItem::Validate not really implemented and set only as virtual
		/// </summary>
		/// <returns></returns>
		public virtual string Validate ()
		{
			return string.Empty;
		}

		#endregion

		#region properties

		[Browsable(false)]
		protected Dictionary<string, object> RowInfo
		{
			get { return _RowInfo; }
		}

		protected virtual string NameOfId
		{
			get { return _NameOfId; }
			set { _NameOfId = value; }
		}

		protected virtual string NameOfName
		{
			get { return _NameOfName; }
			set { _NameOfName = value; }
		}

		/// <summary>
		/// DataRow member
		/// </summary>
		[Browsable(false)]
		public DataRow Row
		{
			get
			{
				return _Row;
			}
		}

		//public object this[int index] 
		//{
		//    get
		//    {
		//        object retVal = string.Empty;

		//        if (_Row != null)
		//        {
		//            retVal = _Row[index];
		//        }

		//        return retVal;
		//    }
		//    set
		//    {
		//        if (_Row != null)
		//        {
		//            _Row.BeginEdit ();
		//            _Row[index] = value;
		//            _Row.EndEdit ();
		//        }
		//    }
		//}

		public object this[string columnName]
		{
			get
			{
				object retVal = string.Empty;

				if (_Row != null)
				{
					retVal = _Row[columnName];
				}
				else if (RowInfo.ContainsKey (columnName))
				{
					return RowInfo[columnName];
				}

				return retVal;
			}
			set
			{
				if (_Row != null)
				{
					// assign only if diferent
					if (_Row[columnName] != value)
					{
						if (_Row.Table.Columns[columnName].ReadOnly)
						{
							_Row.Table.Columns[columnName].ReadOnly = false;
						}

						_Row.BeginEdit ();
						_Row[columnName] = value;
						_Row.EndEdit ();
					}
				}

				RowInfo[columnName] = value;
			}
		}

		#endregion

		#region ctor/destr

		public DataRowItem (DataRow row)
		{
			_Row = row;


			_IsNewItem = _Row == null;
		}

		public DataRowItem (
			DataRow row,
			string nameOfId,
			string nameOfName)
			: this (row)
		{
			_NameOfId = nameOfId;
			_NameOfName = nameOfName;
		}

		#endregion

		#region INamedItem Members

		[Category (DataRowItem.CategoryObjectInformation)]
		[DisplayName ("Pavadinimas")]
		[DefaultValue ("")]
		[Description ("Pavadinimas")]
		public virtual string Name
		{
			get
			{
				return this[_NameOfName].ToString();
			}
			set
			{
				this[_NameOfName] = value;
			}
		}

		#endregion

		#region IItem Members

#if DEBUG
		[Category (DataRowItem.CategorySystemInformation)]
		[DisplayName ("Unikalus identifikatorius")]
		[DefaultValue ("")]
		[Description ("Unikalus identifikatorius.")]
#else
		[Browsable(false)] // it hide property
#endif
		public virtual string Id
		{
			get
			{
				return this[_NameOfId].ToString ();
			}
			protected set
			{
				this[_NameOfId] = value;
			}
		}

#if DEBUG
		[DisplayName ("Naujas"), DefaultValue ("")]
		[Category (DataRowItem.CategorySystemInformation)]
		[Description ("true = naujas irasas.")]
#else
		[Browsable(false)] // it hide property
#endif
		public bool IsNewItem
		{
			get
			{
				if (_Row != null)
				{
					return _Row.RowState == DataRowState.Added ||
						_Row.RowState == DataRowState.Detached ||
						_IsNewItem;
				}
				else
				{
					return true;
				}
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// It converts item to new
		/// Cleanup Id and IsNewItem property
		/// </summary>
		public virtual void ConvertToNew ()
		{
			if (!IsNewItem)
			{
				if (Row != null)
				{
					if (Row.Table != null)
					{
						Row.Table.Columns[NameOfId].ReadOnly = false;
					}
				}

				Id = "0";
				_IsNewItem = true;
			}
		}
		#endregion
	}
}
