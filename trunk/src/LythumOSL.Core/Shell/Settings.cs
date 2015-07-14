using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.Core.Data.Xml;


namespace LythumOSL.Core.Shell
{
	/// <summary>
	/// Class for inheritance and implementation of any application settings universally
	/// very useful and fast to develop any settings
	/// </summary>
	public abstract class Settings : ILythumBase
	{
		#region Constants
		public const string DefaultSettingsName = "Settings";

		#endregion

		#region Properties
		protected string SettingsName { get; set; }
		protected string ApplicationName { get; set; }

		#endregion

		#region Construction

		/// <summary>
		/// Initialization with default name
		/// </summary>
		public Settings ()
			: this (DefaultSettingsName)
		{
		}

		public Settings (string settingsName)
		{
			this.SettingsName = settingsName;
		}

		#endregion

		#region Methods

		public static T Load<T> (string applicationName)
			where T : Settings, new ()
		{
			T retVal;

			RegistryAccess access = new RegistryAccess (applicationName);
			string settingsString = access.Get (DefaultSettingsName, string.Empty);

			if (string.IsNullOrEmpty (settingsString))
			{
				retVal = new T ();
				retVal.SettingsName = DefaultSettingsName;
			}
			else
			{
				retVal = Xml.Deserialize<T> (settingsString);
			}

			// assigning app name
			retVal.ApplicationName = applicationName;
			
			return retVal;
		}

		/// <summary>
		/// Loading settings using Application.ApplicationName 
		/// Application object should present and must be initialized
		/// 
		/// It works well with eg. WPF forms, in eg. constructors
		/// But in case of service and etc this object couldn't present
		/// Then better is to give App Name as parametter and to call it using method with it
		/// </summary>
		/// <returns></returns>
		public static T Load<T> ()
			where T : Settings, new ()
		{
			return Load<T> (LythumOSL.Core.Helpers.ApplicationName);
		}


		public static void Save<T> (T o)
			where T : Settings, new ()
		{
			if(!string.IsNullOrEmpty(o.ApplicationName))
			{
				// using AppName defined in settings object
				Save<T> (o, o.ApplicationName);
			}
			else
			{
				// getting app name from Application object
				// in this case don't execute this method from destructors
				// MainForm.Closing event override is best for it
				// Until this object is initialized
				Save<T> (o, LythumOSL.Core.Helpers.ApplicationName);
			}
		}

		public static void Save<T> (T obj, string applicationName)
		{
			RegistryAccess access = new RegistryAccess (applicationName);

			string serializedStr = Xml.Serialize (obj);

			access.Set (DefaultSettingsName, serializedStr);
		}

		#endregion
	}
}
