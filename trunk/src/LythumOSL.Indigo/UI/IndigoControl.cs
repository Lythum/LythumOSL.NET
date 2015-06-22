using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using LythumOSL.Indigo.Enums;
using LythumOSL.Indigo.Metadata;

namespace LythumOSL.Indigo.UI
{
	public class IndigoControl : UserControl, IIndigoControl
	{
		public IndigoControl ()
		{
			this.Abilities = new List<UICommands> ();
		}

		#region IIndigoControl Members

		public List<UICommands> Abilities { get; protected set; }

		#endregion

		#region Virtual
		protected virtual void OnControlActivated (EventArgs e)
		{
			if (ControlActivated != null)
			{
				ControlActivated (this, e);
			}
		}

		protected virtual void OnControlReactivated (EventArgs e)
		{
			if (ControlReactivated != null)
			{
				ControlReactivated (this, e);
			}
		}

		#endregion

		#region IIndigoControl Members


		public virtual void UICommand (UICommands cmd)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// First time activated control
		/// </summary>
		public event EventHandler ControlActivated;
		/// <summary>
		/// Control activated more than one time
		/// </summary>
		public event EventHandler ControlReactivated;

		bool _Activated = false;
		public void ControlActivation ()
		{
			if (_Activated)
			{
				OnControlReactivated (new EventArgs ());
			}
			else
			{
				_Activated = true;
				OnControlActivated (new EventArgs ());
			}
		}

		#endregion
	}
}
