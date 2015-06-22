using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.Core.Enums;
using LythumOSL.Core.Data.Info;

namespace LythumOSL.UI
{
	/// <summary>
	/// UIDataAccessWrapper it's a simple IDataAccess implementation
	/// which just add FrameworkElement's cursor control, to show hourglass cursor when executing sql queries
	/// </summary>
	public class VisualDataAccess : IDataAccess
	{

		IDataAccess _Access;
		FrameworkElement _Element;

		public FrameworkElement Element
		{
			get { return _Element; }
			set { _Element = value; }
		}

		public VisualDataAccess (FrameworkElement element, IDataAccess access)
		{

			Validation.RequireValid (access, "access");
			//Validation.RequireValid (element, "element");

			_Access = access;
			_Element = element;
		}

		#region IDataAccess Members

		public DataTable Query (string sql)
		{
			DataTable retVal = null;

			try
			{
				SetWaitCursor();
				retVal = _Access.Query (sql);
				SetNormalCursor ();
			}
			catch (Exception ex)
			{
				throw new LythumException (ex);
			}
			finally
			{
				SetNormalCursor ();
			}

			return retVal;
		}

		public DataSet QueryDataSet (string sql)
		{
			DataSet retVal = null;

			try
			{
				SetWaitCursor ();
				retVal = _Access.QueryDataSet (sql);
				SetNormalCursor ();
			}
			catch (Exception ex)
			{
				throw new LythumException (ex);
			}
			finally
			{
				SetNormalCursor ();
			}

			return retVal;
		}

		public string QueryScalar (string sql)
		{
			string retVal = string.Empty;

			try
			{
				SetWaitCursor ();
				retVal = _Access.QueryScalar (sql);
				SetNormalCursor ();
			}
			catch (Exception ex)
			{
				throw new LythumException (ex);
			}
			finally
			{
				SetNormalCursor ();
			}

			return retVal;
		}

		public int Execute (string sql)
		{
			int retVal = -1;

			try
			{
				SetWaitCursor ();
				retVal = _Access.Execute (sql);
				SetNormalCursor ();
			}
			catch (Exception ex)
			{
				throw new LythumException (ex);
			}
			finally
			{
				SetNormalCursor ();
			}

			return retVal;
		}

		public DatabaseTypes DatabaseType
		{
			get
			{
				return _Access.DatabaseType;
			}
		}

		public DatabasesInfo Info
		{
			get
			{
				return _Access.Info;
			}
		}

		#endregion

		#region Helpers
		public void SetWaitCursor ()
		{
			if (_Element != null)
			{
				_Element.Cursor = Cursors.Wait;
			}
		}

		public void SetNormalCursor ()
		{
			if (_Element != null)
			{
				_Element.Cursor = Cursors.Arrow;
			}
		}

		#endregion
	}
}
