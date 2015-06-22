using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Data.Info;

namespace LythumOSL.Core.Data
{
	/// <summary>
	/// Defined to describle database tables structure
	/// </summary>
	public class Database
	{
		public enum ForWhat
		{
			ForUpdate,
			ForInsert,
		}

		/// <summary>
		/// Gets values from datarow, with some preformatting by DbFieldInfo information
		/// </summary>
		/// <param name="info">Database metadata info</param>
		/// <param name="r">DataRow</param>
		/// <param name="fieldInfo">Fields information</param>
		/// <param name="forWhat">For which query type are those fields</param>
		/// <returns>fields array</returns>
		public static string[] GetValues (DatabasesInfo info, DataRow r, DbFieldInfo[] fieldInfo, ForWhat forWhat)
		{
			List<string> retVal = new List<string> ();

			if (fieldInfo != null)
			{
				foreach (DbFieldInfo i in fieldInfo)
				{
					if (r.Table.Columns.Contains (i.Name))
					{
						Type columnType = r.Table.Columns[i.Name].DataType;

						// dates NOW()/GETDATE()
						if ((i.UpdateMode == DbFieldUpdateMode.NowEverytime) ||
							(i.UpdateMode == DbFieldUpdateMode.NowOnInsert && forWhat == ForWhat.ForInsert) ||
							(i.UpdateMode == DbFieldUpdateMode.NowOnUpdate && forWhat == ForWhat.ForUpdate))
						{
							retVal.Add (info.FunctionNow);
						}
						// New guid
						else if (i.UpdateMode == DbFieldUpdateMode.NewGuidOnInsert && forWhat == ForWhat.ForInsert ||
							i.UpdateMode == DbFieldUpdateMode.NewGuidOnUpdate && forWhat == ForWhat.ForUpdate)
						{
							retVal.Add ("'" + NewGuid() + "'");
						}
						// NULLS
						else if ((DBNull.Value.Equals (r[i.Name]) || r[i.Name] == null) )
						{
							switch (i.UpdateMode)
							{
								default:
								case DbFieldUpdateMode.ValueEmptyToNull:
									retVal.Add ("NULL");
									break;

								case DbFieldUpdateMode.ValueEmptyToZero:
									retVal.Add ("0");
									break;
							}
						}
						else // if(i.UpdateMode == DbFieldUpdateMode.PureValue)
						{
							if (columnType == typeof (decimal) ||
								columnType == typeof (double) ||
								columnType == typeof (float))
							{
								retVal.Add ("'" + r[i.Name].ToString ().Replace (",", ".") + "'");
							}
							else
							{
								retVal.Add ("'" + Fix.FixString (r[i.Name].ToString ()) + "'");
							}
						}
					}
				}
			}
			return retVal.ToArray();
		}

		/// <summary>
		/// Crafts save query with cleanup, depends on RowState
		/// </summary>
		/// <param name="info">Database metadata information</param>
		/// <param name="r">DataRow with data</param>
		/// <param name="fieldInfo">Fields which need to update</param>
		/// <param name="tableName">DB table name</param>
		/// <param name="primaryKeyName">Primary key's name</param>
		/// <returns>sql query</returns>
		public static string CraftSaveQuery (
			DatabasesInfo info,
			DataRow r,
			DbFieldInfo[] fieldInfo,
			string tableName,
			string primaryKeyName)
		{
			return CraftSaveQuery (
				info, r, fieldInfo, tableName, primaryKeyName, true);
		}

		/// <summary>
		/// Crafts save query, depends on RowState it will generate Update or Insert SQL query
		/// </summary>
		/// <param name="info">Database metadata information</param>
		/// <param name="r">DataRow with data</param>
		/// <param name="fieldInfo">Fields which need to update</param>
		/// <param name="tableName">DB table name</param>
		/// <param name="primaryKeyName">Primary key's name</param>
		/// <param name="cleanup">
		/// Cleanup will remove from query fields:
		/// NowOnUpdate - while will be done insert
		/// NowOnInsert - while will be done update
		/// </param>
		/// <returns>sql query</returns>
		public static string CraftSaveQuery (
			DatabasesInfo info, 
			DataRow r, 
			DbFieldInfo[] fieldInfo, 
			string tableName, 
			string primaryKeyName,
			bool cleanup)
		{
			string sql = string.Empty;

			Validation.RequireValid (r, "r");

			System.Diagnostics.Debug.Print (r.RowState.ToString ());

			switch (r.RowState)
			{
				case DataRowState.Detached:
				case DataRowState.Added:
				case DataRowState.Modified:

					bool newItem = (r[primaryKeyName] == null || // PK is NULL
						DBNull.Value.Equals( r[primaryKeyName]) || // PK is DBNull
						r[primaryKeyName].ToString().Equals("0") || // PK is 0
						r.RowState == DataRowState.Added || r.RowState == DataRowState.Detached // row states is
					);

					if(newItem)
					{
						sql = CraftInsertQuery (info, r, fieldInfo, tableName);
					}
					else
					{
						sql = CraftUpdateQuery (info, r, fieldInfo, tableName, primaryKeyName);
					}
					break;

				default:
					// don't process it
					break;
			}

			return sql;
		}

		/// <summary>
		/// Crafts update query with cleanup, without checking DataRow::RowState
		/// </summary>
		/// <param name="info">Database metadata information</param>
		/// <param name="r">DataRow with data</param>
		/// <param name="fieldInfo">Fields which need to update</param>
		/// <param name="tableName">DB table name</param>
		/// <param name="primaryKeyName">Primary key's name</param>
		/// <returns>sql query</returns>
		public static string CraftUpdateQuery (
			DatabasesInfo info,
			DataRow r,
			DbFieldInfo[] fieldInfo,
			string tableName,
			string primaryKeyName)
		{
			return CraftUpdateQuery (
				info, r, fieldInfo, tableName, primaryKeyName, true);
		}

		/// <summary>
		/// Crafts update query with cleanup, without checking DataRow::RowState
		/// </summary>
		/// <param name="info">Database metadata information</param>
		/// <param name="r">DataRow with data</param>
		/// <param name="fieldInfo">Fields which need to update</param>
		/// <param name="tableName">DB table name</param>
		/// <param name="primaryKeyName">Primary key's name</param>
		/// <param name="cleanup">
		/// Cleanup will remove from query fields:
		/// NowOnUpdate - while will be done insert
		/// NowOnInsert - while will be done update
		/// </param>
		/// <returns>sql query</returns>
		public static string CraftUpdateQuery (
			DatabasesInfo info, 
			DataRow r,
			DbFieldInfo[] fieldInfo,
			string tableName, 
			string primaryKeyName,
			bool cleanup)
		{
			DbFieldInfo[] fields;
			if (cleanup)
			{
				fields = Cleanup (fieldInfo, ForWhat.ForUpdate);
			}
			else
			{
				fields = fieldInfo;
			}

			// update header
			string sql = info.TokenUpdate + " " + info.GetTableName(tableName) + " SET ";
			string[] values = GetValues (info, r, fields, ForWhat.ForUpdate);

			// i=value
			List<string> setters = new List<string> ();
			for (int i = 0; i < fields.Length; i++)
			{
				setters.Add(
					info.GetFieldName (string.Empty, fields[i].Name) + " = " + values[i]);
			}

			sql += string.Join (", ", setters.ToArray ());

			// whereCause
			sql += " " + info.TokenWhere + " " + info.GetFieldName (null, primaryKeyName) + " = '" + r[primaryKeyName].ToString () + "'";

			return sql;
		}

		/// <summary>
		/// Crafts insert query with cleanup, without checking DataRow::RowState
		/// </summary>
		/// <param name="info">Database metadata information</param>
		/// <param name="r">DataRow with data</param>
		/// <param name="fieldInfo">Fields which need to update</param>
		/// <param name="tableName">DB table name</param>
		/// <returns>sql query</returns>		
		public static string CraftInsertQuery (
			DatabasesInfo info,
			DataRow r,
			DbFieldInfo[] fieldInfo,
			string tableName)
		{
			return CraftInsertQuery (
				info, r, fieldInfo, tableName, true);
		}

		/// <summary>
		/// Crafts insert query with cleanup, without checking DataRow::RowState
		/// </summary>
		/// <param name="info">Database metadata information</param>
		/// <param name="r">DataRow with data</param>
		/// <param name="fieldInfo">Fields which need to update</param>
		/// <param name="tableName">DB table name</param>
		/// <param name="cleanup">
		/// Cleanup will remove from query fields:
		/// NowOnUpdate - while will be done insert
		/// NowOnInsert - while will be done update
		/// </param>
		/// <returns>sql query</returns>		
		public static string CraftInsertQuery (
			DatabasesInfo info, 
			DataRow r,
			DbFieldInfo[] fieldInfo, 
			string tableName,
			bool cleanup)
		{
			DbFieldInfo[] fields;
			if (cleanup)
			{
				fields = Cleanup (fieldInfo, ForWhat.ForInsert);
			}
			else
			{
				fields = fieldInfo;
			}

			// collecting field names for join
			List<string> names = new List<string> ();
			foreach (DbFieldInfo i in fields)
			{
				names.Add (i.Name);
			}

			// insert header
			string sql = info.TokenInsert + " " + info.GetTableName (tableName) + " (";

			// field names
			sql += info.FieldNamePrefix + string.Join (
				info.FieldNamePostfix + ", " + info.FieldNamePrefix,
				names.ToArray()) + info.FieldNamePostfix;

			sql += ") VALUES (";

			// field values
			string[] values = GetValues(info, r, fields, ForWhat.ForInsert);

			sql += string.Join (", ", values) + ")";


			return sql;
		}

		/// <summary>
		/// Cleans up fields from array which is not required for specific query.
		/// Cleanup will remove fields:
		/// NowOnUpdate - while will be done insert
		/// NowOnInsert - while will be done update
		/// </summary>
		/// <param name="infos"></param>
		/// <param name="forWhat"></param>
		/// <returns></returns>
		static DbFieldInfo[] Cleanup (DbFieldInfo[] infos, ForWhat forWhat)
		{
			List<DbFieldInfo> retVal = new List<DbFieldInfo> ();

			foreach (DbFieldInfo i in infos)
			{
				switch (i.UpdateMode)
				{
					case DbFieldUpdateMode.NowOnUpdate:
						if (forWhat == ForWhat.ForUpdate)
						{
							retVal.Add (i);
						}
						break;

					case DbFieldUpdateMode.NowOnInsert:
						if (forWhat == ForWhat.ForInsert)
						{
							retVal.Add (i);
						}
						break;

					default:
						retVal.Add (i);
						break;
				}
			}

			return retVal.ToArray();
		}

		public static string NewGuid ()
		{
			return Guid.NewGuid ().ToString ("N");
		}

		#region Primitives
		/// <summary>
		/// Typical used db tables fields names
		/// </summary>
		public class Primitives
		{
			public const string Id = "_id";
			public const string Name = "name";
			public const string FullName = "fullname";
			
			public const string Code1 = "code1";
			public const string Code2 = "code2";
			public const string PersonalCode = "personalCode";

			public const string Address = "address";
			public const string BankInfo = "bank_info";
			public const string Balance = "balance";
			public const string CreditLimit = "creditLimit";
			
			public const string Phone = "phone";
			public const string CellPhone = "cell_phone";
			public const string Fax = "fax";
			public const string Email = "email";
			public const string ResponsiblePerson = "responsible_person";
			public const string ManagerId = "_managerId";

			public const string Date = "date";

			public const string IsOwner = "_isOwner";
			public const string IsCompany = "_isCompany";

			public const string Removed = "_removed";
			public const string RemovedDate = "_removedDate";
			public const string RemovedUserId = "_removedUserId";

			public const string CreatedDate = "_createdDate";
			public const string CreatedUserId = "_createdUserId";

			public const string ModifiedDate = "_modifiedDate";
			public const string ModifiedUserId = "_modifiedUserId";

			public const string StateId = "_stateId";
			public const string StateDate = "_stateDate";
			public const string StateUserId = "_stateUserId";


			public const string ParentId = "_parentId";
			public const string ClientId = "_clientId";
			public const string UserId = "_userId";
			public const string ChainId = "_chainId";
			/// <summary>
			/// Saskaitos / Invoice NR
			/// </summary>
			public const string InvoiceId = "_invoiceId";
			public const string InvoiceNo = "invoiceNo";
			public const string Advance = "_advance";
			/// <summary>
			/// Užsakymo id
			/// </summary>
			public const string OrderId = "_orderId";
			public const string OrderNo = "orderNo";

            // savikainos
            public const string Cost01 = "cost01";
            public const string Cost02 = "cost02";
            public const string Cost03 = "cost03";
            public const string Cost04 = "cost04";
            public const string Cost05 = "cost05";
            public const string CostTotal = "costTotal";

			public const string Quantity = "quantity";
			public const string Price = "price";
			public const string PriceId = "_priceId";
			public const string Vat = "vat";
			public const string Sum = "sum";
			public const string Paid = "paid";
			public const string Direction = "_direction";

			public const string Active = "_active";
			public const string Description = "desc";
			public const string Deadline = "deadline";
			public const string Guid = "guid";

			public const string Username = "username";
			public const string Password = "password";
			public const string PositionId = "_positionId";
			public const string Permissions = "_permissions";
			public const string Settings = "_settings";

			// permissions
			public const string PermissionsUsers = "_pUsers";
			public const string PermissionsUsersAdmin = "_pUsersAdmin";
            public const string PermissionsUsersAll = "_pUsersAll";
			public const string PermissionsOrders = "_pOrders";
			public const string PermissionsOrdersAdmin = "_pOrdersAdmin";
			public const string PermissionsInvoices = "_pInvoices";
			public const string PermissionsInvoicesAdmin = "_pInvoicesAdmin";
			public const string PermissionsTasks = "_pTasks";
			public const string PermissionsClients = "_pClients";
			public const string PermissionsClientsAll = "_pAllClients";

			public const string PermissionsAccounts = "_pAccounts";
			public const string PermissionsAccountsAdmin = "_pAccountsAdmin";

			public const string PermissionsReports = "_pReports";
		}

		public class Tables
		{
			public const string InvoicesTable = "tbl_invoices";
			public const string InvoiceItemsTable = "tbl_invoice_items";
			public const string ClientsTable = "tbl_clients";
			public const string OrdersTable = "tbl_orders";
			public const string UsersTable = "tbl_users";
		}
		#endregion
	}
}
