using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Data.Common;

using LythumOSL.Core.Enums;
using LythumOSL.Core.Data.Info;
using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	public class Sql : ErrorInfo, IDataAccess
	{
		#region Constants
		/// <summary>
		/// [Const] Default timeout in seconds
		/// </summary>
		public const int DefaultTimeout = 30;

		#endregion

		#region Attributes
		// System.Data
		protected DbConnection _Connection;
		DbTransaction _Transaction;

		string _LastSql;
		/// <summary>
		/// Very important internal attribute
		/// 
		/// if it true then on any error is thrown exception
		/// </summary>
		bool _ThrowException = false;

		SqlHelpers _Helpers;

		// local
		int _CommandTimeout;
		bool _CloseConnection = true;

		// info
		DatabasesInfo _Info = null;


		#endregion

		#region Properties
		public string LastSql
		{
			get { return _LastSql; }
			set { _LastSql = value; }
		}

		public DbTransaction Transaction
		{
			get { return _Transaction; }
			set { _Transaction = value; }
		}

		/// <summary>
		/// Executing command timeout in seconds
		/// </summary>
		public int CommandTimeout
		{
			get
			{
				return _CommandTimeout;
			}
			set
			{
				if(value>0)
					_CommandTimeout = value;
			}
		}

		/// <summary>
		/// IHasConnection implementation too
		/// </summary>
		public virtual IDbConnection Connection
		{
			get { return _Connection; }
		}

		public bool IsTransactionInProgress
		{
			get { return _Transaction != null; }
		}

		public SqlHelpers Helpers
		{
			get { return _Helpers; }
		}

		#endregion

		#region Construction
		/// <summary>
		/// Constructor
		/// 
		/// Require valid ADO.NET connection initialized with connection string
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="throwException">
		/// If it set to true, then on error exception will be thrown
		/// </param>
		public Sql (
			DbConnection connection, 
			bool throwException, 
			bool closeConnection)
		{
			try
			{
				Validation.RequireValid (connection, "connection");
				Validation.RequireValidString (connection.ConnectionString, "connection.ConnectionString");

				_Connection = connection;
				_ThrowException = throwException;
				_CloseConnection = closeConnection;
/*
				Debug.Print (
					">>> Connection type: " + connection.GetType ().ToString () +
					", Connection string: '" + connection.ConnectionString + "'");
*/
				Init ();
			}
			catch (Exception ex)
			{
				Error (ex);
			}
		}

		/// <summary>
		/// Initialize connection by supported providers name and connection string
		/// </summary>
		/// <param name="providerInvariantName"></param>
		/// <param name="connectionString"></param>
		/// <param name="throwException">
		/// If it set to true, then on error exception will be thrown
		/// </param>
		public Sql (
			string providerInvariantName, 
			string connectionString,
			bool throwException,
			bool closeConnection)
		{
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory (providerInvariantName);
				_Connection = factory.CreateConnection ();
				_Connection.ConnectionString = connectionString;
				_ThrowException = throwException;
				_CloseConnection = closeConnection;

				Init ();
			}
			catch (Exception ex)
			{
				Error (ex);
			}
		}

		/// <summary>
		/// Initialize connection by supported provider DataRow (see GetProvidersFactories function)
		/// and given connection string
		/// </summary>
		/// <param name="providerRow"></param>
		/// <param name="connectionString"></param>
		/// <param name="throwException">
		/// If it set to true, then on error exception will be thrown
		/// </param>
		public Sql (
			DataRow providerRow, 
			string connectionString,
			bool throwException,
			bool closeConnection)
		{
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory (providerRow);
				_Connection = factory.CreateConnection ();
				_Connection.ConnectionString = connectionString;
				_ThrowException = throwException;
				_CloseConnection = closeConnection;

				Init ();
			}
			catch (Exception ex)
			{
				Error (ex);
			}
		}

		/// <summary>
		/// All what initialized independed from constructor go here
		/// </summary>
		protected virtual void Init ()
		{
			_Transaction = null;
			_CommandTimeout = DefaultTimeout;

			// Lets initialize helpers
			_Helpers = new SqlHelpers (this);
		}

		#endregion

		#region Methods

		#region Queries

		#region QueryBulk
		public void QueryBulk (
			string pattern,
			DataTable table,
			string[] values)
		{
			string sql = string.Empty;

			try
			{
				// prepare simple values
				List<string> cleanValues = new List<string> ();

				foreach (string value in values)
				{
					if (value[0].Equals ('@'))
					{
						cleanValues.Add (value);
					}
					else
					{
						cleanValues.Add (Fix.FixString (value));
					}
				}

				this.Connect (true, true);

				foreach (DataRow row in table.Rows)
				{
					List<object> parameters = new List<object> ();

					foreach (string value in cleanValues)
					{
						if (value[0].Equals ('@'))
						{
							object rawRowValue = row[value.Substring (1)];

							if (rawRowValue is decimal || 
								rawRowValue is float ||
								rawRowValue is double)
							{
								parameters.Add (rawRowValue.ToString ().Replace (",", "."));
							}
							else if (rawRowValue is Boolean)
							{
								parameters.Add (Fix.FixBool (Convert.ToBoolean (rawRowValue)));
							}
							else
							{
								parameters.Add (Fix.FixString (rawRowValue.ToString ()));
							}

						}
						else
						{
							// just value
							parameters.Add (value);
						}
					}

					sql = string.Format (
						pattern,
						parameters.ToArray ());

					this.Execute (sql);
				}

				// no errors, commit
				this.Close (true);
			}
			catch (Exception ex)
			{
				// errors found, rolling back
				this.TransactionStop (false);
				this.Close ();
				throw new LythumException (
					"Sql::QueryBulk [" + sql + "] error [" + ex.Message + "]",
					ex);
			}
		}

		#endregion

		#region QueryDataSet
		public DataSet QueryDataSet (string sql)
		{
			return QueryDataSet (sql, true);
		}
		/// <summary>
		/// Function returns few select results 
		/// from for example stored procedure
		/// as DataSet tables
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="closeConnection"></param>
		/// <returns></returns>
		public DataSet QueryDataSet (string sql, bool closeConnection)
		{
			Debug.Print ("Sql::Query: " + sql);
			Validation.RequireValidString (sql, "sql");

			Error ();

			_LastSql = sql;

			DataSet retVal = new DataSet ("D");

			IDbCommand cmd = CreateCommand (true);

			if (cmd != null)
			{
				try
				{
					cmd.CommandText = sql;

					IDataReader reader = cmd.ExecuteReader ();

					int tableNumber = 1;

					do
					{
						LythumDataTable t = new LythumDataTable (
							"T" + tableNumber.ToString ());
						t.Load (reader);

						retVal.Tables.Add (t);

						tableNumber++;

					}
					while (!reader.IsClosed);
				}
				catch (Exception ex)
				{
					Error (ex);

					// throw exception if it enabled
					if (_ThrowException)
						throw new LythumException ("Lythum.Core.Data.Sql.Query [ " + _LastSql + " ]", ex);
				}
				finally
				{
					cmd.Dispose ();

					if (closeConnection && !IsTransactionInProgress)
					{
						Close ();
					}
				}
			}
			return retVal;
		}

		#endregion

		#region Query
		public DataTable Query (string sql)
		{
			return Query (sql, true);
		}

		public DataTable Query (string sql, bool closeConnection)
		{
			Debug.Print ("Sql::Query: " + sql);
			Validation.RequireValidString (sql, "sql");

			Error ();

			_LastSql = sql;

			DataTable retVal = null;

			IDbCommand cmd = CreateCommand (true);

			if (cmd != null)
			{
				try
				{
					cmd.CommandText = sql;

					IDataReader reader = cmd.ExecuteReader ();

					retVal = new DataTable ("R");
					retVal.Load (reader);
				}
				catch (Exception ex)
				{
					Error (ex);

					// throw exception if it enabled
					if (_ThrowException)
						throw new LythumException ("Lythum.Core.Data.Sql.Query [ " + _LastSql + " ]", ex);
				}
				finally
				{
					cmd.Dispose ();

					if (closeConnection && !IsTransactionInProgress)
					{
						Close ();
					}
				}
			}
			return retVal;
		}

		/// <summary>
		/// Retrieves data from SQL then make they mapping
		/// and returns dictionary
		/// </summary>
		/// <param name="sql">Sql query</param>
		/// <param name="key">Field name which is key</param>
		/// <param name="value">Field name which is value</param>
		/// <returns></returns>
		public LythumDictionary<string, string> Query (string sql, string key, string value)
		{
			LythumDictionary<string, string> retVal = new LythumDictionary<string, string> ();

			DataTable table = Query (sql);

			if (table != null)
			{

				foreach (DataRow r in table.Rows)
				{
					retVal.Add (
						r[key].ToString (),
						r[value].ToString ());
				}

			}

			return retVal;
		}

		#endregion

		#region QueryScalaar

		public string QueryScalar (string sql)
		{
			return QueryScalar (sql, true);
		}

		public string QueryScalar (string sql, bool closeConnection)
		{
			DataTable table = Query (sql, closeConnection);

			if (table != null)
			{
				if (table.Rows.Count > 0 && table.Columns.Count > 0)
				{
					return table.Rows[0].ItemArray[0].ToString ();
				}
			}

			return string.Empty;
		}

		#endregion

		#region Execute

		public int Execute (string sql)
		{
			return Execute (sql, true);
		}

		public int Execute (string sql, bool closeConnection)
		{
			Debug.Print ("ExecSQL: " + sql);

			Validation.RequireValidString (sql, "sql");

			int retVal = -1;
			_LastSql = sql;
			IDbCommand cmd = CreateCommand (true);

			if (cmd != null)
			{
				try
				{
					cmd.CommandText = sql;
					retVal = cmd.ExecuteNonQuery ();
				}
				catch (Exception ex)
				{
					Error (ex);

					// throw exception if it enabled
					if (_ThrowException)
						throw new LythumException ("Lythum.Core.Data.Sql.Query [ " + _LastSql + " ]", ex);
				}
				finally
				{
					cmd.Dispose ();

					// close if set to close and if transaction not in progress
					if (closeConnection && !IsTransactionInProgress)
					{
						Close ();
					}
				}

			}

			return retVal;
		}

		#endregion

		#region depreaced or unused
		/*
		public bool InsertBulk<A> (
			DataTable table,
			string sql,
			DbParameter[] parameters,
			bool closeConnection)

			where A : DbDataAdapter, new ()
		{
			Debug.Print ("Sql::Update: " + sql);
			Validation.RequireValidString (sql, "sql");

			Error ();

			_LastSql = sql;

			// creating command and connecting to dbs
			DbCommand cmd = CreateCommand (true);

			if (!IsError)
			{
				try
				{
					// assign sql query
					cmd.CommandText = sql;

					// creating adapter
					A a = new A();
					// assigning command
					a.InsertCommand = cmd;
					a.InsertCommand.Parameters.AddRange (parameters);

					// do it
					a.Update (table);


				}
				catch (Exception ex)
				{
					Error (ex);

					// throw exception if it enabled
					if (_ThrowException)
						throw new AlphaException (ex);
				}
				finally
				{
					if (cmd != null)
					{
						cmd.Dispose ();
					}

					if (closeConnection)
					{
						Close ();
					}
				}
			}
			return IsError;
		}

		*/
		#endregion // depreaced

		#endregion

		#region Transactions

		public void TransactionStart (bool commitLastTransaction)
		{
			TransactionStop (commitLastTransaction);

			_Transaction = _Connection.BeginTransaction ();
		}

		public void TransactionStart (bool commitLastTransaction, IsolationLevel isolationLevel)
		{
			TransactionStop (commitLastTransaction);

			_Transaction = _Connection.BeginTransaction (isolationLevel);
		}

		public void TransactionStop (bool commitLastTransaction)
		{
			if (IsTransactionInProgress)
			{
				if (commitLastTransaction)
				{
					_Transaction.Commit ();
				}
				else
				{
					_Transaction.Rollback ();
				}

				_Transaction.Dispose ();
				_Transaction = null;
			}
		}

		#endregion

		#endregion

		#region Static methods
		/// <summary>
		///     Returns a System.Data.DataTable that contains information about all installed
		///     providers that implement System.Data.Common.DbProviderFactory.
		/// </summary>
		/// <returns>
		///     Returns a System.Data.DataTable containing System.Data.DataRow objects that
		///     contain the following data. Column ordinalColumn nameDescription0NameHuman-readable
		///     name for the data provider.1DescriptionHuman-readable description of the
		///     data provider.2InvariantNameName that can be used programmatically to refer
		///     to the data provider.3AssemblyQualifiedNameFully qualified name of the factory
		///     class, which contains enough information to instantiate the object.
		/// </returns>
		public static DataTable GetProvidersFactories ()
		{
			return DbProviderFactories.GetFactoryClasses ();
		}

		/// <summary>
		/// Uzdaro connectiona bet kuriuo atveju
		/// </summary>
		/// <param name="cn"></param>
		public static void Close (IDbConnection cn)
		{
			if (cn.State != ConnectionState.Closed)
			{
				cn.Close ();
			}
		}

		#endregion

		#region Helpers
		/// <summary>
		/// This function will create command for database access
		/// </summary>
		/// <returns></returns>
		public DbCommand CreateCommand (bool connect)
		{
			DbCommand cmd = null;

			try
			{
				if (Connect ())
				{

					cmd = _Connection.CreateCommand ();

					cmd.CommandType = CommandType.Text;

					cmd.CommandTimeout = _CommandTimeout;

					if (IsTransactionInProgress)
					{
						cmd.Transaction = _Transaction;
					}

				}
			}
			catch (Exception ex)
			{
				Error (ex);

				// throw exception if it enabled
				if (_ThrowException)
					throw new LythumException (ex);
			}

			return cmd;
		}

		public void Close ()
		{
			if (_CloseConnection)
			{
				Close (_Connection);
			}
		}

		public void Close (bool commitLastTransaction)
		{
			TransactionStop (commitLastTransaction);

			Close ();
		}

		public bool Connect (
			bool beginTransaction, 
			bool commitLastTransaction)
		{
			bool retVal = Connect ();

			if (retVal && beginTransaction)
			{
				TransactionStart (commitLastTransaction);
			}

			return retVal;
		}

		public bool Connect ()
		{
			try
			{
				if (_Connection.State == ConnectionState.Closed)
				{
					_Connection.Open ();
				}

				return true;
			}
			catch (Exception ex)
			{
				Error (ex);

				// throw exception if it enabled
				if (_ThrowException)
				{
					throw ex;
				}
				else
				{
					return false;
				}
			}
		}

		#endregion


		#region IDataAccess Members

		public DatabaseTypes DatabaseType
		{
			get
			{
#warning todo: implement DatabaseType detection
				DatabaseTypes retVal = DatabaseTypes.Unknown;

				return retVal;
			}
		}

		public DatabasesInfo Info
		{
			get
			{
				if (_Info == null)
				{
					_Info = new DatabasesInfo (this.DatabaseType);
				}

				return _Info;

			}
		}

		#endregion
	}
}
