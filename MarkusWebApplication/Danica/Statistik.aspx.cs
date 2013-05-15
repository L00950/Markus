using System;
using System.Globalization;
using System.Linq;
using MarkusModel;

namespace Danica
{
    public partial class Statistik : System.Web.UI.Page
    {
        private const string Format = "# ### ##0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack) return;

            var statistik = FilHanterare.Läs<ClaesStatistik>(@"c:\data\statistik.txt").ToList().FirstOrDefault();
            if (statistik == null) return;

            var premier = statistik.Premier.First(_ => _.ÅretsVärde);
            var förraÅretsPremier = statistik.Premier.First(_ => _.ÅretsVärde == false);

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
            var depåVidd = Math.Round(premier.DepåPremier / premier.TotalaPremier * 100, 0, MidpointRounding.AwayFromZero);
            var fondVidd = Math.Round(premier.FondPremier / premier.TotalaPremier * 100, 0, MidpointRounding.AwayFromZero);
            depåBar.Width = depåVidd.ToString(CultureInfo.InvariantCulture) + "%";
            depåBar.InnerText = "Depå " + depåBar.Width;
            fondBar.Width = fondVidd.ToString(CultureInfo.InvariantCulture) + "%";
            fondBar.InnerText = "Fond " + fondBar.Width;
            var kryssVidd = 100 - depåVidd - fondVidd;
            kryssBar.Width = kryssVidd.ToString(CultureInfo.InvariantCulture) + "%";
            kryssBar.InnerText = "Kryss " + kryssBar.Width;
            var bankVidd = Math.Round(premier.BankPremier / premier.TotalaPremier * 100, 0, MidpointRounding.AwayFromZero);
            bankBar.Width = bankVidd.ToString(CultureInfo.InvariantCulture) + "%";
            bankBar.InnerText = "Bank " + bankBar.Width;
            mäklarBar.Width = (100 - bankVidd - kryssVidd).ToString(CultureInfo.InvariantCulture) + "%";
            mäklarBar.InnerText = "Mäklare " + mäklarBar.Width;
            kryssBar2.Width = kryssVidd.ToString(CultureInfo.InvariantCulture) + "%";
            kryssBar2.InnerText = "Kryss " + kryssBar2.Width;
            estimat.Text = (premier.TotalaPremier * 0.000001 / DateTime.Today.DayOfYear * 365).ToString(Format).Trim();
        }

        private static string SkrivVärde(double d1, double d2)
        {
            return (d1*0.000001).ToString(Format).Trim() + " (" + (d2*0.000001).ToString(Format).Trim() + ")";
        }
    }
}
