using System;
using System.Linq;
using MarkusModel;

namespace BBS
{
    public partial class AllaMedlemmar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            KontrolleraSession();

            var medlemmar = MedlemsRegister.HämtaAllaMedlemmar();
            var bryggplatser = MedlemsRegister.HämtaAllaBryggplatser();

            var resultat = (from m in medlemmar
                            join b in bryggplatser on m.Id equals b.MedlemsId
                            orderby m.Namn
                            select new {m.Namn, m.Email, m.Tel, m.Mobil, Plats = b.Id, b.Båt}).ToList();

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