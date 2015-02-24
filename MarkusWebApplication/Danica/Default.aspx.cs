using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MarkusModel;

namespace Danica
{
    public partial class Default : System.Web.UI.Page
    {
        private const string Format = "# ### ##0";

        protected string BankPie;
        protected string MäklarePie;
        protected string KryssPie;

        protected string DepåPie;
        protected string FondPie;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack) return;

            var cookie = Request.Cookies["userNameDanica"];
            if (cookie == null)
                Response.Redirect("Login.aspx");
            Response.Cookies.Add(MarkusWebApplication.Danica.SkapaNyCookie());
           

            var statistik = FilHanterare.Läs<ClaesStatistik>(@"c:\data\statistik.txt").ToList().FirstOrDefault();
            if (statistik == null) return;

            var premier = statistik.Premier.First(_ => _.ÅretsVärde);
            var förraÅretsPremier = statistik.Premier.First(_ => _.ÅretsVärde == false);
            var värde = statistik.Värde;

            SkrivTillTabell(premier, förraÅretsPremier, statistik.FörraÅretsTotalPremie);
            SkrivTillBarer(premier);
            SkrivStekarna(statistik);
            if(värde != null)
                SkrivVärdeTillBarer(värde);
        }

        private void SkrivStekarna(ClaesStatistik statistik)
        {
            if (statistik.Stekar == null)
            {
                stekarrubrik.Visible = false;
                stekarlinje.Visible = false;
                return;
            }

            foreach (var stek in statistik.Stekar)
            {
                var förstaraden = new HtmlTableRow();
                var belopp = new HtmlTableCell {InnerText = stek.Belopp.ToString("# ### ##0") + "kr"};
                belopp.Style.Add("font-size", "14px");
                förstaraden.Cells.Add(belopp);
                förstaraden.Cells.Add(new HtmlTableCell { InnerText = stek.Mäklare });
                stekar.Rows.Add(förstaraden);
                var andraraden = new HtmlTableRow();
                andraraden.Cells.Add(new HtmlTableCell { InnerText = stek.Försäkring });
                andraraden.Cells.Add(new HtmlTableCell { InnerText = stek.Mäklarbolag });
                stekar.Rows.Add(andraraden);
                var mellanrum = new HtmlTableRow();
                mellanrum.Cells.Add(new HtmlTableCell { Height = "10px" });
                stekar.Rows.Add(mellanrum);
            }
        }

        private void SkrivTillBarer(ÅrsPremier premier)
        {
            BankPie = Math.Round(premier.BankPremier * 0.000001, 0, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");
            KryssPie = Math.Round(premier.Kryss * 0.000001, 0, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");
            MäklarePie = Math.Round(premier.MäklarPremier * 0.000001, 0, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");
            FondPie = Math.Round(premier.FondPremier * 0.000001, 0, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");
            DepåPie = Math.Round(premier.DepåPremier * 0.000001, 0, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");
        }

        private void SkrivVärdeTillBarer(Värde värde)
        {
            var depåVidd = Math.Round(värde.Depå / värde.Totalt * 100, 0, MidpointRounding.AwayFromZero);
            var fondVidd = Math.Round(värde.Fond / värde.Totalt * 100, 0, MidpointRounding.AwayFromZero);
            volymDepå.Width = depåVidd.ToString(CultureInfo.InvariantCulture) + "%";
            volymDepå.InnerText = "Depå " + volymDepå.Width;
            volymFond.Width = fondVidd.ToString(CultureInfo.InvariantCulture) + "%";
            volymFond.InnerText = "Fond " + volymFond.Width;
            var kryssVidd = 100 - depåVidd - fondVidd;
            volymKryss.Width = kryssVidd.ToString(CultureInfo.InvariantCulture) + "%";
            volymKryss.InnerText = "Kryss " + volymKryss.Width;
        }

        private void SkrivTillTabell(ÅrsPremier premier, ÅrsPremier förraÅretsPremier, double förraÅretsTotalaPremie)
        {
            bankDepå.Text = SkrivVärde(premier.DepåBank, förraÅretsPremier.DepåBank);
            bankFond.Text = SkrivVärde(premier.FondBank, förraÅretsPremier.FondBank);
            mäklareDepå.Text = SkrivVärde(premier.DepåMäklare, förraÅretsPremier.DepåMäklare);
            mäklareFond.Text = SkrivVärde(premier.FondMäklare, förraÅretsPremier.FondMäklare);
            kryss.Text = SkrivVärde(premier.Kryss, förraÅretsPremier.Kryss);
            depå.Text = SkrivVärde(premier.DepåPremier, förraÅretsPremier.DepåPremier);
            fond.Text = SkrivVärde(premier.FondPremier + premier.Kryss, förraÅretsPremier.FondPremier + förraÅretsPremier.Kryss);
            summaBank.Text = SkrivVärde(premier.BankPremier, förraÅretsPremier.BankPremier);
            summaMäklare.Text = SkrivVärde(premier.MäklarPremier, förraÅretsPremier.MäklarPremier);
            summaKryss.Text = SkrivVärde(premier.Kryss, förraÅretsPremier.Kryss);
            summa.Text = SkrivVärde(premier.TotalaPremier, förraÅretsPremier.TotalaPremier);
            estimat.Text = (premier.TotalaPremier / förraÅretsPremier.TotalaPremier * förraÅretsTotalaPremie * 0.000001).ToString(Format).Trim();
        }

        private static string SkrivVärde(double d1, double d2)
        {
            return (d1*0.000001).ToString(Format).Trim() + " (" + (d2*0.000001).ToString(Format).Trim() + ")";
        }
    }
}
