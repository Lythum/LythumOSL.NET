using System;
using System.Deployment.Application;
using System.Collections.Generic;
using System.Text;

namespace LythumOSL.Core.Licensing
{
	public class ClickOnce
	{
		#region enums
		public enum UpdateStatus
		{
			/// <summary>
			/// Update not found
			/// </summary>
			UpdateNotFound,
			/// <summary>
			/// Update found but user canceled updating
			/// </summary>
			UpdateFoundButCanceled,
			/// <summary>
			/// Update found, probably tried to update but got error
			/// </summary>
			UpdateFoundButErrorOnInstall,
			/// <summary>
			/// System sucessful updated
			/// </summary>
			Updated,
			/// <summary>
			/// Some error happen checking for updates
			/// </summary>
			SystemError,
			/// <summary>
			/// Application is not network installed or not at all ClickOnce system type
			/// </summary>
			WrongApplication,
			/// <summary>
			/// Update found looks like we can install it
			/// </summary>
			UpdateAvailable,
		}

		public enum UpdateMode
		{
			/// <summary>
			/// User can choose do to do update or not
			/// </summary>
			Manual,
			/// <summary>
			/// Update must be done by force or quit from program
			/// </summary>
			Enforced,
		}

		#endregion

		/// <summary>
		/// Silent checking for updates
		/// </summary>
		/// <param name="onlyIfNetworkDeployed">proceed only if app is network deployed</param>
		/// <returns>
		/// true - update found
		/// false - update not found or error
		/// </returns>
		public static bool CheckForUpdates (bool onlyIfNetworkDeployed)
		{
			UpdateCheckInfo info = null;

			if (onlyIfNetworkDeployed &&
				!ApplicationDeployment.IsNetworkDeployed)
			{
				// is not network deployed, finishing
				return false;
			}

			// init deployment instance
			ApplicationDeployment ad;

			try
			{
				ad = ApplicationDeployment.CurrentDeployment;
			}
			catch
			{
				return false;
			}

			// checking for update
			try
			{
				info = ad.CheckForDetailedUpdate ();

			}
			catch (DeploymentDownloadException dde)
			{
				return false;
			}
			catch (InvalidDeploymentException ide)
			{
				return false;
			}
			catch (InvalidOperationException ioe)
			{
				return false;
			}


			if (info.UpdateAvailable)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Returns version in human readable format
		/// </summary>
		/// <param name="defaultVersion"></param>
		/// <returns>Returns version in human readable format exp. 1.0.0.1</returns>
		public static Version GetCurrentDeploymentVersion (string defaultVersion)
		{
			if (ApplicationDeployment.IsNetworkDeployed)
			{
				return ApplicationDeployment.CurrentDeployment.CurrentVersion;
			}

			return new Version (defaultVersion);
		}

		/// <summary>
		/// Returns version in machine readable format
		/// </summary>
		/// <returns>Returns version in machine readable format</returns>
		public static string GetCurrentDeploymentVersion (Version defaultVersion)
		{
			// version info msdn reference: http://msdn.microsoft.com/en-us/library/bff8h2e1.aspx
			// major.minor.build.revision 

			Version ver = defaultVersion;

			// if it's empty
			if (ver == null)
			{
				ver = new Version ("1.0.0.0");
			}

			// All ok clickonce deployiment app found
			// getting version
			if (ApplicationDeployment.IsNetworkDeployed)
			{
				ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
			}

			string retVal = string.Empty;

			retVal += ver.Major.ToString ("X8");
			retVal += ver.Minor.ToString ("X8");
			retVal += ver.Build.ToString ("X8");
			retVal += ver.Revision.ToString ("X8");

			return retVal;
		}
	}
}
