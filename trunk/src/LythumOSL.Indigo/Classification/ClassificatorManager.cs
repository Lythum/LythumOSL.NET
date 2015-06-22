using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Microsoft.Windows.Controls;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.Indigo.Enums;
using LythumOSL.Indigo.Metadata;
using LythumOSL.Indigo.Validators;

namespace LythumOSL.Indigo.Classification
{
	public class ClassificatorManager : IClassificatorManager
	{
		#region Attributes
		DataTable _Table;

		#endregion

		public ClassificatorManager (
			IDataAccess access,
			ITableDesc tableDesc,
			IClassificatorCredentials credentials)
		{
			Validation.RequireValid (access, "access");
			Validation.RequireValid (tableDesc, "tableDesc");
			Validation.RequireValid (credentials, "credentials");

			this.DataAccess = access;
			this.TableDescription = tableDesc;
			this.Credentials = credentials;
		}

		#region IClassificatorManager Members

		public IDataAccess DataAccess { get; protected set; }
		public ITableDesc TableDescription { get; protected set; }
		public IClassificatorCredentials Credentials { get; protected set;  }


		public DataTable Search ()
		{
			TableDescription.Prepare ();

			_Table = DataAccess.Query (
				TableDescription.QuerySelect (DataAccess.Info));

			TableDescription.QueryPostProcess (_Table);

			return _Table;
		}

		public void InitUI (DataGrid grid)
		{
			grid.Columns.Clear ();
			grid.AutoGenerateColumns = false;

			grid.RowValidationRules.Clear ();
			grid.RowValidationRules.Add (new TableDescValidator (TableDescription));

			foreach (IColumnDesc cd in TableDescription.Columns)
			{
				// column
				DataGridColumn column;

				switch (cd.ColumnType)
				{
					default:
					case DataColumnType.Text:
						column = CreateDataGridColumn<DataGridTextColumn> (cd);
						break;

					case DataColumnType.CheckBox:
						column = CreateDataGridColumn<DataGridCheckBoxColumn> (cd);
						break;

					case DataColumnType.ComboBox:
						column = CreateDataGridColumn<DataGridComboBoxColumn> (cd);
						break;

					case DataColumnType.Dictionary:
						column = CreateDataGridColumn<DataGridTextColumn> (cd);
						column.IsReadOnly = true;
						break;

				}

				// Init column name
				if (String.IsNullOrEmpty (cd.DisplayName))
				{
					column.Header = cd.Name;
				}
				else
				{
					column.Header = cd.DisplayName;
				}

				if (cd.Visible)
				{
					column.Visibility = Visibility.Visible;
				}
				else
				{
					column.Visibility = Visibility.Hidden;
				}

				grid.Columns.Add (column);
			}
		}

		public void PopulateResult (DataGrid grid, DataTable table)
		{
			if (table == null)
				return;

			foreach (DataColumn c in table.Columns)
			{
				// looking for IColumnDesc
				if (!TableDescription.Columns.ContainsKey (c.ColumnName))
					continue; // resume next if not found
				IColumnDesc cd = TableDescription.Columns[c.ColumnName];

				// ReadOnly
				c.AutoIncrement = false;
			}

			table.AcceptChanges ();
			grid.ItemsSource = table.DefaultView;

		}

		public bool Save ()
		{
			if (_Table == null)
				return false;

			DataTable changes = _Table.GetChanges ();
			bool changesAffected = false;

			if (changes != null)
			{
				foreach (DataRow r in changes.Rows)
				{
					switch (r.RowState)
					{
						case DataRowState.Added:
							DataAccess.Execute (TableDescription.QueryInsert (DataAccess.Info, r));
							changesAffected = true;
							break;

						case DataRowState.Modified:
							DataAccess.Execute (TableDescription.QueryUpdate (DataAccess.Info, r));
							changesAffected = true;
							break;

						default:
							break;
					}
				}
			}

			return changesAffected;
		}

		public void Delete (DataRow[] rows)
		{
			if (rows == null)
				return;

			string sql = string.Empty;

			foreach (DataRow row in rows)
			{
				sql += TableDescription.QueryDelete (DataAccess.Info, row, Credentials.UserId) +
					DataAccess.Info.QueryTerminator + '\r' + '\n';
			}

			DataAccess.Execute (sql);
		}

		#endregion

		#region Helpers
		/// <summary>
		/// Creates generic type DataGridColumn and binds it
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="cd"></param>
		/// <returns></returns>
		public static DataGridColumn CreateDataGridColumn<T> (IColumnDesc cd)
			where T : DataGridColumn, new ()
		{
			DataGridColumn column = new T ();

			if (column is DataGridBoundColumn && cd is Binding)
			{
				((DataGridBoundColumn)column).Binding = (Binding)cd;
			}


			return column;
		}


		#endregion
	}
}
