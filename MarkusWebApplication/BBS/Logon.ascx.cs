using System;
using System.Linq;
using System.Web.Caching;
using MarkusModel;

namespace BBS
{
    public partial class Logon : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void LoggaInKnappClick(object sender, EventArgs e)
        {
            var medlemmar = MedlemsRegister.HämtaAllaMedlemmar().ToList();
            var medlem = medlemmar.FirstOrDefault(_ => _.Email != null && _.Lösenord != null && _.Email.ToUpper() == användare.Text.Trim().ToUpper() && _.Lösenord.ToUpper() == lösenord.Text.Trim().ToUpper());
            var id = Guid.NewGuid().ToString();
            if (medlem != null)
            {
                Cache.Add(id, medlem, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Normal, (key, value, reason) => { });
                Response.Redirect("Default.aspx?sida=medlemssida&id=" + id);
            }
        }
        protected void NyttLösenKnappClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?sida=nyttlosenord");
        }
    }
}