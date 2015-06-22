using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;

using LythumOSL.Core;

namespace LythumOSL.Core.IO
{
	public class File : ErrorInfo
	{
		/*
		< (less than)
		> (greater than)
		: (colon)
		" (double quote)
		/ (forward slash)
		\ (backslash)
		| (vertical bar or pipe)
		? (question mark)
		* (asterisk)
		*/
		public const string UnallowedFSSymbols = "<>:\"|?*";



		#region Helpers
		/// <summary>
		/// File exist validation
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsExist(string path)
		{
			FileInfo info = new FileInfo(path);
			return info.Exists;
		}

		/// <summary>
		/// Validate directory
		/// </summary>
		/// <param name="path">Path with directory and file</param>
		/// <returns></returns>
		public static bool IsValidDirectory(string path)
		{
			FileInfo info = new FileInfo(path);
			return info.Directory.Exists;
		}

		/// <summary>
		/// Generate and return temporary file name
		/// </summary>
		/// <returns></returns>
		public static string MakeTemporaryFileName()
		{
			return Path.GetTempFileName();
		}

		/// <summary>
		/// If given filename or path is empty it generate temporary file name
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static void FileSolution(ref string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				path = MakeTemporaryFileName();
			}
		}

		public static void Write (string fileName, string data, bool append)
		{
			Write (fileName, data, append, Encoding.Unicode);
		}

		public static void Write (string fileName, string data, bool append, Encoding enc)
		{
			StreamWriter sw = new StreamWriter (fileName, append, enc);

			sw.Write (data);
			sw.Close ();
		}

		public static void Write (
			string fileName,
			DataTable table,
			string separator,
			bool append,
			bool withColumnNames,
			bool skipWithUnderscoreBegin)
		{
			if (table != null)
			{
				bool appendNow = append;
				Dictionary<int, bool> _AddMap = 
					new Dictionary<int, bool> ();
				string buffer = string.Empty;

				if (withColumnNames)
				{
					for(int i=0;i<table.Columns.Count;i++)
					{
						string name = table.Columns[i].ColumnName;
						bool addIt;

						if(skipWithUnderscoreBegin &&
							name.Substring(0,1).Equals("_"))
						{
							addIt = false; 
						}
						else
						{
							addIt = true;
						}

						_AddMap.Add (i, addIt);

						if (addIt)
						{
							buffer += name + separator;
						}
					}
					buffer += "\r\n";
					Write (fileName, buffer, appendNow);

					appendNow = true;
				}

				bool first = true;

				foreach (DataRow r in table.Rows)
				{
					buffer = string.Empty;

					for (int i = 0; i < table.Columns.Count; i++)
					{
#warning TODO: bugas, vyksta luzis sioje vietoje jei withColumnNames=false!
						if (_AddMap[i])
						{
							buffer += r[i].ToString () + separator;
						}
					}
					buffer += "\r\n";

					Write (fileName, buffer, appendNow);

					appendNow = true;
				}
			}
		}

		public static string Read (string filename)
		{
			string retVal = string.Empty;

			if (File.IsExist (filename))
			{
				TextReader tr = new StreamReader ( filename );

				retVal = tr.ReadToEnd ();
			}

			return retVal;
		}

		public static IEnumerable<string> ReadLines (string filename, Encoding enc)
		{
			string line = string.Empty;

			if (File.IsExist (filename))
			{
				TextReader tr = new StreamReader (filename, enc);

				do
				{
					line = tr.ReadLine ();

					if (!string.IsNullOrEmpty (line))
					{
						yield return line;
					}

				} while (!string.IsNullOrEmpty (line));
			}
		}

		public static string FixFileName (string fileName)
		{
			string retVal = fileName;

			foreach (char c in UnallowedFSSymbols)
			{
				retVal = retVal.Replace (c, '-');
			}

			return retVal;
		}
		#endregion
	}
}
