using System;

namespace MarkusWebApplication
{
	public partial class Mail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, EventArgs e)
		{
            if(!IsPostBack)
            {
                if (Request.QueryString["receiver"] != "")
                    receiver.Value = Request.QueryString["receiver"];
                if (Request.QueryString["subject"] != "")
                    subject.Value = Request.QueryString["subject"];
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

		protected void sendbutton_ServerClick(object sender, EventArgs e)
		{
			if(email.Value == "")
				errormessage.Text = "Please, enter your e-mail";
			else if(name.Value == "")
				errormessage.Text = "Please, enter your name";
			else
			{
				var body = "Namn: " + name.Value + "\n\n";
				if(address.Value != "")
					body += "Adress: " + address.Value + "\n\n";
				if(phone.Value != "")
					body += "Tel: " + phone.Value + "\n\n";
				body += "E-mail: " + email.Value + "\n\n";
				body += "Meddelande: " + message.Value;

				if(Request.QueryString["receiver"] == "markus")
					receiver.Value = "markus@linderback.com";

                try
                {
                    var client = Gmail.GmailSmtpKlient();
                    client.Send(email.Value, receiver.Value, subject.Value, body);
                    if(receiver.Value == "markus@linderback.com")
                        Response.Redirect("mailskickat.htm");
                    Response.Redirect("mailsend.htm");
                }
                catch (Exception ex)
                {
                    errormessage.Text = "Error! Message not send, please try again later! " + ex.Message;
                }
            }
		}
	}
}
