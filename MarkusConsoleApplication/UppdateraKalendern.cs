using System;

namespace ConsoleApplication
{
    static class UppdateraKalendern
    {
        public static void Kör()
        {
            new MarkusModel.Kalender(DateTime.Today.Year).CreateCalendar(6);
        }

        public static void Skapafil()
        {
            const string mall = @"C:\Development\Markus\MarkusWeb\MonteRojo\default_mall.htm";
            var reader = new System.IO.StreamReader(mall);
            var innehåll = reader.ReadToEnd();
            reader.Close();
            innehåll = innehåll.Replace("#kalender1#", new MarkusModel.Kalender(DateTime.Today.Year).SkapaÅr(6));
            innehåll = innehåll.Replace("#kalender2#", new MarkusModel.Kalender(DateTime.Today.Year+1).SkapaÅr(6));

            const string fil = @"C:\Development\Markus\MarkusWeb\MonteRojo\default.asp";
            if (System.IO.File.Exists(fil))
                System.IO.File.Delete(fil);

            var writer = new System.IO.StreamWriter(fil);
            writer.Write(innehåll);
            writer.Close();
        }
    }
}
