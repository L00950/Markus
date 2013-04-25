using System;
using System.Net.Mail;

namespace MarkusWebApplication
{
	/// <summary>
	/// Summary description for Mail.
	/// </summary>
	public partial class Mail : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if(!IsPostBack)
            {
                if (Request.QueryString["receiver"] != "")
                    this.receiver.Value = Request.QueryString["receiver"];
                if (Request.QueryString["subject"] != "")
                    this.subject.Value = Request.QueryString["subject"];
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

		protected void sendbutton_ServerClick(object sender, System.EventArgs e)
		{
			if(this.email.Value == "")
				this.errormessage.Text = "Please, enter your e-mail";
			else if(this.name.Value == "")
				this.errormessage.Text = "Please, enter your name";
			else
			{
				string body = "Namn: " + this.name.Value + "\n\n";
				if(this.address.Value != "")
					body += "Adress: " + this.address.Value + "\n\n";
				if(this.phone.Value != "")
					body += "Tel: " + this.phone.Value + "\n\n";
				body += "E-mail: " + this.email.Value + "\n\n";
				body += "Meddelande: " + this.message.Value;

				if(Request.QueryString["receiver"] == "markus")
					this.receiver.Value = "markus@linderback.com";

                try
                {
                    SmtpClient client = new SmtpClient("smtp.bredband.net");
                    client.Credentials = new System.Net.NetworkCredential("b248634", "jtk001");
                    client.Send(email.Value, receiver.Value, subject.Value, body);
                    if(receiver.Value == "markus@linderback.com")
                        Response.Redirect("mailskickat.htm");
                    Response.Redirect("mailsend.htm");
                }
                catch (Exception ex)
                {
                    this.errormessage.Text = "Error! Message not send, please try again later! " + ex.Message;
                }

                //ClassLibrary.Mail mailObj = new ClassLibrary.Mail();
                //bool retval = mailObj.SendMail("linderback.com", this.email.Value, this.receiver.Value, this.subject.Value, body);
                //if(retval == true)
                //Response.Redirect("mailsend.htm");
                //else
                //this.errormessage.Text = "Error! Message not send, please try again later!";
            }
		}
	}
}
