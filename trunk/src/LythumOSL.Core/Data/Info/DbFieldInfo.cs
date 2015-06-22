using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data.Info
{
	public class DbFieldInfo : ILythumBase
	{
		public string Name { get; set; }
		public DbFieldUpdateMode UpdateMode { get; set; }

		public DbFieldInfo ()
		{
			UpdateMode = DbFieldUpdateMode.ValueEmptyToNull;
		}

		public DbFieldInfo (
			string name)
			: this ()
		{
			Name = name;
		}

		public DbFieldInfo (
			string name,
			DbFieldUpdateMode updateMode)
			: this (name)
		{
			UpdateMode = updateMode;
		}
	}
}
