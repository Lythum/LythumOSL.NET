#define RESOLVE_ASSEMBLIES_DEBUG_ONLY
//#define MULTILANGUAGE_ENABLED
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace LythumOSL.Wpf
{
	public class LythumApplication : Application
	{
		#region Constants

		const string NameOfCulture = "Culture";
		const string NameOfUICulture = "UICulture";

		#endregion

		#region Ctor

		public LythumApplication (ApplicationSettingsBase settings)
			: base ()
		{
#if MULTILANGUAGE_ENABLED
			LythumApplication.InitializeCultures (settings);
#endif

#if (RESOLVE_ASSEMBLIES_DEBUG_ONLY && DEBUG)
			InitResolver ();
#endif
		}

		public LythumApplication ()
			: base ()
		{
		}

		#endregion

		#region Multilingual

		private static void InitializeCultures (ApplicationSettingsBase settings)
		{
			if (settings[NameOfCulture] != null)
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo (settings[NameOfCulture].ToString ());
			}
			if (settings[NameOfUICulture] != null)
			{
				Thread.CurrentThread.CurrentUICulture = new CultureInfo (settings[NameOfUICulture].ToString ());
			}

			FrameworkElement.LanguageProperty.OverrideMetadata (typeof (FrameworkElement), new FrameworkPropertyMetadata (
				XmlLanguage.GetLanguage (CultureInfo.CurrentCulture.IetfLanguageTag)));
		}

		#endregion

		#region Reference solving

#if (RESOLVE_ASSEMBLIES_DEBUG_ONLY && DEBUG)

		public void InitResolver ()
		{
			AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs e)
			{
				Debug.Print ("Assembly resolve, for: " + e.Name);

				AssemblyName requestedName = new AssemblyName (e.Name);

				if (requestedName.Name.Contains ("LythumOSL.UI"))
				{
					string partialName = e.Name.Substring (0, e.Name.IndexOf (','));
					return Assembly.Load (new AssemblyName (partialName));
				}
				else
				{
					return null;
				}
			};
		}

#endif

		#endregion
	}
}
