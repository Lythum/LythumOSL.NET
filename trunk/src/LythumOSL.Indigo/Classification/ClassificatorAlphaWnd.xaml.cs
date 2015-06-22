using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Windows.Controls;

using LythumOSL.Core;
using LythumOSL.Core.Data.Info;
using LythumOSL.Core.Metadata;
using LythumOSL.Indigo.Enums;
using LythumOSL.Indigo.Metadata;
using LythumOSL.UI;

namespace LythumOSL.Indigo.Classification
{
	/// <summary>
	/// Interaction logic for ClassificatorAlphaWnd.xaml
	/// </summary>
	public partial class ClassificatorAlphaWnd : Window
	{
		#region Attributes
		IClassificatorManager _Manager;
		IClassificatorCredentials _Credentials;
		IDataAccess _Access;

		DataRow SelectedRow { get; set; }
		DataTable _Table;

		#endregion

		#region Ctor

		public ClassificatorAlphaWnd (
			IDataAccess access, 
			ITableDesc desc, 
			IClassificatorCredentials credentials)
		{
			InitializeComponent ();

			_Access = new VisualDataAccess (this, access);
			_Manager = new ClassificatorManager (_Access, desc, credentials);
			_Credentials = credentials;

			SelectedRow = null;

			InitUI (desc);

			DgrMain.CanUserDeleteRows = false;
		}

		#endregion

		#region Static

		public static DataRow Start (IDataAccess access, ITableDesc desc, IClassificatorCredentials credentials)
		{
			try
			{
				ClassificatorAlphaWnd f =
					new ClassificatorAlphaWnd (access, desc, credentials);

				if (f.ShowDialog () == true)
				{
					return f.SelectedRow;
				}
			}
			catch (Exception ex)
			{
				Messages.Error (ex.Message + "\r\n" + ex.StackTrace);
			}

			return null;
		}
		#endregion

		#region Helpers

		void InitUI (ITableDesc desc)
		{
			Title = desc.DisplayName;

			if (_Credentials.CanSelect)
			{
				CmdSelect.IsDefault = true;
			}
			else
			{
				CmdOk.IsDefault = true;
				CmdSelect.Visibility = Visibility.Hidden;
			}

			DgrMain.CanUserDeleteRows = false;
			DgrMain.CanUserAddRows = _Credentials.CanCreate;
			DgrMain.IsReadOnly = !_Credentials.CanModify;

			_Manager.InitUI (DgrMain);
		}

		void Save ()
		{
			if (_Manager.Save ())
			{
				Search ();
			}
		}

		bool Delete ()
		{
			if (DgrMain.SelectedItems.Count < 1)
				return false;

			List<DataRow> rows = new List<DataRow> ();
			foreach (object o in DgrMain.SelectedItems)
			{
				if (o is DataRowView)
				{
					rows.Add (((DataRowView)o).Row);
				}
			}

			if (rows.Count < 1)
				return false;

			if (Messages.Question (Properties.Resources.CONFIRM_DELETE_ITEMS))
			{
				_Manager.Delete (rows.ToArray ());
				return true;
			}

			return false;

		}

		void Search ()
		{
			_Table = _Manager.Search ();
			_Manager.PopulateResult (DgrMain, _Table);
		}

		#endregion

		// EVENTS

		private void CmdSearch_Click (object sender, RoutedEventArgs e)
		{
			Search ();
		}

		private void CmdOk_Click (object sender, RoutedEventArgs e)
		{
			Save ();
		}

		private void DgrMain_KeyUp (object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Delete:
					if (_Credentials.CanDelete)
					{
						if (Delete ())
						{
							Search ();
						}
					}
					break;

				default:
					break;
			}
		}

		private void CmdCancel_Click (object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close ();
		}

	}
}
