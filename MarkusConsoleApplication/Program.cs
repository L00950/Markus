using System;
using System.Linq;
using ConsoleApplication;
using System.Web.Script.Serialization;

namespace MarkusConsoleApplication
{
    static class Program
    {
        static void Main(string[] args)
        {
            if(args.Any(_ => _.Equals("BBS")))
                LäsInMedlemmar.Kör();
            if (args.Any(_ => _.Equals("UppdateraKalender")))
                UppdateraKalendern.Kör();
            if (args.Any(_ => _.Equals("test")))
            {
                try
                {
                    var smtp = Gmail.TeliaSmtpKlient();
                    smtp.Send("markus.linderback@telia.com", "markus.linderback@danica.se", "Test", "Hej!");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (args.Any(_ => _.Equals("json")))
            {
                var o = new TestKlass{Datum = DateTime.Now, Double = 123.456, Sträng = "Markus Linderbäck"};
                var ser = new JavaScriptSerializer();
                Console.WriteLine(ser.Serialize(o));
            }
        }
    }
    public class TestKlass
    {
        public string Sträng;
        public DateTime Datum;
        public double Double;
    }
}
