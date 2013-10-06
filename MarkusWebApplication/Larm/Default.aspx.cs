using System;
using System.Collections.Generic;
using System.Linq;
using MarkusModel;

namespace Larm
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var larm = FilHanterare.Läs<MarkusModel.Larm>(@"c:\data\larm.txt").FirstOrDefault();
            SättKnappar(larm != null && larm.Aktiverat);
        }

        private void SättKnappar(bool aktiverat)
        {
            if(aktiverat)
            {
                Aktivera.Enabled = false;
                Avaktivera.Enabled = true;
                Meddelandetext.Text = "Larmet är PÅ";
                Meddelandetext.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Aktivera.Enabled = true;
                Avaktivera.Enabled = false;
                Meddelandetext.Text = "Larmet är AV";
                Meddelandetext.ForeColor = System.Drawing.Color.Green;
            }
        }

        protected void Aktivera_Click(object sender, EventArgs e)
        {
            Spara(true);
        }
        protected void Avaktivera_Click(object sender, EventArgs e)
        {
            Spara(false);
        }
        private void Spara(bool aktiverat)
        {
            FilHanterare.Spara(new List<MarkusModel.Larm> { new MarkusModel.Larm { Aktiverat = aktiverat } }, @"c:\data\larm.txt");
            SättKnappar(aktiverat);
        }
        protected void Uppdatera_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
}
}