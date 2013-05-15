using System;

namespace Danica
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void okButton_Click(object sender, EventArgs e)
        {
            if (lösenord.Text == "7865")
            {
                Response.Cookies.Add(MarkusWebApplication.Danica.SkapaNyCookie());
                Response.Redirect("Default.aspx");
            }
            lösenord.Text = "";
        }
    }
}
