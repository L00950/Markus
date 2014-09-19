using System;

public partial class UpdateWeb : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var start = DateTime.Now;
        new MarkusModel.Kalender().CreateCalendar(6);
        var tidDetTog = DateTime.Now - start;
        tid.Text = tidDetTog.TotalMilliseconds.ToString("# ### ##0");
    }
}
