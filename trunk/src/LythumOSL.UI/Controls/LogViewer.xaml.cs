/*
 * Created by SharpDevelop.
 * User: Agira
 * Date: 07/14/2015
 * Time: 01:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LythumOSL.UI.Controls
{
	/// <summary>
	/// This class is not finished and not used anywhere yet
	/// </summary>
	public partial class LogViewer : UserControl
	{
			
		public class LogEntry
		{
			#warning INFO: Unused class
			public int LogLevel { get; set; }
			public DateTime Date { get; set; }
			public string Message { get; set; }
		}

		
		ObservableCollection<LogEntry> _Log;
		
		public LogViewer()
		{
			InitializeComponent();
			
			_Log = new ObservableCollection<LogEntry>();
			DgrMain.ItemsSource = _Log;
		}
		
		public void Message (int logLevel, string msg)
		{
			_Log.Add(new LogEntry() 
			{ 
			    LogLevel = logLevel,
			    Message = msg,
			    Date = DateTime.Now,			
			});
		}
	}
}