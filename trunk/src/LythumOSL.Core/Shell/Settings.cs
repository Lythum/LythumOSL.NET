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
	public class Settings : ILythumBase
	{
		#region Constants
		public const string DefaultSettingsName = "Settings";

		#endregion

		#region Properties
		public string SettingsName;

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
			SettingsName = settingsName;
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

			return retVal;
		}

		public static T Load<T> ()
			where T : Settings, new ()
		{
			return Load<T> (LythumOSL.Core.Helpers.ApplicationName);
		}


		public static void Save<T> (T obj)
			where T : Settings, new ()
		{
			Save<T> (obj, LythumOSL.Core.Helpers.ApplicationName);
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
