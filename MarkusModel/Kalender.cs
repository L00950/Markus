using System;
using System.Collections.Generic;

namespace MarkusModel
{
	public class Kalender
	{
        public Kalender(int firstyear)
        {
            _firstyear = firstyear;
            _secondyear = firstyear + 1;
        }
	    private readonly int _firstyear;
	    private readonly int _secondyear;
	    private List<DateTime> _bokadeDatum;

		public void CreateCalendar(int objectid)
		{
            // Hämta bokade datum
            _bokadeDatum = new List<DateTime>();
		    var bokningar = HanteraBokningar.HämtaBokningar((Hus) objectid);
		    foreach (var bokning in bokningar)
		    {
		        var tmpDate = bokning.Ankomst;
                while(tmpDate <= bokning.Avresa)
                {
                    if(!_bokadeDatum.Contains(tmpDate))
                        _bokadeDatum.Add(tmpDate);
                    tmpDate = tmpDate.AddDays(1);
                }
		    }

			// Första året
			for(var i=1;i<=12;i++)
			{
                CreateMonth(objectid, _firstyear, i);
			}
			// Andra året
			for(var i=1;i<=12;i++)
			{
                CreateMonth(objectid, _secondyear, i);
			}
		}
		private void CreateMonth(int objectid, int year, int month)
		{
			var cdtDate = new DateTime(year, month, 1);
			while(cdtDate.DayOfWeek != DayOfWeek.Monday)
				cdtDate = cdtDate.AddDays(-1);
		    var filename = @"c:\inetpub\wwwroot\linderback\" + year + @"\" + objectid + @"\" + month + ".asp";
			// Ta bort gammla fil
			if(System.IO.File.Exists(filename))
				System.IO.File.Delete(filename);

			// Skapa ny fil
			var writer = new System.IO.StreamWriter(filename);
			writer.WriteLine(GetBefore(month));
			string szTmp;
			CreateWeek(objectid, ref cdtDate, month, out szTmp);
			writer.WriteLine(szTmp);
			while(cdtDate.Month == month)
			{
				CreateWeek(objectid, ref cdtDate, month, out szTmp);
				writer.WriteLine(szTmp);
			}
			writer.WriteLine(GetAfter());
			writer.Close();
		}
	

		private void CreateWeek(int objectid, ref DateTime cdtStartDate, int nMonth, out string szReturn)
		{
			szReturn = "";
			szReturn += "<tr>";
			for(var i=0;i<7;i++)
			{
				if(cdtStartDate.Month == nMonth)
				{
					string szTmp;
                    if (cdtStartDate < DateTime.Today && objectid == 6)
                        szTmp = "<td bgcolor=grey>" + cdtStartDate.Day + "</td>";
					else if(_bokadeDatum.Contains(cdtStartDate))
						szTmp = "<td bgcolor=yellow>" + cdtStartDate.Day + "</td>";
					else
						szTmp = "<td>" + cdtStartDate.Day + "</td>";
					szReturn += szTmp;
				}
				else
				{
					szReturn += "<td></td>";
				}
				cdtStartDate = cdtStartDate.AddDays(1);
			}
			szReturn += "</tr>";
		}

		private static string GetBefore(int month)
		{
			string strmonth;
			switch(month)
			{
				case 1:
					strmonth = "January";
					break;
				case 2:
					strmonth = "February";
					break;
				case 3:
					strmonth = "March";
					break;
				case 4:
					strmonth = "April";
					break;
				case 5:
					strmonth = "May";
					break;
				case 6:
					strmonth = "June";
					break;
				case 7:
					strmonth = "July";
					break;
				case 8:
					strmonth = "August";
					break;
				case 9:
					strmonth = "September";
					break;
				case 10:
					strmonth = "October";
					break;
				case 11:
					strmonth = "November";
					break;
				case 12:
					strmonth = "December";
					break;
				default:
					strmonth = "";
					break;
			}
		    var szTmp = "<HTML>\n"+
		                   "<HEAD>\n"+
		                   "<META NAME=\"GENERATOR\" Content=\"Microsoft Visual Studio 6.0\">\n"+
		                   "<META http-equiv=\"pragma\" content=\"no-cache\">\n"+
		                   "<TITLE></TITLE>\n"+
		                   "<STYLE>\n"+
		                   "table, body\n"+
		                   "{\n"+
		                   "    FONT-FAMILY: Verdana;\n"+
		                   "    FONT-SIZE: 8pt;\n"+
		                   "}\n"+
		                   "td\n"+
		                   "{\n"+
		                   "	TEXT-ALIGN: center;\n"+
		                   "}\n"+
		                   "</STYLE>\n"+
		                   "</HEAD>\n"+
		                   "<body bgcolor=\"white\" bottomMargin=0 topmargin=0 rightmargin=0 leftmargin=0>\n"+
		                   "<table>\n"+
		                   "<tr>\n"+
		                   "<td colspan=8 style=\"font-size:20px;\">\n"+
		                   strmonth + "\n"+
		                   "</td>\n"+
		                   "</tr>\n"+
		                   "<tr>\n"+
		                   "<td width=15>\n"+
		                   "Mo\n"+
		                   "</td>\n"+
		                   "<td width=15>\n"+
		                   "Tu\n"+
		                   "</td>\n"+
		                   "<td width=15>\n"+
		                   "We\n"+
		                   "</td>\n"+
		                   "<td width=15>\n"+
		                   "Th\n"+
		                   "</td>\n"+
		                   "<td width=15>\n"+
		                   "Fr\n"+
		                   "</td>\n"+
		                   "<td width=15>\n"+
		                   "Sa\n"+
		                   "</td>\n"+
		                   "<td width=15>\n"+
		                   "Su\n"+
		                   "</td>\n"+
		                   "</tr>";
			return szTmp;
		}
		private static string GetAfter()
		{
			return "</table>\n</body>\n</HTML>";
		}
	}
}
