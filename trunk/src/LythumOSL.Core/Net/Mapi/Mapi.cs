using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using LythumOSL.Core;
using LythumOSL.Core.Properties;

namespace LythumOSL.Core.Net.Mapi
{
	public class Mapi : IDisposable
	{
		#region Constants / Enums
		const int MAPI_LOGON_UI = 0x00000001;
		const int MAPI_DIALOG = 0x00000008;
		const int maxAttachments = 20;

		enum HowTo { MAPI_ORIG = 0, MAPI_TO, MAPI_CC, MAPI_BCC };

		#endregion

		#region Attributes
		List<MapiRecipDesc> _Recipients = new
			List<MapiRecipDesc> ();
		List<string> _Attachments = new List<string> ();
		int _LastError = 0;

		/// <summary>
		/// Async attributes uses only SendMailPopupAsync stuff
		/// 
		/// If will be used in other places 
		/// rename it to just _Subject and etc
		/// </summary>
		string _AsyncSubject = string.Empty;
		string _AsyncBody = string.Empty;
		int _AsyncRetVal = 0;

		#endregion


		#region Externs
		[DllImport ("MAPI32.DLL")]
		public static extern int MAPISendMail (IntPtr sess, IntPtr hwnd,
			MapiMessage message, int flg, int rsv);

		#endregion

		#region Methods

		public bool AddRecipientTo (string email)
		{
			return AddRecipient (
				email, HowTo.MAPI_TO);
		}

		public bool AddRecipientCC (string email)
		{
			return AddRecipient (
				email, HowTo.MAPI_TO);
		}

		public bool AddRecipientBCC (string email)
		{
			return AddRecipient (
				email, HowTo.MAPI_TO);
		}

		public void AddAttachment (string attachmentFileName)
		{
			_Attachments.Add (
				attachmentFileName);
		}

		public int SendMailPopup (string subject, string body)
		{
			return SendMail (
				subject, body, MAPI_LOGON_UI | MAPI_DIALOG);
		}

		void SendAsync ()
		{
			try
			{
				_AsyncRetVal = SendMailPopup (_AsyncSubject, _AsyncBody);

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print ("SendAsync *ERROR* " + ex.Message);
			}
			finally
			{
			}
		}

		public int SendMailPopupAsync (string subject, string body)
		{
			_AsyncSubject = subject;
			_AsyncBody = body;

			Thread asyncSend = new Thread (SendAsync);
			asyncSend.Start ();

			return _AsyncRetVal;
		}

		public int SendMailDirect (string subject, string body)
		{
			return SendMail (
				subject, body, MAPI_LOGON_UI);
		}

		public string GetLastError ()
		{
			if (_LastError <= 26)
				return errors[_LastError];
			return "MAPI error [" + _LastError.ToString () + "]";
		}

		#endregion

		#region Helpers

		int SendMail (string subject, string body, int how)
		{
			MapiMessage msg = new MapiMessage ();
			msg.subject = subject;
			msg.noteText = body;

			msg.recips = GetRecipients (out msg.recipCount);
			msg.files = GetAttachments (out msg.fileCount);

			_LastError = MAPISendMail (
				new IntPtr (0), new IntPtr (0), msg, how, 0);

			if (_LastError > 1)
			{
				throw new LythumException ("MAPISendMail failed! " + GetLastError ());
			}

			Cleanup (ref msg);
			return _LastError;
		}

		bool AddRecipient (string email, HowTo howTo)
		{
			MapiRecipDesc recipient = new MapiRecipDesc ();

			recipient.recipClass = (int)howTo;
			recipient.name = email;
			_Recipients.Add (recipient);

			return true;
		}

		IntPtr GetRecipients (out int recipCount)
		{
			recipCount = 0;
			if (_Recipients.Count == 0)
				return IntPtr.Zero;

			int size = Marshal.SizeOf (typeof (MapiRecipDesc));
			IntPtr intPtr = Marshal.AllocHGlobal (_Recipients.Count * size);

			int ptr = (int)intPtr;
			foreach (MapiRecipDesc mapiDesc in _Recipients)
			{
				Marshal.StructureToPtr (mapiDesc, (IntPtr)ptr, false);
				ptr += size;
			}

			recipCount = _Recipients.Count;
			return intPtr;
		}

		IntPtr GetAttachments (out int fileCount)
		{
			fileCount = 0;
			if (_Attachments == null)
				return IntPtr.Zero;

			if ((_Attachments.Count <= 0) || (_Attachments.Count >
				maxAttachments))
				return IntPtr.Zero;

			int size = Marshal.SizeOf (typeof (MapiFileDesc));
			IntPtr intPtr = Marshal.AllocHGlobal (_Attachments.Count * size);

			MapiFileDesc mapiFileDesc = new MapiFileDesc ();
			mapiFileDesc.position = -1;
			int ptr = (int)intPtr;

			foreach (string strAttachment in _Attachments)
			{
				mapiFileDesc.name = Path.GetFileName (strAttachment);
				mapiFileDesc.path = strAttachment;
				Marshal.StructureToPtr (mapiFileDesc, (IntPtr)ptr, false);
				ptr += size;
			}

			fileCount = _Attachments.Count;
			return intPtr;
		}

		void Cleanup (ref MapiMessage msg)
		{
			int size = Marshal.SizeOf (typeof (MapiRecipDesc));
			int ptr = 0;

			if (msg.recips != IntPtr.Zero)
			{
				ptr = (int)msg.recips;
				for (int i = 0; i < msg.recipCount; i++)
				{
					Marshal.DestroyStructure ((IntPtr)ptr,
						typeof (MapiRecipDesc));
					ptr += size;
				}
				Marshal.FreeHGlobal (msg.recips);
			}

			if (msg.files != IntPtr.Zero)
			{
				size = Marshal.SizeOf (typeof (MapiFileDesc));

				ptr = (int)msg.files;
				for (int i = 0; i < msg.fileCount; i++)
				{
					Marshal.DestroyStructure ((IntPtr)ptr,
						typeof (MapiFileDesc));
					ptr += size;
				}
				Marshal.FreeHGlobal (msg.files);
			}

			_Recipients.Clear ();
			_Attachments.Clear ();
			_LastError = 0;
		}

		#endregion

		readonly string[] errors = new string[] {
			Lythum.Core.Properties.Resources.MAPI_MSG_00,
			Lythum.Core.Properties.Resources.MAPI_MSG_01,
			Lythum.Core.Properties.Resources.MAPI_MSG_02,
			Lythum.Core.Properties.Resources.MAPI_MSG_03,
			Lythum.Core.Properties.Resources.MAPI_MSG_04,
			Lythum.Core.Properties.Resources.MAPI_MSG_05,
			Lythum.Core.Properties.Resources.MAPI_MSG_06,
			Lythum.Core.Properties.Resources.MAPI_MSG_07,
			Lythum.Core.Properties.Resources.MAPI_MSG_08,
			Lythum.Core.Properties.Resources.MAPI_MSG_09,
			Lythum.Core.Properties.Resources.MAPI_MSG_10,
			Lythum.Core.Properties.Resources.MAPI_MSG_11,
			Lythum.Core.Properties.Resources.MAPI_MSG_12,
			Lythum.Core.Properties.Resources.MAPI_MSG_13,
			Lythum.Core.Properties.Resources.MAPI_MSG_14,
			Lythum.Core.Properties.Resources.MAPI_MSG_15,
			Lythum.Core.Properties.Resources.MAPI_MSG_16,
			Lythum.Core.Properties.Resources.MAPI_MSG_17,
			Lythum.Core.Properties.Resources.MAPI_MSG_18,
			Lythum.Core.Properties.Resources.MAPI_MSG_19,
			Lythum.Core.Properties.Resources.MAPI_MSG_20,
			Lythum.Core.Properties.Resources.MAPI_MSG_21,
			Lythum.Core.Properties.Resources.MAPI_MSG_22,
			Lythum.Core.Properties.Resources.MAPI_MSG_23,
			Lythum.Core.Properties.Resources.MAPI_MSG_24,
			Lythum.Core.Properties.Resources.MAPI_MSG_25,
			Lythum.Core.Properties.Resources.MAPI_MSG_26,
        };

		#region IDisposable Members

		public void Dispose ()
		{
		}

		#endregion
	}

}
