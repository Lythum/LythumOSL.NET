using System;
using System.Collections.Generic;
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

using LythumOSL.Core.Metadata;
using LythumOSL.UI;

namespace LythumOSL.UI.Forms
{
	/// <summary>
	/// Interaction logic for FrmLogin.xaml
	/// </summary>
	public partial class FrmLogin : Window
	{
		ILoginInfo _Info;

		public enum MessageType
		{
			Warning,
			Question,
			Error
		}

		public FrmLogin (ILoginInfo info, string title)
		{
			// assign info
			_Info = info;
			_Info.Success = false;

			InitializeComponent ();

			// assign titles
			this.LblTitle.Content = this.Title = title;

			TxtLogin.Text = _Info.Username;
			TxtPassword.Password = _Info.Password;
	
			PgrInfo.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			PgrInfo.SelectedObject = info.ConnectionInfo;

			TxtLogin.SelectAll ();
			TxtLogin.Focus ();
		}

		private void Any_MouseDown (object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove ();
			}
		}

		private void CmdOk_Click (object sender, RoutedEventArgs e)
		{
			_Info.Username = TxtLogin.Text;
			_Info.Password = TxtPassword.Password;
			_Info.Success = true;

			this.DialogResult = true;
			this.Close ();
		}
	}
}
