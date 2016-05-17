using System;
using System.Linq;
using System.Web.Script.Serialization;

namespace MarkusConsoleApplication
{
    static class Program
    {
        static void Main(string[] args)
        {
            if(args.Any(_ => _.Equals("BBS")))
                LäsInMedlemmar.Kör();
            else if (args.Any(_ => _.Equals("UppdateraKalender")))
            {
                var kalender = new MarkusModel.Kalender();
                kalender.SkapaKalendrarFörMonteRojo();
                kalender.SkapaKalendrarFörRyda();
            }
            else if (args.Any(_ => _.Equals("test")))
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
            else if (args.Any(_ => _.Equals("json")))
            {
                var o = new TestKlass{Datum = DateTime.Now, Double = 123.456, Sträng = "Markus Linderbäck"};
                var ser = new JavaScriptSerializer();
                Console.WriteLine(ser.Serialize(o));
            }
            else if(args.Any(_ => _.Equals("PDF")))
            {
                TestaLäsaPdf();
            }
        }

        public static void TestaLäsaPdf()
        {
            var reader = new iTextSharp.text.pdf.PdfReader(@"c:\dokument\test.pdf");
            foreach(var field in reader.AcroFields.Fields)
            {
                var key = field.Key;
                var value = reader.AcroFields.GetField(key);
                Console.WriteLine(key + ": " + value);
            }
            var stream = new System.IO.MemoryStream();
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, stream) { FormFlattening = true };
            stamper.Close();

            var hashcode = reader.GetHashCode();
        }
    }
    public class TestKlass
    {
        public string Sträng;
        public DateTime Datum;
        public double Double;
    }
}
