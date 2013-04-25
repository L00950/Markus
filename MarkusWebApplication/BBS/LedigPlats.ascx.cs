using System;
using System.Linq;
using System.Web.UI.WebControls;
using MarkusModel;

namespace BBS
{
    public partial class LedigPlats : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            ListaPlatser();
        }

        private void ListaPlatser()
        {
            var medlemmar = MedlemsRegister.HämtaAllaMedlemmar().ToList();
            var bryggplatser = MedlemsRegister.HämtaAllaBryggplatser().ToList();

            var ledigaPlatser =
                FilHanterare.Läs<LedigBryggplats>(new LedigBryggplats().FilNamn())
                            .Where(_ => _.Dag == Convert.ToDateTime(Request.QueryString["datum"])).ToList();

            foreach (var ledigplats in ledigaPlatser)
            {
                var bryggplats = bryggplatser.FirstOrDefault(_ => _.Id == ledigplats.BryggplatsId);
                if(bryggplats == null)
                    continue;
                var medlem = medlemmar.FirstOrDefault(_ => _.Id == bryggplats.MedlemsId);
                if(medlem == null)
                    continue;

                var rad = new TableRow();

                rad.Cells.Add(new TableCell { Text = bryggplats.Id });
                rad.Cells.Add(new TableCell { Text = medlem.Namn });
                rad.Cells.Add(new TableCell { Text = medlem.Tel });
                rad.Cells.Add(new TableCell { Text = medlem.Mobil });
                rad.Cells.Add(new TableCell { Text = medlem.Email });

                tabell.Rows.Add(rad);
            }
        }

        protected void TillbakaKnappClick(object sender, EventArgs e)
        {
            KontrolleraSession();
            Response.Redirect("Default.aspx?sida=ledigaplatser&id=" + Request.QueryString["id"]);
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