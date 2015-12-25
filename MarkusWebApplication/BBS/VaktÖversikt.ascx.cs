using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MarkusModel;

namespace BBS
{
    public partial class VaktÖversikt : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            månad.SelectedValue = Request.QueryString["month"];

            LaddaVaktgång();
        }

        private void LaddaVaktgång()
        {
            var filer = MedlemsRegister.HämtaVaktLoggFiler();
            var loggposter = new List<Loggpost>();
            foreach (var fil in filer)
                loggposter.AddRange(MedlemsRegister.LäsVaktLogg(fil));


            for (var i = 1; i <= 31; i++)
            {
                var rad = new TableRow();

                rad.Cells.Add(SkapaCell(Convert.ToInt32(Request.QueryString["month"]), i, loggposter));

                tabell.Rows.Add(rad);
            }
        }

        private IEnumerable<Loggpost> HämtaFörDatum(IEnumerable<Loggpost> loggposter, DateTime datum)
        {
            var starttid = new DateTime(datum.Year, datum.Month, datum.Day, 18, 0, 0);
            var sluttid = starttid.AddDays(1);
            return loggposter.Where(_ => _.Tidpunkt >= starttid && _.Tidpunkt < sluttid);
        }

        private TableCell SkapaCell(int månad, int dag, IEnumerable<Loggpost> loggposter)
        {
            var cell = new TableCell();
            if (dag <= DateTime.DaysInMonth(DateTime.Today.Year, månad))
            {
                var datum = new DateTime(DateTime.Today.Year, månad, dag);
                var loggposterFörDatum = HämtaFörDatum(loggposter, datum).ToList();
                var antal = loggposterFörDatum.Count();
                cell.ForeColor = datum.DayOfWeek == DayOfWeek.Sunday
                                     ? System.Drawing.Color.Red
                                     : System.Drawing.Color.Black;
                var text = new Label {Text = datum.Day + " " + HämtaMånadsnamn(datum) + " "};
                if (antal == 0)
                    text.Text += "";
                cell.Controls.Add(text);
                if (antal > 0)
                {
                    cell.Controls.Add(new Label{Text = loggposterFörDatum.Min(_ => _.Tidpunkt).ToString("HH:mm") + "-" + loggposterFörDatum.Max(_ => _.Tidpunkt).ToString("HH:mm") + " ("});
                    var länk = new HyperLink
                        {
                            NavigateUrl = "Default.aspx?sida=vaktloggfordatum&id=" + Request.QueryString["id"] + "&datum=" + datum.ToString("yyyy-MM-dd") + "&month=" + this.månad.SelectedValue,
                            Text = antal.ToString()
                        };
                    cell.Controls.Add(länk);
                    cell.Controls.Add(new Label { Text = ")" });
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
        private Medlem KontrolleraSession()
        {
            var obj = Cache[Request.QueryString["id"]];
            if (obj == null)
                Response.Redirect("Default.aspx");
            return (Medlem)obj;
        }
        protected void TillbakaKnappClick(object sender, EventArgs e)
        {
            KontrolleraSession();
            Response.Redirect("Default.aspx?sida=medlemssida&id=" + Request.QueryString["id"]);
        }
        protected void månad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?sida=vakt&id=" + Request.QueryString["id"] + "&month=" + månad.SelectedValue);
        }
    }
}