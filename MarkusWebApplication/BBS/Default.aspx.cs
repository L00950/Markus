using System;
using System.Web.UI;

namespace BBS
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hämta vilken kontroll som ska laddas
            var sida = Request.QueryString["sida"];
            if (Request.QueryString["id"] == null || Cache[Request.QueryString["id"]] == null || sida == null)
            {
                if (sida != null && sida == "nyttlosenord")
                {
                    innehåll.Controls.Add(LoadControl("NyttLösenord.ascx"));
                    return;
                }
                innehåll.Controls.Add(LoadControl("Logon.ascx"));
                return;
            }


            switch (sida)
            {
                case "medlemssida":
                    innehåll.Controls.Add(LoadControl("Medlemssida.ascx"));
                    break;
                case "medlemskalender":
                    innehåll.Controls.Add(LoadControl("MedlemsKalender.ascx"));
                    break;
                case "nyttlosenord":
                    innehåll.Controls.Add(LoadControl("NyttLösenord.ascx"));
                    break;
                case "ledigaplatser":
                    innehåll.Controls.Add(LoadControl("LedigaPlatser.ascx"));
                    break;
                case "ledigplats":
                    innehåll.Controls.Add(LoadControl("LedigPlats.ascx"));
                    break;
                case "medlemmar":
                    innehåll.Controls.Add(LoadControl("AllaMedlemmar.ascx"));
                    break;
                case "vaktlogg":
                    innehåll.Controls.Add(LoadControl("VaktLogg.ascx"));
                    break;
                case "vakt":
                    innehåll.Controls.Add(LoadControl("VaktÖversikt.ascx"));
                    break;
                case "vaktloggfordatum":
                    innehåll.Controls.Add(LoadControl("VaktLoggDatum.ascx"));
                    break;
            }
        }
    }
}