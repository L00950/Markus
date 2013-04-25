using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace MarkusWebApplication
{
	public class DataGeneric
	{
		private static SqlConnection GetConnection()
		{
			SqlConnection conn = new SqlConnection();
#if(DEBUG)
            conn.ConnectionString = "server=STATION;User ID=sa;Pwd=;database=MARKUS";
#else
			conn.ConnectionString = "server=STATION;User ID=sa;Pwd=;database=MARKUS";
#endif
            conn.Open();
			return conn;
		}

		public static XmlElement ExecQueryReturnXml(SqlCommand com)
		{
			com.Connection = GetConnection();
			XmlReader xr = com.ExecuteXmlReader();
			XmlDocument xml = new XmlDocument();
			xml.Load(xr);
			xr.Close();
			com.Connection.Close();
			com.Connection.Dispose();
			com.Dispose();
			return xml.DocumentElement;
		}

		public static DataSet ExecQueryReturnDataSet(SqlCommand com)
		{
			com.Connection = GetConnection();
			DataSet ds = new DataSet();
			SqlDataAdapter adapter = new SqlDataAdapter();
			adapter.SelectCommand = com;
			adapter.Fill(ds);
			com.Connection.Close();
			com.Connection.Dispose();
			com.Dispose();
			return ds;
		}

		public static int ExecQueryReturnInt(SqlCommand com)
		{
			com.Connection = GetConnection();
			SqlDataReader sdr = com.ExecuteReader();
			sdr.Read();
			int nRetVal = sdr.GetInt32(0);
			sdr.Close();
			com.Connection.Close();
			com.Connection.Dispose();
			com.Dispose();
			return nRetVal;
		}

		public static double ExecQueryReturnDouble(SqlCommand com)
		{
			com.Connection = GetConnection();
			SqlDataReader sdr = com.ExecuteReader();
			sdr.Read();
			double dRetVal = sdr.GetDouble(0);
			sdr.Close();
			com.Connection.Close();
			com.Connection.Dispose();
			com.Dispose();
			return dRetVal;
		}

		public static int ExecQuery(SqlCommand com)
		{
			SqlParameter returnParam = com.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;
			com.Connection = GetConnection();
			com.ExecuteNonQuery();
			int nReturn = int.Parse(com.Parameters["RETURN_VALUE"].Value.ToString());
			com.Connection.Close();
			com.Connection.Dispose();
			com.Dispose();
			return nReturn;
		}
	}
}
