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
            var mall = @"C:\Development\Markus\MarkusWeb\Linderback\mall.htm";
            var reader = new System.IO.StreamReader(mall);
            var innehåll = reader.ReadToEnd();
            reader.Close();
            innehåll = innehåll.Replace("#kalender#", new MarkusModel.Kalender(DateTime.Today.Year).SkapaÅr(6));

            var fil = @"C:\Development\Markus\MarkusWeb\Linderback\default2.htm";
            if (System.IO.File.Exists(fil))
                System.IO.File.Delete(fil);

            var writer = new System.IO.StreamWriter(fil);
            writer.Write(innehåll);
            writer.Close();
        }
    }
}
