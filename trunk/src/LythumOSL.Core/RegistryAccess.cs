using System;
using Microsoft.Win32;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core
{
	public class RegistryAccess : ILythumBase
	{
		#region Attributes
		RegistrySettingsType _KeyType;
		string _CompanyName;
		string _ApplicationName;

		#endregion

		#region Properties
		public RegistrySettingsType KeyType
		{
			get
			{
				return _KeyType;
			}
		}

		public object this[string param]
		{
			get
			{
				RegistryKey key = GetKey(false);
				return key.GetValue(param);
			}
			set
			{
				RegistryKey key = GetKey(true);
				key.SetValue(param, value);
			}
		}

		#endregion

		#region Construction

		/// <summary>
		/// Open current user registry key with default company name (ACG)
		/// </summary>
		/// <param name="application"></param>
		public RegistryAccess(string application)
			: this(application, Resources.Defaults.RegistryKeyDefaultCompany, false)
		{
		}

		/// <summary>
		/// Open current user registry key
		/// </summary>
		/// <param name="application"></param>
		/// <param name="companyName"></param>
		public RegistryAccess(string application, string companyName)
			: this(application, companyName, false)
		{
		}

		/// <summary>
		/// Open local machine or current user registry key with default company name (ACG)
		/// </summary>
		/// <param name="application"></param>
		/// <param name="isLocalMachine"></param>
		public RegistryAccess(string application, bool isLocalMachine)
			: this(application, Resources.Defaults.RegistryKeyDefaultCompany, isLocalMachine)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="application"></param>
		/// <param name="companyName"></param>
		/// <param name="isLocalMachine"></param>
		public RegistryAccess(
			string application,
			string companyName,
			bool isLocalMachine)
		{
			Validation.RequireValidString(application, "application");
			Validation.RequireValidString(companyName, "companyName");

			_ApplicationName = application;
			_CompanyName = companyName;

			_KeyType = (
				isLocalMachine ?
				RegistrySettingsType.LocalMachine :
				RegistrySettingsType.CurrentUser);
		}

		/// <summary>
		/// Open only some of system registry keys
		/// </summary>
		/// <param name="type"></param>
		public RegistryAccess(RegistrySettingsType type)
		{
			_KeyType = type;
		}
		#endregion

		#region Methods

		public T Get<T>(string name, T defaultValue)
		{
			RegistryKey key = GetKey(false);

			return (T)key.GetValue(name, defaultValue);
		}

		public T Get<T>(string name)
		{
			RegistryKey key = GetKey(false);

			return (T)key.GetValue(name);
		}

		public void Set(string name, object value)
		{
			RegistryKey key = GetKey(true);

			key.SetValue(name, value);
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Gets system registry key
		/// If key is unknown then like default will be returned current user
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static RegistryKey GetSystemKey(RegistrySettingsType type)
		{
			switch (type)
			{
				case RegistrySettingsType.ClassesRoot:
					return Registry.ClassesRoot;

				case RegistrySettingsType.CurrentConfig:
					return Registry.CurrentConfig;

				default:
				case RegistrySettingsType.CurrentUser:
					return Registry.CurrentUser;

				case RegistrySettingsType.DynData:
					return Registry.DynData;

				case RegistrySettingsType.LocalMachine:
					return Registry.LocalMachine;

				case RegistrySettingsType.PerformanceData:
					return Registry.PerformanceData;

				case RegistrySettingsType.Users:
					return Registry.Users;
			}
		}

		/// <summary>
		/// Return initialized key, in any system key case method return system (root level) key
		/// except is current user or local machine key
		/// </summary>
		/// <param name="writeAccess"></param>
		/// <returns></returns>
		public RegistryKey GetKey(bool writable)
		{
			RegistryKey parentKey = GetSystemKey(_KeyType);

			switch (_KeyType)
			{
				// System
				default:
					return parentKey;

				case RegistrySettingsType.CurrentUser:
				case RegistrySettingsType.LocalMachine:
					string subName = BuildSubKeyName();
					RegistryKey retVal = parentKey.OpenSubKey(subName, writable);

					if (retVal == null)
					{
						retVal = parentKey.CreateSubKey(subName);
					}

					return retVal;
			}

		}

		string BuildSubKeyName()
		{
			return string.Format(
				"{0}\\{1}\\{2}",
				Resources.Defaults.RegistryKeyDefaultSoftware,
				_CompanyName,
				_ApplicationName);
		}

		#endregion
	}

	#region Enumeration
	public enum RegistrySettingsType
	{
		// Summary:
		//     Defines the types (or classes) of documents and the properties associated
		//     with those types. This field reads the Windows registry base key HKEY_CLASSES_ROOT.
		ClassesRoot,
		//
		// Summary:
		//     Contains configuration information pertaining to the hardware that is not
		//     specific to the user. This field reads the Windows registry base key HKEY_CURRENT_CONFIG.

		CurrentConfig,
		//
		// Summary:
		//     Contains information about the current user preferences. This field reads
		//     the Windows registry base key HKEY_CURRENT_USER
		CurrentUser,
		//
		// Summary:
		//     Contains dynamic registry data. This field reads the Windows registry base
		//     key HKEY_DYN_DATA.
		//
		// Exceptions:
		//   System.ObjectDisposedException:
		//     The operating system is not Windows 98, Windows 98 Second Edition, or Windows
		//     Millennium Edition.
		DynData,
		//
		// Summary:
		//     Contains the configuration data for the local machine. This field reads the
		//     Windows registry base key HKEY_LOCAL_MACHINE.
		LocalMachine,
		//
		// Summary:
		//     Contains performance information for software components. This field reads
		//     the Windows registry base key HKEY_PERFORMANCE_DATA.
		PerformanceData,
		//
		// Summary:
		//     Contains information about the default user configuration. This field reads
		//     the Windows registry base key HKEY_USERS.
		Users
	}

	#endregion
}
