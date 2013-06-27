using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    static class UppdateraKalendern
    {
        public static void Kör()
        {
            new MarkusModel.Kalender(DateTime.Today.Year).CreateCalendar(6);
        }
    }
}
