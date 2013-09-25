using System;
using System.Linq;
using ConsoleApplication;

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
                    var smtp = Gmail.GmailSmtpKlient();
                    smtp.Send("markus.linderback@gmail.com", "markus.linderback@danica.se", "Test", "Hej!");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
