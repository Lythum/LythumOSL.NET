using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LythumOSL.Core.Data
{
	[XmlRoot ("AlphaDictionary")]
	public class LythumDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		const string DefaultItemName = "I";
		const string DefaultKeyName = "K";
		const string DefaultValueName = "V";

		#region CTORS

		public LythumDictionary ()
		{
		}

		/// <summary>
		/// Dictionary which can be initialized on construction
		/// </summary>
		/// <param name="entries"></param>
		public LythumDictionary (
			LythumDictionaryEntry<TKey, TValue>[] entries)
		{
			if (entries != null)
			{
				foreach (
					LythumDictionaryEntry<TKey, TValue> entry in entries)
				{
					Add (entry.Key, entry.Value);
				}
			}
		}

		#endregion

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		public void ReadXml (System.Xml.XmlReader reader)
		{
			XmlSerializer keySerializer = 
				new XmlSerializer (typeof (TKey));

			XmlSerializer valueSerializer = 
				new XmlSerializer (typeof (TValue));

			bool wasEmpty = reader.IsEmptyElement;

			reader.Read ();

			if (wasEmpty)
				return;

			while (
				reader.NodeType != System.Xml.XmlNodeType.EndElement)
			{
				reader.ReadStartElement (DefaultItemName);
				reader.ReadStartElement (DefaultKeyName);

				TKey key = (TKey)keySerializer.Deserialize (reader);

				reader.ReadEndElement ();

				reader.ReadStartElement (DefaultValueName);

				TValue value = (TValue)valueSerializer.Deserialize (reader);

				reader.ReadEndElement ();

				this.Add (key, value);

				reader.ReadEndElement ();
				reader.MoveToContent ();
			}

			reader.ReadEndElement ();
		}



		public void WriteXml (System.Xml.XmlWriter writer)
		{

			XmlSerializer keySerializer = 
				new XmlSerializer (typeof (TKey));

			XmlSerializer valueSerializer = 
				new XmlSerializer (typeof (TValue));

			foreach (TKey key in this.Keys)
			{
				writer.WriteStartElement (DefaultItemName);
				writer.WriteStartElement (DefaultKeyName);

				keySerializer.Serialize (writer, key);

				writer.WriteEndElement ();

				writer.WriteStartElement (DefaultValueName);
				TValue value = this[key];
				valueSerializer.Serialize (writer, value);
				writer.WriteEndElement ();
				writer.WriteEndElement ();
			}
		}

		#endregion
	}
}
