using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MarkusModel;

namespace BBS
{
    public partial class LedigaPlatser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LaddaKalender();
        }

        private void LaddaKalender()
        {
            var ledigaDatum =
                FilHanterare.Läs<LedigBryggplats>(new LedigBryggplats().FilNamn()).ToList();

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

        private TableCell SkapaCell(int månad, int dag, IEnumerable<LedigBryggplats> ledigaDatum)
        {
            var cell = new TableCell();
            if (dag <= DateTime.DaysInMonth(DateTime.Today.Year, månad))
            {
                var datum = new DateTime(DateTime.Today.Year, månad, dag);
                var ledigaBryggplatser = ledigaDatum as IList<LedigBryggplats> ?? ledigaDatum.ToList();
                var antal = ledigaBryggplatser.Count(_ => _.Dag == datum);
                cell.ForeColor = datum.DayOfWeek == DayOfWeek.Sunday
                                     ? System.Drawing.Color.Red
                                     : System.Drawing.Color.Black;
                var text = new Label {Text = datum.Day + " " + HämtaMånadsnamn(datum) + " "};
                if (antal == 0)
                    text.Text += "0st";
                cell.Controls.Add(text);
                if (antal > 0)
                {
                    var länk = new HyperLink
                        {
                            NavigateUrl = "Default.aspx?sida=ledigplats&id=" + Request.QueryString["id"] + "&datum=" + datum.ToString("yyyy-MM-dd"),
                            Text = antal + "st"
                        };
                    cell.Controls.Add(länk);
                }
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
        protected void TillbakaKnappClick(object sender, EventArgs e)
        {
            KontrolleraSession();
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