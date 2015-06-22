using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

using LythumOSL.Core.Metadata;
using LythumOSL.Core.Data.Info;

using LythumOSL.Indigo.Enums;

namespace LythumOSL.Indigo.Metadata
{
	/// <summary>
	/// Derived from named item, which has Name property
	/// </summary>
	public interface IColumnDesc : IMetadataDbItem
	{
		DataColumnType ColumnType { get; set; }
		ITableDesc Table { get; set; }

		string SelectFormula { get; set; }
		object DefaultValue { get; set; }

		/// <summary>
		/// Affects Searcheable
		/// </summary>
		bool Visible { get; set; }
		
		/// <summary>
		/// Required is allways saved to database
		/// Affects visible
		/// </summary>
		bool Required { get; set; }
		
		/// <summary>
		/// Affects visible
		/// </summary>
		bool Searcheable { get; set; }

		/// <summary>
		/// Primary keys are supposed to be auto assigned by database. 
		/// Affects ReadOnly, Visible, Searcheable (default false)
		/// </summary>
		bool PrimaryKey { get; set; }
		
		/// <summary>
		/// Primary keys are supposed to be auto assigned by database. 
		/// Affects ReadOnly, Visible, Searcheable (default false)
		/// </summary>
		bool ForeignKey { get; set; }

		/// <summary>
		/// default 0
		/// </summary>
		int Order { get; set; }

		DataColumn DataColumn { get; }

		bool IsTechnicalColumn { get; }
		bool IsSelectFormulaUsed { get; }

		IValueConverter Converter { get; }

		void Prepare (ITableDesc table);

		/// <summary>
		/// Field select value
		/// </summary>
		/// <param name="f"></param>
		/// <returns>select field or empty</returns>
		string FieldSelect (IDatabaseInfo i);


		/// <summary>
		/// Preprocessing value before saving to database
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		string ValueToString (object value);

		/// <summary>
		/// Processing after got result from database
		/// </summary>
		/// <param name="table"></param>
		void QueryPostProcess (DataTable table);
	}
}
