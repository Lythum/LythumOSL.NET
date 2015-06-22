using System;
using System.Data;
using System.Collections.Generic;

using LythumOSL.Core;
using LythumOSL.Core.Data;
using LythumOSL.Core.Metadata;
using LythumOSL.Indigo.Metadata;

namespace LythumOSL.Indigo.Data
{
	public class TableDesc : MetadataDbItem, ITableDesc
	{
		#region Const
		const string DefaultNameName = "name";
		const string DefaultRemovedName = "_removed";
		const string DefaultRemovedUserId = "_removedUserid";
		const string DefaultRemovedDateName = "_removedDate";
		const string DefaultNamePostfix = "_Name";

		#endregion

		#region Attributes

		IColumnDesc _PrimaryKeyColumn;
		IColumnDesc _NameColumn;

		#endregion

		#region Ctor

		public TableDesc ()
		{
			_PrimaryKeyColumn = null;
			_NameColumn = null;
			Columns = new NamedItemsCollection<IColumnDesc> ();
			Selectable = false;
		}

		/// <summary>
		/// Only for metadata purposes
		/// </summary>
		/// <param name="name"></param>
		public TableDesc (string name, IColumnDesc[] columns)
			: this (name, name, name, columns)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">table name</param>
		/// <param name="displayName"></param>
		/// <param name="selectable"></param>
		public TableDesc (string name, string alias, string displayName)
			: this ()
		{
			Validation.RequireValidString (name, "name");
			Validation.RequireValidString (alias, "alias");

			this.Name = name;
			this.Alias = alias;
			this.DisplayName = displayName;
		}

		public TableDesc (string name, string alias, string displayName, IColumnDesc[] columns)
			: this (name, alias, displayName)
		{
			Columns.AddRange (columns);
		}


		#endregion

		#region ITableDesc Members

		#region Properties

		public bool Selectable { get; set; }
		public NamedItemsCollection<IColumnDesc> Columns { get; protected set; }

		#region Implemented properties
		public IColumnDesc PrimaryKeyColumn
		{
			get
			{
				if (_PrimaryKeyColumn == null)
				{
					foreach (IColumnDesc cd in Columns)
					{
						if (cd.PrimaryKey)
						{
							_PrimaryKeyColumn = cd;
							break;
						}
					}
				}

				if (_PrimaryKeyColumn == null)
					throw new LythumException ("TableDesc: Primary Key column not found!");

				return _PrimaryKeyColumn;
			}
		}

		public IColumnDesc NameColumn
		{
			get
			{
				if (_NameColumn == null)
				{
					foreach (IColumnDesc cd in Columns)
					{
						if (cd.Name.Equals (DefaultNameName))
						{
							_NameColumn = cd;
							break;
						}
					}
				}

				if (_NameColumn == null)
					throw new LythumException ("TableDesc: Name column not found!");

				return _NameColumn;
			}
		}

		#endregion

		#endregion

		#region Methods


		public void Prepare ()
		{
			foreach (IColumnDesc cd in Columns)
			{
				cd.Prepare (this);
			}
		}

		public virtual string QuerySelect (IDatabaseInfo i)
		{
			string sql = i.TokenSelect + " ";
			List<string> values = new List<string> ();
			string joinsSql = string.Empty;

			foreach (IColumnDesc cd in Columns)
			{
				
				string fieldSelect = cd.FieldSelect (i);
				if (!string.IsNullOrEmpty (fieldSelect))
				{
					values.Add (fieldSelect);
				}

				if (cd is IColumnDictionaryDesc)
				{
					IColumnDictionaryDesc cdd = (IColumnDictionaryDesc)cd;

					// generate join
					joinsSql += " " + string.Format (
						(cd.Required ? i.JoinInner : i.JoinLeft),
						new string[] {
							// JOIN
							i.GetTableName(cdd.TableDesc.Name, cdd.TableDesc.Alias),
							// ON
							i.GetFieldName(Alias, cd.Name) + " = " + 
								i.GetFieldName(cdd.TableDesc.Alias, cdd.TableDesc.PrimaryKeyColumn.Name)
						});

					// generate join select value, display value
					values.Add (i.GetFieldName (
						cdd.TableDesc.Alias,
						PrimaryKeyColumn.Name,
						cdd.TableDesc.NameColumn.Name + DefaultNamePostfix));
				}
			}

			sql += string.Join( i.FieldSeparator, values.ToArray()) + " " + 
				i.TokenFrom + " " + i.GetTableName (Name, Alias);

			sql += joinsSql;

			sql += " " + i.TokenWhere + " " + i.GetFieldName (Alias, DefaultRemovedName) + " = '0'";

			sql += i.QueryTerminator;

			System.Diagnostics.Debug.Print ("SQL " + sql);

			return sql;
		}

		public virtual string QueryUpdate (IDatabaseInfo i, DataRow r)
		{
			string sql = i.TokenUpdate + " " + i.GetTableName (Name) + " SET ";
			List<string> values = new List<string> ();
			string whereCause = string.Empty;

			foreach (IColumnDesc cd in Columns)
			{
				if (cd.PrimaryKey)
				{
					whereCause += i.GetFieldName (Name, cd.Name) + " = '" + r[cd.Name].ToString () + "'";
				}
				else if (r[cd.Name] != DBNull.Value)
				{
					values.Add (i.GetFieldName (Name, cd.Name) + " = '" + Fix.FixString (cd.ValueToString (r[cd.Name])) + "'");
				}
				else if (r[cd.Name] == DBNull.Value && cd.Required)
				{
					values.Add (i.GetFieldName (Name, cd.Name) + " = " + i.ValueNull);
				}

			}

			sql += string.Join (i.FieldSeparator, values.ToArray ()) + " " + 
				i.TokenWhere + " " + whereCause + i.QueryTerminator;

			return sql;
		}

		public virtual string QueryInsert (IDatabaseInfo i, DataRow r)
		{
			string sql = i.TokenInsert + " " + i.GetTableName (Name);
			List<string> names = new List<string> ();
			List<string> values = new List<string> ();

			foreach (IColumnDesc cd in Columns)
			{
				if (cd.PrimaryKey)
				{
					// do nothing, we can't insert PK
				}
				else if (r[cd.Name] != DBNull.Value)
				{
					names.Add (i.GetFieldName (Name, cd.Name));
					values.Add (
						"'" + Fix.FixString (cd.ValueToString (r[cd.Name])) + "'");
				}
				else if (r[cd.Name] == DBNull.Value && cd.Required)
				{
					names.Add (cd.Name);
					values.Add (i.ValueNull);
				}

			}

			sql += " (" + string.Join (i.FieldSeparator, names.ToArray ()) + ") VALUES (" +
				string.Join (i.FieldSeparator, values.ToArray ()) + ")" + i.QueryTerminator;

			if (names.Count > 0)
			{
			}
			else
			{
				throw new LythumException ("No fields to save!");
			}

			return sql;
		}

		public virtual string QueryDelete (IDatabaseInfo i, DataRow r, string userId)
		{
			string sql = i.TokenUpdate + " " + i.GetTableName (Name);

			sql += " SET " + i.GetFieldName (Name, DefaultRemovedName) + " = 1, " +
				i.GetFieldName (Name, DefaultRemovedDateName) + " = " + i.FunctionNow + ", " +
				i.GetFieldName (Name, DefaultRemovedUserId) + " = " + userId;

			sql += " " + i.TokenWhere + i.GetFieldName (Name, PrimaryKeyColumn.Name) +
				" = '" + r[PrimaryKeyColumn.Name].ToString () + "'";

			return sql;


		}

		public virtual void QueryPostProcess (DataTable t)
		{
			foreach (IColumnDesc cd in Columns)
			{
				cd.QueryPostProcess (t);
			}
		}

		#endregion

		#endregion

		#region Helpers

		#endregion
	}
}
