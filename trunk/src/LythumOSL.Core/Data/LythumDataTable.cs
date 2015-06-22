using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	[Serializable]
	public class LythumDataTable : DataTable, ILythumBase
	{
		#region Enums

		#endregion

		#region Attributes
		int _SelectedIndex = -1;

		string _SelectQuery = string.Empty;

		DbDataAdapter _DataAdapter = null;

		#endregion

		#region Properties

		[XmlIgnore]
		public DbDataAdapter DataAdapter
		{
			get { return _DataAdapter; }
			set { _DataAdapter = value; }
		}

		[XmlIgnore]
		public string SelectQuery
		{
			get
			{
#warning todo: solve this
/*
				if (string.IsNullOrEmpty (_SelectQuery))
				{
					throw new AlphaException ("AlphaDataTable::SelectQuery is not initialized!");
				}
*/
				return _SelectQuery;
			}
			set { _SelectQuery = value; }
		}

		public int SelectedIndex
		{
			get { return _SelectedIndex; }
			set { _SelectedIndex = value; }
		}

		#endregion

		#region CTORS
		public LythumDataTable ()
			: base()
		{
		}

		public LythumDataTable (string name)
			: base (name)
		{
		}

		public LythumDataTable (
			string name, string selectQuery)
			: base(name)
		{
			_SelectQuery = selectQuery;
		}

		public LythumDataTable (DataTable table)
			:base()
		{
			base.TableName = table.TableName;
			base.Rows.Clear ();
		}

		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		/// <param name="delimiter"></param>
		/// <param name="includeHeader"></param>
		/// <param name="progress">Can be null</param>
		public void WriteCSV (
			string file, 
			string delimiter, 
			bool includeHeader,
			IProgressIterator progress)
		{
			Csv.Write (
				file, 
				this, 
				delimiter, 
				includeHeader, 
				progress);
		}

		public bool Update ()
		{
			bool retVal = false;

			if (_DataAdapter != null)
			{
				_DataAdapter.Update (this);
			}
			return retVal;
		}

		/// <summary>
		/// Create and add new row to AlphaDataTable
		/// If data available add data
		/// </summary>
		/// <param name="data">can be null</param>
		/// <returns></returns>
		public DataRow AddNewRow (object[] data)
		{
			DataRow row = NewRow ();

			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					row[i] = data[i];
				}
			}

			Rows.Add (row);

			return row;
		}

		/// <summary>
		/// Checks do any records exist by given filter
		/// </summary>
		/// <param name="filterExpression">
		/// typical DataTable.Select filterExpression
		/// </param>
		/// <returns>true if exist, false if not</returns>
		public bool AnyRecordsExist (string filterExpression) 
		{
			return LythumDataTable.AnyRecordsExist (
				this,
				filterExpression);
		}

		#endregion

		#region static

		public static void PrepareTableForDataGrid (DataTable t)
		{
			PrepareTableForDataGrid (t, true);
		}

		public static void PrepareTableForDataGrid (DataTable t, bool setZeroIncrement )
		{
			if (t != null)
			{
				foreach (DataColumn c in t.Columns)
				{
					if (c.AutoIncrement && setZeroIncrement)
					{
						c.AutoIncrement = false;
						c.DefaultValue = "0";
					}
					else if (c.ReadOnly)
					{
						c.ReadOnly = false;
					}
				}
			}
		}

		/// <summary>
		/// Checks do any records exist by given filter
		/// </summary>
		/// <param name="table">table</param>
		/// <param name="filterExpression">
		/// typical DataTable.Select filterExpression
		/// </param>
		/// <returns>true if exist, false if not</returns>
		public static bool AnyRecordsExist (DataTable table, string filterExpression)
		{
			DataRow[] rows = table.Select (filterExpression);

			return rows.Length > 0;
		}

		public static void SetAllColumnsReadOnly (DataTable table, bool readOnly)
		{
			if (table != null)
			{

				foreach (DataColumn c in table.Columns)
				{
					c.ReadOnly = readOnly;
				}
			}
		}

		/// <summary>
		/// Call DataTable select method corresponding to valid parameters
		/// </summary>
		/// <param name="table"></param>
		/// <param name="filter"></param>
		/// <param name="sort"></param>
		/// <returns></returns>
		public static DataRow[] Select (DataTable table, string filter, string sort)
		{
			Validation.RequireValid (table, "table");

			if (!string.IsNullOrEmpty (filter) && !string.IsNullOrEmpty (sort))
			{
				return table.Select (filter, sort);
			}
			else if (!string.IsNullOrEmpty (filter))
			{
				return table.Select (filter);
			}
			else
			{
				return table.Select ();
			}
		}

		public static DataTable SelectTable (DataTable table, string filter, string sort)
		{
			DataRow[] rows = Select (table, filter, sort);

			DataTable result = table.Clone ();

			foreach (DataRow row in rows)
			{
				result.ImportRow(row);
			}

			return result;
		}

		public static DataView SelectView (DataTable table)
		{
			return SelectView (table, string.Empty, string.Empty);
		}

		public static DataView SelectView (DataTable table, string filter, string sort)
		{
			DataView v = new DataView (table);
			if (string.IsNullOrEmpty (filter) && string.IsNullOrEmpty (sort))
			{
			}
			else
			{
				if (!string.IsNullOrEmpty (filter))
				{
					v.RowFilter = filter;
				}

				if(!string.IsNullOrEmpty(sort))
				{
					v.Sort = sort;
				}
				//return new DataView (table, filter, sort, DataViewRowState.CurrentRows);
			}

			return v;
		}

		public static string[] GetTableColumnAsArray (DataTable table, int column)
		{
			string[] retVal = null;

			if (table != null)
			{
				if (table.Columns.Count > column)
				{
					List<string> array = new List<string> ();

					foreach (DataRow row in table.Rows)
					{
						array.Add (row[column].ToString ());
					}

					retVal = array.ToArray ();
				}
			}

			return retVal;
		}

		/// <summary>
		/// Priskirti reiksme tik jeigu ji kitokia
		/// </summary>
		/// <param name="row"></param>
		/// <param name="rowName"></param>
		/// <param name="value"></param>
		public static void AssignRowValueIfDiferent (
			DataRow row,
			string rowName,
			object value)
		{
			if (!row[rowName].Equals (value))
			{
				row[rowName] = value;
			}
		}

		public static void InsertFirstRow (
			DataTable table,
			object[] values)
		{
			if (values != null)
			{
				DataRow row = table.NewRow ();

				for (int i = 0; i < values.Length; i++)
				{
					row[i] = values[i];
				}

				table.Rows.InsertAt (row, 0);
			}
		}

		public static LythumDataTable CreateStringsTable (string name, string[] columns)
		{
			return CreateTable<string> (name, columns);
		}

		public static LythumDataTable CreateTable<T> (string name, string[] columns)
		{
			Validation.RequireValidString (name, "name");
			Validation.RequireValid (columns, "columns");

			LythumDataTable table = new LythumDataTable (name);

			foreach (string column in columns)
			{
				table.Columns.Add (column, typeof (T));
			}

			return table;
		}
		
		public static bool IsNullOrEmpty (DataTable table)
		{
			if (table != null)
			{
				if (table.Rows.Count > 0)
				{
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}
