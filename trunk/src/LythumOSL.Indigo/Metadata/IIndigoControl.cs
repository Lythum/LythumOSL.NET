using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Indigo.Enums;

namespace LythumOSL.Indigo.Metadata
{
	public interface IIndigoControl
	{
		List<UICommands> Abilities { get; }

		void UICommand (UICommands cmd);
		void ControlActivation ();

		event EventHandler ControlActivated;
		event EventHandler ControlReactivated;
	}
}
