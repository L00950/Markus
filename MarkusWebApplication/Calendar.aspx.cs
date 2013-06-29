using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MarkusModel;


namespace MarkusWebApplication
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		protected HtmlTableRow Newbuttonrow;
		protected HyperLink HyperLink1;
		protected HyperLink Hyperlink2;
	
		protected void Page_Load(object sender, EventArgs e)
		{
		    if (IsPostBack) return;

		    // Initiera vilket hus man håller på med = 0
		    ViewState["object"] = 0;
		    // Fyll datagriden
		    FillBookings();
		    bookingsrow.Visible = false;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.bookings.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.BookingsEditCommand);
			this.bookings.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.BookingsDeleteCommand);

            this.deletebutton.Attributes.Add("onclick", "return confirm('Vill Du ta bort?');");

		}
		#endregion

		protected void NewbuttonServerClick(object sender, EventArgs e)
		{
			newbookingrow.Visible = true;
			bookingsrow.Visible = false;
			id.Value = "";
			newbookinglabel.Text = "Ny bokning";
			CalendarStart.SelectedDate = DateTime.Now;
			CalendarEnd.SelectedDate = DateTime.Now;
			person.Value = "";
			note.Value = "";
		}

		protected void CancelbuttonServerClick(object sender, EventArgs e)
		{
			bookingsrow.Visible = true;
			newbookingrow.Visible = false;			
		}

        protected void DeletebuttonServerClick(object sender, EventArgs e)
        {
            newbookingrow.Visible = false;
            bookingsrow.Visible = true;

            if (String.IsNullOrEmpty(id.Value)) return;

            HanteraBokningar.TaBortBokning(Convert.ToInt32(id.Value));

            // Fyll datagriden
            FillBookings();

            // Uppdatera websidor
            UpdateWeb();
        }

		protected void SavebuttonServerClick(object sender, EventArgs e)
		{
			bookingsrow.Visible = true;
			newbookingrow.Visible = false;

			// Spara ny
            if (id.Value == "")
                HanteraBokningar.SparaNyBokning(new Bokning
                                                    {
                                                        Ankomst = CalendarStart.SelectedDate,
                                                        Avresa = CalendarEnd.SelectedDate,
                                                        Hus = (Hus)Convert.ToInt32(ViewState["object"]),
                                                        Kommentar = note.Value,
                                                        Person = person.Value
                                                    });
            else
            {
                var bokning = HanteraBokningar.HämtaBokning(Convert.ToInt32(id.Value));
                bokning.Ankomst = CalendarStart.SelectedDate;
                bokning.Avresa = CalendarEnd.SelectedDate;
                bokning.Kommentar = note.Value;
                bokning.Person = person.Value;
                HanteraBokningar.UppdateraBokning(bokning);
            }

		    // Fyll datagriden
			FillBookings();

			// Uppdatera websidor
			UpdateWeb();
		}

		private void BookingsEditCommand(object source, DataGridCommandEventArgs e)
		{
			var bokningsId = Convert.ToInt32(bookings.Items[e.Item.ItemIndex].Cells[0].Text);
			newbookingrow.Visible = true;
			bookingsrow.Visible = false;
			var bokning = HanteraBokningar.HämtaBokning(bokningsId);
			id.Value = bokningsId.ToString();
			newbookinglabel.Text = "Uppdatera bokning";

			CalendarStart.VisibleDate = bokning.Ankomst;
			CalendarStart.SelectedDate = bokning.Ankomst;
			CalendarEnd.VisibleDate = bokning.Avresa;
			CalendarEnd.SelectedDate = bokning.Avresa;

			person.Value = bokning.Person;
			note.Value = bokning.Kommentar;
		}

		private void BookingsDeleteCommand(object source, DataGridCommandEventArgs e)
		{
            HanteraBokningar.TaBortBokning(Convert.ToInt32(bookings.Items[e.Item.ItemIndex].Cells[0].Text));

			// Fyll datagriden
			FillBookings();

			// Uppdatera websidor
			UpdateWeb();
		}

		protected void LoginServerClick(object sender, EventArgs e)
		{
			ViewState["userid"] = HanteraBokningar.Login(username.Value, password.Value);
			if(Convert.ToInt32(ViewState["userid"]) > 0)
			{
			    HämtafilInformation();
				loginrow.Visible = false;
				overviewrow.Visible = true;
				loginmessage.InnerText = "";
			}
			else
				loginmessage.InnerText = "Fel username eller password!";
		}

		protected void LogoutServerClick(object sender, EventArgs e)
		{
			ViewState["userid"] = 0;
			loginrow.Visible = true;
			overviewrow.Visible = false;
			username.Value = "";
			password.Value = "";
		}

        private void HämtafilInformation()
        {
            var filInfo = HanteraBokningar.FilInformation();
            antalBokningar.Text = HanteraBokningar.AntalBokningar().ToString("# ##0");
            dataFil.Text = filInfo.FullName;
            filStorlek.Text = filInfo.Length.ToString("# ### ### ##0") + " bytes";
            senastSparad.Text = filInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
		private void UpdateWeb()
		{
            new Kalender(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["firstyear"])).CreateCalendar(Convert.ToInt32(ViewState["object"]));
		}
		private void FillBookings()
		{
            switch (Convert.ToInt32(ViewState["object"]))
            {
                case 1: bokningar.InnerText = "Bokningar Lilla Huset"; break;
                case 2: bokningar.InnerText = "Bokningar Ett Tvåan"; break;
                case 5: bokningar.InnerText = "Bokningar Hebbes Hus"; break;
                case 6: bokningar.InnerText = "Bokningar Monte Rojo"; break;
            }
			see2006.Attributes.Add("onclick", "showModalDialog('http://linderback.com/linderback/swe/" + ViewState["object"] + "/bokat1.htm' , 0, 'dialogWidth:625px;dialogHeight:600px')");
			see2007.Attributes.Add("onclick", "showModalDialog('http://linderback.com/linderback/swe/" + ViewState["object"] + "/bokat2.htm' , 0, 'dialogWidth:625px;dialogHeight:600px')");
		    
            if (Convert.ToInt32(ViewState["object"]) == 0) return;

		    var onjektsbokningar = HanteraBokningar.HämtaBokningar((Hus) Convert.ToInt32(ViewState["object"]));
		    if (!sehistorik.Checked)
                onjektsbokningar = onjektsbokningar.Where(_ => _.Avresa >= DateTime.Today);
            bookings.DataSource = onjektsbokningar;
		    bookings.DataBind();
		}

		protected void LillahusetServerClick(object sender, EventArgs e)
		{
			ViewState["object"] = 1;
			FillBookings();
			overviewrow.Visible = false;
			bookingsrow.Visible = true;
		}

		protected void EttvaanServerClick(object sender, EventArgs e)
		{
			ViewState["object"] = 2;
            FillBookings();
            overviewrow.Visible = false;
			bookingsrow.Visible = true;
		}

        protected void HebbesServerClick(object sender, EventArgs e)
        {
            ViewState["object"] = 5;
            FillBookings();
            overviewrow.Visible = false;
            bookingsrow.Visible = true;
        }

        protected void MonterojoServerClick(object sender, EventArgs e)
        {
            ViewState["object"] = 6;
            FillBookings();
            overviewrow.Visible = false;
            bookingsrow.Visible = true;
        }

        protected void SäkerhetskopieraServerClick(object sender, EventArgs e)
        {
            var filnamn = HanteraBokningar.Säkerhetskopiera();
            statusText.Text = "Säkerhetskopiering har skett till " + filnamn;
        }

        protected void GobackServerClick(object sender, EventArgs e)
        {
            ViewState["object"] = 2;
            FillBookings();
            bookingsrow.Visible = false;
            HämtafilInformation();
            overviewrow.Visible = true;
        }
        protected void CalendarStartSelectionChanged(object sender, EventArgs e)
        {
            JusteraFramSlutdatumTillStartdatum();
        }
        protected void CalendarEndSelectionChanged(object sender, EventArgs e)
        {
            JusteraFramSlutdatumTillStartdatum();
        }
        private void JusteraFramSlutdatumTillStartdatum()
        {
            if (CalendarEnd.SelectedDate >= CalendarStart.SelectedDate) return;

            CalendarEnd.SelectedDate = CalendarStart.SelectedDate;
            CalendarEnd.VisibleDate = CalendarEnd.SelectedDate;
        }
        protected void SeHistorikCheckedChanged(object sender, EventArgs e)
        {
            FillBookings();
        }
    }
}
