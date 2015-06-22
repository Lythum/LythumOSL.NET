using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data
{
	/// <summary>
	/// DataTable which supports caching
	/// </summary>
	public class LythumDataTableCache : ILythumBase
	{
		#region Delegates
		/// <summary>
		/// Delegate query function
		/// </summary>
		/// <returns>SQL result</returns>
		public delegate DataTable SqlQuery(string sql);
		public delegate DataTable SqlQueryVoid ();

		#endregion

		#region Attributes
		public SqlQuery Query { get; set; }
		public SqlQueryVoid QueryVoid { get; set; }
		public string Sql { get; set; }
		public TimeSpan Delay { get; set; }
		public DateTime LastRefresh { get; protected set; }
		
		// data
		DataTable _Table = null;

		#endregion

		#region Properties
		public DataTable Table
		{
			get
			{
				if (IsTableNeedRefresh)
				{
					_Table = RefreshTable ();
				}

				return _Table;
			}
			set
			{
				// Set mode only reset cache
				if (value == null)
				{
					_Table = value;
				}
			}
		}

		public bool IsTableNeedRefresh
		{
			get
			{
				if (_Table == null || DateTime.Now > (LastRefresh + Delay))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		#endregion

		#region Init

		public LythumDataTableCache (
			SqlQuery queryFunction,
			string sql,
			TimeSpan delay)
		{
			Validation.RequireValid (queryFunction, "queryFunction");
			Validation.RequireValidString (sql, "sql");
			Validation.RequireValid (delay, "delay");

			Query = queryFunction;
			QueryVoid = null;
			Sql = sql;
			Delay = delay;
		}

		public LythumDataTableCache (
			SqlQuery queryFunction,
			string sql,
			int hours,
			int minutes,
			int seconds)
			: this (queryFunction, sql, new TimeSpan (hours, minutes, seconds))
		{
		}

		public LythumDataTableCache (
			SqlQueryVoid queryFunction,
			TimeSpan delay)
		{
			Validation.RequireValid (queryFunction, "queryFunction");
			Validation.RequireValid (delay, "delay");

			QueryVoid = queryFunction;
			Query = null;
			Delay = delay;
		}

		#endregion

		#region Methods

		DataTable RefreshTable ()
		{
			LastRefresh = DateTime.Now;

			if (QueryVoid != null)
			{
				return QueryVoid ();
			}
			else if (Query != null && !string.IsNullOrEmpty (Sql))
			{
				return Query (Sql);
			}
			else
			{
				throw new LythumException ("Query function or sql query is not specified!");
			}
		}


		#endregion
	}
}
