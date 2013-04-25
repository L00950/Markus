using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MarkusModel;

namespace BBS
{
    public partial class NyttLösenord : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void SkickaClick(object sender, EventArgs e)
        {
            if (email.Text == "")
            {
                infoText.Text = "Ange e-postadress";
                infoText.ForeColor = Color.Red;
                return;
            }

            var medlem = FilHanterare.Läs<Medlem>(new Medlem().FilNamn()).FirstOrDefault(_ => _.Email != null && _.Email.Trim().ToUpper() == email.Text.Trim().ToUpper());
            if (medlem == null)
            {
                infoText.Text = "E-postadress saknas i BBS register";
                infoText.ForeColor = Color.Red;
                return;
            }
            var nyttLösen = new Random(DateTime.Now.Millisecond).Next(1000, 9999);
            medlem.Lösenord = nyttLösen.ToString(CultureInfo.InvariantCulture);
            MedlemsRegister.UppdateraMedlem(medlem);
            string errormessage;
            new MarkusService().SendMail("B51137", "jtk001", email.Text.Trim(), "Lösenord BBS", "Hej,\n\nAnvändarnamn: <din e-postadress>\nLösenord: " + medlem.Lösenord + "\n", new List<MarkusService.Bilaga>().ToArray(), out errormessage);

            infoText.Text = "Nytt lösenord skickat till angiven e-post";
            infoText.ForeColor = Color.Black;
        }
        protected void TillbakaKnappClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}