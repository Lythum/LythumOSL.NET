using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using LythumOSL.Core;
using LythumOSL.Core.Enums;
using LythumOSL.Core.Data;
using LythumOSL.Core.Data.Info;
using LythumOSL.Core.Metadata;
using LythumOSL.Core.Data.Xml;
using LythumOSL.Core.Net.Http;

namespace LythumOSL.Net.Lslw
{
	/// <summary>
	/// High level LSLW webservice client implementation
	/// </summary>
	public class LslwService : LslwRawConnection, IDataAccess
	{
		DatabasesInfo _Info = null;

		#region ctor

		protected LslwService ()
			: base()
		{
		}

		public LslwService (LslwSettings settings)
			: base (settings)
		{
		}

		public LslwService (LslwSettings settings, IMessenger messenger)
			: base(settings, messenger)
		{
		}

		#endregion

		#region Methods

		public LslwResult Request (LslwRequest request)
		{
			HttpAttributes attr = new HttpAttributes ();
			string xml =  Xml.SerializeObject (request);
			LslwResult result = null;

			if (Connect ())
			{

				attr["data"] = Aes.Encrypt (xml);

				result = RawRequest (
					LslwRawOperation.AesData,
					attr);
			}

			return result;
			//else
			//{
			//    throw new Exception ("Can't connect to the server!");
			//}
		}

		public LslwResult RequestExecute (string sql)
		{
			LslwRequest request = InitRequest ();
			request.Type = LslwRequestType.Execute;
			request.Command = sql;

			return Request (request);

		}

		public LslwResult RequestQuery (string sql)
		{
			LslwRequest request = InitRequest ();
			request.Type = LslwRequestType.Query;
			request.Command = sql;

			return Request (request);
		}

		public LslwResult RequestCustom (string command, string[] data)
		{
			LslwRequest request = InitRequest ();
			request.Type = LslwRequestType.Custom;
			request.Command = command;
			request.Data = data;

			return Request (request);

		}

		#endregion

		#region Helpers

		LslwRequest InitRequest ()
		{
			LslwRequest request = new LslwRequest ();
			request.TimeStamp = DateTime.Now;
			//request.TimeStamp = new DateTime (2012, 12, 01);

			return request;
		}

		#endregion

		#region IDataAccess Members

		public DataTable Query (string sql)
		{
			DataSet ds = QueryDataSet (sql);

			if (ds != null)
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}

			return null;
		}

		public DataSet QueryDataSet (string sql)
		{
			LslwResult result = RequestQuery (sql);
			DataSet retVal = null;

			if (result != null)
			{
				if (result.Data != null)
				{
					if (result.Data.DataSet != null)
					{
						return result.Data.DataSet;
					}
				}
			}

			return retVal;
		}

		public string QueryScalar (string sql)
		{
			string retVal = string.Empty;
			DataTable table = Query (sql);

			if (table != null)
			{
				if (table.Rows.Count > 0)
				{
					retVal = table.Rows[0][0].ToString ();
				}
			}

			return retVal;
		}

		public int Execute (string sql)
		{
			LslwResult result = RequestExecute (sql);

			//if (result != null)
			//{
			//    if (result.Data != null)
			//    {
			//        if (result.Data.Table != null)
			//        {
			//            retVal = result.Data.Table;
			//        }
			//    }
			//}

			return 0;
		}

		public DatabaseTypes DatabaseType
		{
			get { return DatabaseTypes.MySql; }
		}

		public DatabasesInfo Info
		{
			get
			{
				if (_Info == null)
				{
					_Info = new DatabasesInfo (this.DatabaseType);
				}

				return _Info;
			}
		}
		#endregion

	}
}
