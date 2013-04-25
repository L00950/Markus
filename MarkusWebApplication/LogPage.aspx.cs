using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace MarkusWebApplication
{
	/// <summary>
	/// Summary description for LogPage.
	/// </summary>
	public partial class LogPage : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				FillLog();
				name.ToolTip = "Ange ditt namn här";
				messsage.ToolTip = "Skriv ditt meddelande här";
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void send_Click(object sender, System.EventArgs e)
		{
			if(messsage.Text != "")
				DataAccess.InsertLog(messsage.Text, name.Text);
			messsage.Text = "";
			name.Text = "";
			FillLog();
		}
		private void FillLog()
		{
			log.Text = "";
			DataSet ds = DataAccess.GetLog();
			for(int n=0;n<ds.Tables[0].Rows.Count;n++)
			{
				string message = ds.Tables[0].Rows[n].ItemArray[1].ToString().TrimEnd();
				string name = ds.Tables[0].Rows[n].ItemArray[3].ToString().TrimEnd();
				DateTime time = Convert.ToDateTime(ds.Tables[0].Rows[n].ItemArray[2]);
				log.Text += name + " skrev " + time.ToString("yyyy-MM-dd HH:mm:ss") + "\n" + message + "\n\n";
			}
			log.ToolTip = "Meddelande";
		}
	}
}
