using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using LythumOSL.Core;

namespace LythumOSL.Core.Data.Xml
{
	[Serializable]
	public class TinyXmlDataTable : DataTable, IXmlSerializable
	{
		#region Constants
		const string DefaultElementTable = "table";
		const string DefaultElementColumns = "cs";
		const string DefaultElementColumn = "c";
		const string DefaultElementRows = "rs";
		const string DefaultElementRow = "r";

		const string DefaultLabelName = "nm";
		const string DefaultLabelAlias = "al";
		const string DefaultLabelType = "tp";

		#endregion

		public TinyXmlDataTable ()
			: base("t")
		{
		}

		public TinyXmlDataTable (string tableName)
			: base (tableName)
		{
			Validation.RequireValidString ("tableName", tableName);
		}

		#region IXmlSerializable Members

		XmlSchema IXmlSerializable.GetSchema ()
		{
			return null;
		}

		#region ReadXml

		void IXmlSerializable.ReadXml (XmlReader r)
		{
			this.ReadXmlSchema (r);
			this.ReadXml (r);
		}

		#endregion

		#region WriteXml

		void IXmlSerializable.WriteXml (XmlWriter w)
		{
			this.WriteXmlSchema (w);
			this.WriteXml (w);
		}

		#endregion // WriteXml

		#endregion
	}
}
