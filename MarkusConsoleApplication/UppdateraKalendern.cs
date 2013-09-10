using System;

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
