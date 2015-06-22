#define COLUMN_VALIDATION
using System;
using System.Data;
using System.Windows.Data;
using System.Windows;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.Indigo.Enums;
using LythumOSL.Indigo.Metadata;

namespace LythumOSL.Indigo.Data
{
	public partial class ColumnDesc :  Binding, IColumnDesc
	{
		#region Attributes
		string _Name;
		bool _ReadOnly;
		bool _Required;

		#endregion

		#region IColumnDesc Members

		public DataColumnType ColumnType { get; set; }
		public string SelectFormula { get; set; }
		public string DisplayName { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }
		public ITableDesc Table { get; set; }

		/// <summary>
		/// Default value, but if its value is null it won't be used as initialization value
		/// </summary>
		public object DefaultValue { get; set; }

		public bool Visible { get; set; }
		public bool Searcheable { get; set; }
		public bool PrimaryKey { get; set; }
		public bool ForeignKey { get; set; }

		public DataColumn DataColumn { get; set; }
		public int Order { get; set; }

		#region Implemented properties

		public bool IsSelectFormulaUsed
		{
			get
			{
				return _Name != SelectFormula;
			}
		}

		public bool IsTechnicalColumn
		{
			get
			{
				if (PrimaryKey || ForeignKey || !Visible)
				{
					return true;
				}

				if (!string.IsNullOrEmpty (Name))
				{
					if (Name[0].Equals ('_'))
					{
						return true;
					}
				}

				return false;
			}
		}
		/// <summary>
		/// Used as machine name, also as field name in sql queries if SelectFormula is not set.
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (!IsSelectFormulaUsed)
				{
					SelectFormula = value;
				}

				_Name = value;
				base.Path = new PropertyPath (value);
			}
		}

		public bool Required
		{
			get
			{
				return _Required;
			}
			set
			{
				_Required = value;

#if COLUMN_VALIDATION
				if (_Required)
				{
				    base.ValidationRules.Add (new Validators.EmptyValidator ());
				}
				else
				{
				    this.ValidationRules.Clear ();
				}
#endif
			}
		}

		public bool ReadOnly
		{
			get
			{
				return _ReadOnly;
			}
			set
			{
				_ReadOnly = value;

				if (value)
				{
					base.Mode = BindingMode.OneWay;
				}
				else
				{
					base.Mode = BindingMode.TwoWay;
				}
			}
		}

		#endregion

		#endregion

		#region Ctor
		public ColumnDesc (string name)
			: base(name)
		{
			this.Name = string.Empty;
			this.DisplayName = string.Empty;
			this.Alias = string.Empty;
			this.Description = string.Empty;

			this.DefaultValue = DBNull.Value;
			this.Visible = true;
			this.ReadOnly = false;
			this.Required = false;
			this.Searcheable = false;
			this.PrimaryKey = false;
			this.ForeignKey = false;
			this.Order = 0;
			this.ColumnType = DataColumnType.Text;
			this.DataColumn = null;
		}

		public ColumnDesc (
			string name, 
			string displayName)
			: this (name)
		{
			this.Name = name;
			this.DisplayName = displayName;
		}

		#endregion

		#region Methods

		public void Prepare (ITableDesc table)
		{
			this.Table = table;
		}

		/// <summary>
		/// Field select value
		/// </summary>
		/// <param name="f"></param>
		/// <returns>select field or empty</returns>
		public virtual string FieldSelect (IDatabaseInfo i)
		{
			string retVal = string.Empty;

			if (this.IsSelectFormulaUsed)
			{
				retVal += i.GetFormula (this.SelectFormula, this.Name);
			}
			else
			{
				retVal += i.GetFieldName (Table.Alias, this.Name);
			}

			return retVal;
		}

		public virtual string ValueToString (object value)
		{
			// validate value
			if (DBNull.Value.Equals (value))
			{
				throw new LythumException ("Nulls must be processed in ITableDesc");
			}

			// validate DataColumn
			if (DataColumn == null)
			{
				throw new LythumException ("DataColumn must be initialized in ColumnDesc::QueryPostProcess!");
			}

			switch (DataColumn.DataType.ToString())
			{
				case "System.Decimal":
					return value.ToString ().Replace (',', '.');

				default:
					return value.ToString ();
			}
		}

		/// <summary>
		/// Processing after got result from database
		/// </summary>
		/// <param name="table"></param>
		public virtual void QueryPostProcess (DataTable table)
		{
			DataColumn = table.Columns[this.Name];
			DataColumn.ReadOnly = this.ReadOnly;
			DataColumn.DefaultValue = this.DefaultValue;
		}

		#endregion

	}
}
