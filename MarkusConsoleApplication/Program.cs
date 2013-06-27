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
        }
    }
}
