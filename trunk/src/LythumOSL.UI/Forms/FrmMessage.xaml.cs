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

namespace LythumOSL.UI.Forms
{
	/// <summary>
	/// Interaction logic for FrmMessage.xaml
	/// </summary>
	public partial class FrmMessage : Window
	{
		readonly Brush ErrorColor = Brushes.Red;
		readonly Brush WarningColor = Brushes.MediumSeaGreen;
		readonly Brush QuestionColor = Brushes.CornflowerBlue;

		public enum MessageType
		{
			Warning,
			Question,
			Error
		}

		public FrmMessage (
			MessageType type,
			string msg,
			string title)
		{
			InitializeComponent ();

			switch (type)
			{
				case MessageType.Warning:

					TxtMsg.Foreground = WarningColor;
					ImgInfo.Source = new BitmapImage (new Uri ("/Lythum.UI;component/Images/Warning.png", UriKind.Relative));

					CmdCancel.Visibility = Visibility.Hidden;
					break;

				case MessageType.Question:

					TxtMsg.Foreground = QuestionColor;
					ImgInfo.Source = new BitmapImage (new Uri ("/Lythum.UI;component/Images/Question.png", UriKind.Relative));

					//CmdCancel.Visibility = Visibility.Hidden;
					break;

				case MessageType.Error:

					TxtMsg.Foreground = ErrorColor;
					ImgInfo.Source = new BitmapImage (new Uri ("/Lythum.UI;component/Images/Error.png", UriKind.Relative));

					CmdCancel.Visibility = Visibility.Hidden;
					break;
			}

			if (type != MessageType.Question)
			{
				/*CmdOk.Content = Manager.GetString ("OK");
				CmdOk.Left = (Width - CmdOk.Width) / 2;
				CancelButton = CmdOk;*/
			}

			TxtMsg.Text = msg;
			this.Title = title;


		}



		public static bool Start (
			MessageType type,
			string msg,
			string title)
		{
			FrmMessage f = new FrmMessage (type, msg, title);
			Nullable<bool> retVal = f.ShowDialog ();

			if (!retVal.HasValue)
			{
				return false;
			}
			else
			{
				return (bool)retVal;
			}
		}

		private void CmdCopy_Click (object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetData (DataFormats.Text, TxtMsg.Text);
			}
			catch
			{
			}
		}

		private void CmdOk_Click (object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close ();

		}

	}
}
