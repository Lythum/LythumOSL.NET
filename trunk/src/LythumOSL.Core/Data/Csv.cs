using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using LythumOSL.Core;
using LythumOSL.Core.Metadata;
using LythumOSL.Core.IO;

namespace LythumOSL.Core.Data
{
	public class Csv
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		/// <param name="table"></param>
		/// <param name="delimiter"></param>
		/// <param name="includeHeader"></param>
		/// <param name="progress">can be null</param>
		public static void Write (
			string file,
			DataTable table,
			string delimiter,
			bool includeHeader,
			IProgressIterator progress)
		{
			string data = string.Empty;
			int columns = 0;

			if (includeHeader)
			{
				if (table != null)
				{
					columns = table.Columns.Count;

					// Writting column names
					for (int i = 0; i < columns; i++)
					{
						if (i > 0)
						{
							data += delimiter;
						}

						data += table.Columns[i].ColumnName;
					}
					// Adding end
					data += "\r\n";

					File.Write (file, data, false);
				}

				if (progress != null)
				{
					progress.Minimum = 0;
					progress.Maximum = table.Rows.Count;
					progress.Value = 0;
				}

				// Writting data
				foreach (DataRow row in table.Rows)
				{
					data = string.Empty;

					for (int i = 0; i < columns; i++)
					{
						if (i > 0)
						{
							data += delimiter;
						}

						data += row.ItemArray[i].ToString ();
					}
					data += "\r\n";
					File.Write (file, data, true);

					if (progress != null)
					{
						if (progress.Maximum > progress.Value)
						{
							progress.Value++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Opens CSV file
		/// </summary>
		/// <param name="file"></param>
		/// <param name="delimiter"></param>
		/// <param name="firstRowIsHeader"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static DataTable Open (
			string file,
			char delimiter,
			bool firstRowIsHeader,
			Encoding encoding)
		{
			System.IO.FileInfo fi = new System.IO.FileInfo (file);
			DataTable table = new DataTable (fi.Name);
			bool first = (firstRowIsHeader ? true : false);

			foreach (
				string line in Lythum.Core.IO.File.ReadLines (file, encoding))
			{
				if (line.Length > 0)
				{
					string[] columns = line.Split (delimiter);

					// if row have more columns then table then add they
					if (columns.Length > table.Columns.Count)
					{
						if (first)
						{
							foreach (string field in columns)
							{
								if (table.Columns.Contains (field))
								{
									table.Columns.Add (field + table.Columns.Count.ToString ());
								}
								else
								{
									table.Columns.Add (field);
								}
							}
						}
						else
						{
							int from = table.Columns.Count;
							int to = columns.Length;

							for (int i = from; i < to; i++)
							{
								table.Columns.Add ((i + 1).ToString ());
							}
						}
					}

					if (!first)
					{
						// add data
						DataRow row = table.NewRow ();
						for (int x = 0; x < columns.Length; x++)
						{
							row[x] = columns[x];
						}

						table.Rows.Add (row);
					} // first

					first = false;
				} // if (line.Length > 0)
			} // foreach line

			return table;


		}

		/// <summary>
		/// Open CSV with system default encoding
		/// </summary>
		/// <param name="file"></param>
		/// <param name="delimiter"></param>
		/// <param name="firstRowIsHeader"></param>
		/// <returns></returns>
		public static DataTable Open (
			string file,
			char delimiter,
			bool firstRowIsHeader)
		{
			return Open (
				file,
				delimiter,
				firstRowIsHeader,
				Encoding.Default);
		}
	}
}
