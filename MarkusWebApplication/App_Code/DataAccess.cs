using System;
using System.Data;
using System.Data.SqlClient;

namespace MarkusWebApplication
{
	/// <summary>
	/// Summary description for DataAccess.
	/// </summary>
	public class DataAccess
	{
		public static DataSet FillBookings(int objectid)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spGetBookings";
			com.Parameters.Add("@objectid", objectid);
			return DataGeneric.ExecQueryReturnDataSet(com);
		}
		public static DataSet GetBooking(int id)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spGetBooking";
			com.Parameters.Add("@id", id);
			return DataGeneric.ExecQueryReturnDataSet(com);
		}
		public static void InsertBooking(int objectid, DateTime startdate, DateTime enddate, string person, string note)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spInsertBooking";
			com.Parameters.Add("@startdate", startdate);
			com.Parameters.Add("@enddate", enddate);
			com.Parameters.Add("@person", person);
			com.Parameters.Add("@note", note);
			com.Parameters.Add("@objectid", objectid);
			DataGeneric.ExecQuery(com);
		}
		public static void UpdateBooking(int id, DateTime startdate, DateTime enddate, string person, string note)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spUpdateBooking";
			com.Parameters.Add("@id", id);
			com.Parameters.Add("@startdate", startdate);
			com.Parameters.Add("@enddate", enddate);
			com.Parameters.Add("@person", person);
			com.Parameters.Add("@note", note);
			DataGeneric.ExecQuery(com);
		}
		public static void DeleteBooking(int id)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spDeleteBooking";
			com.Parameters.Add("@id", id);
			DataGeneric.ExecQuery(com);
		}
		public static bool IsBooked(int objectid, DateTime date)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spIsBooked";
			com.Parameters.Add("@date", date);
			com.Parameters.Add("@objectid", objectid);
			return Convert.ToBoolean(DataGeneric.ExecQueryReturnInt(com));
		}
		public static int Login(string username, string password)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spLogin";
			com.Parameters.Add("@username", username);
			com.Parameters.Add("@password", password);
			return DataGeneric.ExecQueryReturnInt(com);
		}

		#region LogPage
		public static DataSet GetLog()
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spGetLog";
			return DataGeneric.ExecQueryReturnDataSet(com);
		}
		public static void InsertLog(string message, string name)
		{
			SqlCommand com = new SqlCommand();
			com.CommandType = CommandType.StoredProcedure;
			com.CommandText = "spInsertLog";
			com.Parameters.Add("@message", message);
			com.Parameters.Add("@name", name);
			DataGeneric.ExecQuery(com);
		}
		#endregion
	}
}
