using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Data
{
	public class LythumDataColumn : DataColumn
	{
		// Summary:
		//     Initializes a new instance of a System.Data.DataColumn class as type string.
		public LythumDataColumn ()
			: base ()
		{
		}
		//
		// Summary:
		//     Inititalizes a new instance of the System.Data.DataColumn class, as type
		//     string, using the specified column name.
		//
		// Parameters:
		//   columnName:
		//     A string that represents the name of the column to be created. If set to
		//     null or an empty string (""), a default name will be specified when added
		//     to the columns collection.
		public LythumDataColumn (string columnName)
			: base (columnName)
		{
		}
		//
		// Summary:
		//     Inititalizes a new instance of the System.Data.DataColumn class using the
		//     specified column name and data type.
		//
		// Parameters:
		//   columnName:
		//     A string that represents the name of the column to be created. If set to
		//     null or an empty string (""), a default name will be specified when added
		//     to the columns collection.
		//
		//   dataType:
		//     A supported System.Data.DataColumn.DataType.
		//
		// Exceptions:
		//   System.ArgumentNullException:
		//     No dataType was specified.
		public LythumDataColumn (string columnName, Type dataType)
			: base (columnName, dataType)
		{
		}
		//
		// Summary:
		//     Initializes a new instance of the System.Data.DataColumn class using the
		//     specified name, data type, and expression.
		//
		// Parameters:
		//   columnName:
		//     A string that represents the name of the column to be created. If set to
		//     null or an empty string (""), a default name will be specified when added
		//     to the columns collection.
		//
		//   dataType:
		//     A supported System.Data.DataColumn.DataType.
		//
		//   expr:
		//     The expression used to create this column. For more information, see the
		//     System.Data.DataColumn.Expression property.
		//
		// Exceptions:
		//   System.ArgumentNullException:
		//     No dataType was specified.
		public LythumDataColumn (string columnName, Type dataType, string expr)
			: base (columnName, dataType, expr)
		{
		}
		//
		// Summary:
		//     Initializes a new instance of the System.Data.DataColumn class using the
		//     specified name, data type, expression, and value that determines whether
		//     the column is an attribute.
		//
		// Parameters:
		//   type:
		//     One of the System.Data.MappingType values.
		//
		//   columnName:
		//     A string that represents the name of the column to be created. If set to
		//     null or an empty string (""), a default name will be specified when added
		//     to the columns collection.
		//
		//   dataType:
		//     A supported System.Data.DataColumn.DataType.
		//
		//   expr:
		//     The expression used to create this column. For more information, see the
		//     System.Data.DataColumn.Expression property.
		//
		// Exceptions:
		//   System.ArgumentNullException:
		//     No dataType was specified.
		public LythumDataColumn (string columnName, Type dataType, string expr, MappingType type)
			: base (columnName, dataType, expr, type)
		{
		}

		// Own constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName">Column name - used in data bindings</param>
		/// <param name="dataType">type of column</param>
		/// <param name="caption">caption used as header value</param>
		/// <param name="unique"></param>
		/// <param name="notNull"></param>
		public LythumDataColumn (
			string columnName, Type dataType, string caption, bool unique, bool notNull)
			: base (columnName, dataType)
		{
			Caption = caption;
			Unique = unique;
			AllowDBNull = !notNull;
		}
	}
}
