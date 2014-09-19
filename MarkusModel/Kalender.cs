using System;
using System.Collections.Generic;

namespace MarkusModel
{
	public class Kalender
	{
        public Kalender()
        {
        }
	    private List<DateTime> _bokadeDatum;

        private void HämtaBokadeDatum(int objectid)
        {
            _bokadeDatum = new List<DateTime>();
            var bokningar = HanteraBokningar.HämtaBokningar((Hus)objectid);
            foreach (var bokning in bokningar)
            {
                var tmpDate = bokning.Ankomst;
                while (tmpDate <= bokning.Avresa)
                {
                    if (!_bokadeDatum.Contains(tmpDate))
                        _bokadeDatum.Add(tmpDate);
                    tmpDate = tmpDate.AddDays(1);
                }
            }
        }

	    public void SkapaKalendrarFörObjekt(int objectid)
	    {
            const string mall = @"C:\Development\Markus\MarkusWeb\MonteRojo\default_mall.htm";
            var reader = new System.IO.StreamReader(mall);
            var innehåll = reader.ReadToEnd();
            reader.Close();
            innehåll = innehåll.Replace("#kalender1#", SkapaÅr(objectid, DateTime.Today.Year));
            innehåll = innehåll.Replace("#kalender2#", SkapaÅr(objectid, DateTime.Today.Year + 1));

            const string fil = @"C:\Development\Markus\MarkusWeb\MonteRojo\default.asp";
            if (System.IO.File.Exists(fil))
                System.IO.File.Delete(fil);

            var writer = new System.IO.StreamWriter(fil);
            writer.Write(innehåll);
            writer.Close();
	    }
		public void CreateCalendar(int objectid)
		{
            HämtaBokadeDatum(objectid);

			// Första året
			for(var i=1;i<=12;i++)
			{
                SkapaMånadTillFil(objectid, DateTime.Today.Year, i);
			}
			// Andra året
			for(var i=1;i<=12;i++)
			{
                SkapaMånadTillFil(objectid, DateTime.Today.Year + 1, i);
			}
		}

        private void SkapaMånadTillFil(int objectid, int year, int month)
		{
			var cdtDate = new DateTime(year, month, 1);
			while(cdtDate.DayOfWeek != DayOfWeek.Monday)
				cdtDate = cdtDate.AddDays(-1);
            var filename = @"C:\Development\Markus\MarkusWeb\Linderback\" + year + @"\" + objectid + @"\" + month + ".asp";
			// Ta bort gammla fil
			if(System.IO.File.Exists(filename))
				System.IO.File.Delete(filename);

			// Skapa ny fil
			var writer = new System.IO.StreamWriter(filename);
			writer.WriteLine(SkapaDataFöreFörFil(month));
			string szTmp;
			CreateWeek(objectid, ref cdtDate, month, out szTmp);
			writer.WriteLine(szTmp);
			while(cdtDate.Month == month)
			{
				CreateWeek(objectid, ref cdtDate, month, out szTmp);
				writer.WriteLine(szTmp);
			}
			writer.WriteLine(SkapaDataEfterFörFil());
			writer.Close();
		}

        public string SkapaÅr(int objectid, int år)
        {
            HämtaBokadeDatum(objectid);

            var månad = 1;
            var str = "<table>\n";
            for (var tertial = 0; tertial < 3; tertial++)
            {
                str += "<tr>\n";
                for (var kolumn = 0; kolumn < 4; kolumn++)
                {
                    str += "<td>\n";
                    str += SkapaMånad(objectid, år, månad++);
                    str += "</td>\n";
                }
                str += "</tr>\n";
            }
            str += "</table>\n";
            return str;
        }

        private string SkapaMånad(int objectid, int year, int month)
        {
            var str = "";
            var cdtDate = new DateTime(year, month, 1);
            while (cdtDate.DayOfWeek != DayOfWeek.Monday)
                cdtDate = cdtDate.AddDays(-1);

            // Skapa ny fil
            str += SkapaDataFöre(month);
            string szTmp;
            CreateWeek(objectid, ref cdtDate, month, out szTmp);
            str += szTmp;
            while (cdtDate.Month == month)
            {
                CreateWeek(objectid, ref cdtDate, month, out szTmp);
                str += szTmp;
            }
            str += SkapaDataEfter();

            return str;
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

        private static string Månadsnamn(int månad)
        {
            var strmonth = "";
            switch (månad)
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
            return strmonth;
        }

        private static string SkapaDataFöre(int month)
        {
            var strmonth = Månadsnamn(month);
            var szTmp = "<table style=\"TEXT-ALIGN: center; FONT-FAMILY: Verdana; FONT-SIZE: 8pt\" cellspacing=\"0\" cellpadding=\"0\">\n" +
                           "<tr>\n" +
                           "<td colspan=8 style=\"font-size:20px;\">\n" +
                           strmonth + "\n" +
                           "</td>\n" +
                           "</tr>\n" +
                           "<tr>\n" +
                           "<td width=15>\n" +
                           "Mo\n" +
                           "</td>\n" +
                           "<td width=15>\n" +
                           "Tu\n" +
                           "</td>\n" +
                           "<td width=15>\n" +
                           "We\n" +
                           "</td>\n" +
                           "<td width=15>\n" +
                           "Th\n" +
                           "</td>\n" +
                           "<td width=15>\n" +
                           "Fr\n" +
                           "</td>\n" +
                           "<td width=15>\n" +
                           "Sa\n" +
                           "</td>\n" +
                           "<td width=15>\n" +
                           "Su\n" +
                           "</td>\n" +
                           "</tr>";
            return szTmp;
        }

		private static string SkapaDataFöreFörFil(int month)
		{
		    var strmonth = Månadsnamn(month);
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

        private static string SkapaDataEfter()
        {
            return "</table>\n";
        }

        private static string SkapaDataEfterFörFil()
		{
			return "</table>\n</body>\n</HTML>";
		}
	}
}
