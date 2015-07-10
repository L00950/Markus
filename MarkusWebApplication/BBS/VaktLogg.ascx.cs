using System;
using System.Linq;
using MarkusModel;

namespace BBS
{
    public partial class VaktLogg : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            KontrolleraSession();

            var vaktlogg = MedlemsRegister.LäsVaktLoggFörBryggplats(Request.QueryString["bryggplats"]);

            var resultat = (from m in vaktlogg
                            orderby m.Tidpunkt
                            select new {m.Tidpunkt, m.Plats}).ToList();

            lista.DataSource = resultat;
            lista.DataBind();
        }
        protected void TillbakaClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?sida=medlemssida&id=" + Request.QueryString["id"]);
        }
        private Medlem KontrolleraSession()
        {
            var obj = Cache[Request.QueryString["id"]];
            if (obj == null)
                Response.Redirect("Default.aspx");
            return (Medlem)obj;
        }
    }
}