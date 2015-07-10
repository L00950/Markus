using System;
using System.Collections.Generic;
using System.Linq;
using MarkusModel;

namespace BBS
{
    public partial class VaktLoggDatum : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            KontrolleraSession();

            var filer = MedlemsRegister.HämtaVaktLoggFiler();
            var loggposter = new List<Loggpost>();
            foreach (var fil in filer)
                loggposter.AddRange(MedlemsRegister.LäsVaktLogg(fil));

            var loggposterFörDatum = HämtaFörDatum(loggposter, Convert.ToDateTime(Request.QueryString["datum"])).ToList();

            var resultat = (from m in loggposterFörDatum
                            orderby m.Tidpunkt
                            select new {m.Tidpunkt, m.Plats}).ToList();

            lista.DataSource = resultat;
            lista.DataBind();
        }

        private IEnumerable<Loggpost> HämtaFörDatum(IEnumerable<Loggpost> loggposter, DateTime datum)
        {
            var starttid = new DateTime(datum.Year, datum.Month, datum.Day, 18, 0, 0);
            var sluttid = starttid.AddDays(1);
            return loggposter.Where(_ => _.Tidpunkt >= starttid && _.Tidpunkt < sluttid);
        }

        protected void TillbakaClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?sida=vakt&id=" + Request.QueryString["id"]);
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