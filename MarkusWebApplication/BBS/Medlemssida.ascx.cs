﻿using System;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using MarkusModel;

namespace BBS
{
    public partial class Medlemssida : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var medlem = KontrolleraSession();

            var bryggplatser = MedlemsRegister.HämtaAllaBryggplatserFörMedlem(medlem).ToList();
            
            var first = true;
            foreach (var bryggplats in bryggplatser)
            {
                var row = new TableRow();
                row.Controls.Add(new TableCell{Text = first ? "Bryggplatser" : ""});
                first = false;
                row.Controls.Add(new TableCell{Text = bryggplats.Id + " (" + bryggplats.Båt + ")"});
                var url = "Default.aspx?sida=medlemskalender&id=" + Request.QueryString["id"] + "&bryggplats=" + bryggplats.Id;
                var button = new HyperLink
                    {
                        Text = "Kalender"
                    };
                button.Attributes["href"] = url;
                var cell = new TableCell();
                cell.Controls.Add(button);
                row.Controls.Add(cell);
                bryggplatsLista.Controls.Add(row);
            }
        }
        protected void LoggaUtKnappClick(object sender, EventArgs e)
        {
            Cache.Remove(Request.QueryString["id"]);
            Response.Redirect("Default.aspx");
        }
        protected void SparaKnappClick(object sender, EventArgs e)
        {
            KontrolleraSession();
            if (nyttLösenord.Text.Trim().Length <= 2 || nyttLösenord.Text.Trim() != nyttLösenordIgen.Text.Trim())
            {
                feltext.Text = "För kort lösenord, eller så är det inte samma om angetts i båda rutorna";
                return;
            }

            var medlem = KontrolleraSession();
            medlem.Lösenord = nyttLösenord.Text.Trim();
            MedlemsRegister.UppdateraMedlem(medlem);
            feltext.Text = "Nya lösenordet sparat!";
            feltext.ForeColor = Color.Black;
            nyttLösenord.Text = "";
            nyttLösenordIgen.Text = "";
        }
        private Medlem KontrolleraSession()
        {
            var obj = Cache[Request.QueryString["id"]];
            if (obj == null)
                Response.Redirect("Default.aspx");
            return (Medlem)obj;
        }
        protected void LedigaPlatserKnappClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?sida=ledigaplatser&id=" + Request.QueryString["id"]);
        }
        protected void MedlemmarClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?sida=medlemmar&id=" + Request.QueryString["id"]);
        }
    }
}