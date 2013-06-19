using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MarkusModel;

namespace BBS
{
    public partial class MedlemsKalender : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LaddaKalender();
        }

        private void LaddaKalender()
        {
            var ledigaDatum =
                FilHanterare.Läs<LedigBryggplats>(new LedigBryggplats().FilNamn())
                            .Where(_ => _.BryggplatsId == BryggplatsId()).ToList();

            for (var i = 1; i <= 31; i++)
            {
                var rad = new TableRow();

                rad.Cells.Add(SkapaCell(5, i, ledigaDatum));
                rad.Cells.Add(SkapaCell(6, i, ledigaDatum));
                rad.Cells.Add(SkapaCell(7, i, ledigaDatum));
                rad.Cells.Add(SkapaCell(8, i, ledigaDatum));
                rad.Cells.Add(SkapaCell(9, i, ledigaDatum));

                tabell.Rows.Add(rad);
            }
        }

        private static TableCell SkapaCell(int månad, int dag, IEnumerable<LedigBryggplats> ledigaDatum)
        {
            var cell = new TableCell();
            if (dag <= DateTime.DaysInMonth(DateTime.Today.Year, månad))
            {
                var datum = new DateTime(DateTime.Today.Year, månad, dag);
                cell.ForeColor = datum.DayOfWeek == DayOfWeek.Sunday
                                     ? System.Drawing.Color.Red
                                     : System.Drawing.Color.Black;
                var checkbox = new CheckBox
                                   {
                                       ID = String.Format("CD{0}{1}", datum.Month.ToString("#00"), datum.Day.ToString("#00")),
                                       Text = datum.Day + " " + HämtaMånadsnamn(datum),
                                       Enabled = datum >= DateTime.Today,
                                       Checked = ledigaDatum.Any(_ => _.Dag == datum)
                                   };
                cell.Controls.Add(checkbox);
            }
            return cell;
        }

        private static string HämtaMånadsnamn(DateTime dag)
        {
            switch (dag.Month)
            {
                case 1: return "Jan";
                case 2: return "Feb";
                case 3: return "Mars";
                case 4: return "April";
                case 5: return "Maj";
                case 6: return "Juni";
                case 7: return "Juli";
                case 8: return "Aug";
                case 9: return "Sept";
                case 10: return "Okt";
                case 11: return "Nov";
                case 12: return "Dec";
                default: return "";
            }
        }
        protected string BryggplatsId()
        {
            return Request.QueryString["bryggplats"];
        }
        protected void TillbakaKnappClick(object sender, EventArgs e)
        {
            KontrolleraSession();
            Response.Redirect("Default.aspx?sida=medlemssida&id=" + Request.QueryString["id"]);
        }
        protected void SparaKnappClick(object sender, EventArgs e)
        {
            KontrolleraSession();

            var datumLista = HämtaMarkeradeDatum();
            var ledigaBryggplatser = FilHanterare.Läs<LedigBryggplats>(new LedigBryggplats().FilNamn()).ToList();
            ledigaBryggplatser.RemoveAll(_ => _.BryggplatsId == BryggplatsId() && _.Dag < DateTime.Today);
            ledigaBryggplatser.AddRange(from i in datumLista select new LedigBryggplats{BryggplatsId = BryggplatsId(), Dag = i});
            FilHanterare.Spara(ledigaBryggplatser, new LedigBryggplats().FilNamn());

            LaddaKalender();
        }
        private Medlem KontrolleraSession()
        {
            var obj = Cache[Request.QueryString["id"]];
            if (obj == null)
                Response.Redirect("Default.aspx");
            return (Medlem)obj;
        }

        private IEnumerable<DateTime> HämtaMarkeradeDatum()
        {
            var datumLista = new List<DateTime>();
            var form = Request.Form;
            foreach (string key in form.Keys)
            {
                if(key.Contains(":CD") && key.Length == 12)
                    datumLista.Add(new DateTime(DateTime.Today.Year, 
                                  Convert.ToInt32(key.Substring(8, 2)), 
                                  Convert.ToInt32(key.Substring(10, 2))));
            }
            return datumLista;
        }
    }
}