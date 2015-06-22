using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using LythumOSL.Core.Metadata;

namespace LythumOSL.Core.Data.Xml
{
	public class Xml : ILythumBase
	{
		/// <summary>
		/// Kadangi .net string yra UTF16 formato
		/// tai jei serializuota i UTF8 informacija priskiriame string'ui
		/// ji vel tampa UTF16, del to geriau is karto saugoti i faila!
		/// </summary>
		/// <param name="o"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool UTF8Serialize (
			object o, string path)
		{
			XmlSerializer serializer = new XmlSerializer (o.GetType ());

			XmlWriterSettings settings = new XmlWriterSettings ();
			settings.Encoding = new UTF8Encoding (false); // no BOM in a .NET string
			settings.Indent = true;
			settings.OmitXmlDeclaration = false;

			using (StreamWriter writer = new StreamWriter (
				path, 
				false,
				new UTF8Encoding (false, false)))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create (writer, settings))
				{
					serializer.Serialize (xmlWriter, o);
				}
				return true;
			}
		}

		public static string SerializeObject (object o)
		{
			XmlSerializer serializer = new XmlSerializer (o.GetType ());

			using (StringWriter writer = new Utf8StringWriter ())
			{
				serializer.Serialize (writer, o);

				return writer.ToString ();
			}
		}

		public static string Serialize<T> (T o)
		{
#warning TODO: Serialize<T> Implement Unicode support

			XmlSerializer ser = new XmlSerializer (typeof (T));
			StringWriter sw = new StringWriter ();
			ser.Serialize (sw, o);

			return sw.ToString ();
		}

		public static T Deserialize<T> (string xml)
			where T : new ()
		{
			T retVal = new T ();

            try
            {
                if (string.IsNullOrEmpty(xml.Trim()))
                {
                    retVal = new T();
                }
                else
                {

                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    StringReader sr = new StringReader(xml);
                    retVal = (T)ser.Deserialize(sr);
                }
            }
            catch (InvalidOperationException ioex)
            {
                Debug.WriteLine(ioex.Message);
                
                retVal = new T();
            }
            catch (Exception ex)
            {
                retVal = new T();
            }

			return retVal;
		}

		public static T DeserializeFromFile<T> (string file)
			where T : new ()
		{
			XmlSerializer serializer = new XmlSerializer (typeof (T));
			TextReader tr = new StreamReader (file);
			T retVal = (T)serializer.Deserialize (tr);
			tr.Close ();

			return retVal;
		}

		public class Utf8StringWriter : StringWriter
		{
			public override Encoding Encoding
			{
				get { return Encoding.UTF8; }
			}
		}
	}
}
