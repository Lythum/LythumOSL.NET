using System;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.UI.Forms;

namespace LythumOSL.UI
{
	public class Messages
	{
		public static void Warning (string text, string title)
		{
			FrmMessage.Start (
				FrmMessage.MessageType.Warning,
				text,
				title);
		}

		public static void Warning (string text)
		{
			Warning (text, Properties.Resources.LabelWarning);
		}

		public static void Error (string text, string title)
		{
			FrmMessage.Start (
				FrmMessage.MessageType.Error,
				text,
				title);
		}

		public static void Error (string text)
		{
			Error (text, Properties.Resources.LabelError);
		}

		public static void Error (Exception ex)
		{
			Error (
				ex.Message + "\r\n\r\n" + ex.StackTrace, 
				Properties.Resources.LabelError);
		}

		public static void Error (ErrorInfo info, string title)
		{
			Error (info.ErrorText, title);
		}

		public static void Error(ErrorInfo info)
		{
			 Error (info, Properties.Resources.LabelError);
		}

		public static bool Question (string text, string title)
		{

			return FrmMessage.Start (
				FrmMessage.MessageType.Question,
				text,
				title);
		}

		public static bool Question (string text)
		{
			return Question (text, Properties.Resources.LabelQuestion);
		}

		#region Login
		/// <summary>
		/// Login window
		/// </summary>
		/// <param name="info">
		/// Suggest to use for this Lyhthum.Data.DataTypes.LoginInfo
		/// </param>
		/// <returns>True if user in login form pressed ok</returns>
		public static bool Login (ILoginInfo info, string title)
		{
			FrmLogin login = new FrmLogin (info, title);
			login.ShowDialog ();

			return info.Success;
		}

		#endregion
	}
}
