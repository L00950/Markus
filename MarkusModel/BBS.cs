using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkusModel
{
    [Serializable]
    public class Medlem : IFilKlass
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Lösenord { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Mobil { get; set; }
        public string Gata { get; set; }
        public string PostNummer { get; set; }
        public string Ort { get; set; }

        public string FilNamn()
        {
            return @"c:\data\medlemmar.txt";
        }
    }
    [Serializable]
    public class Bryggplats : IFilKlass
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set 
            { 
                _id = value.Trim();
                _id = _id.Replace(" ", "");
                if (_id.Length == 2)
                    _id = _id.Substring(0, 1) + "0" + _id.Substring(1, 1);
            }
        }
        public int MedlemsId { get; set; }
        public string Båt { get; set; }
        public string FilNamn()
        {
            return @"c:\data\bryggplatser.txt";
        }
    }
    [Serializable]
    public class LedigBryggplats : IFilKlass
    {
        public string BryggplatsId { get; set; }
        public DateTime Dag { get; set; }
        public string FilNamn()
        {
            return @"c:\data\ledigabryggplatser.txt";
        }
    }

    public class Loggpost
    {
        public string Plats;
        public DateTime Tidpunkt;
    }

    public static class MedlemsRegister
    {
        public enum Medlem
        {
            Markus = 580,
            Kjell = 299,
            Gerda = 251,
            Bergholm = 465,
            Andreas = 145,
            AndersW = 498,
            AndersJ = 587,
            Edholm = 399
        }

        public static readonly List<Medlem> Styrelse = new List<Medlem>{Medlem.Markus, Medlem.Kjell, Medlem.Gerda, Medlem.Bergholm, Medlem.AndersJ, Medlem.Andreas, Medlem.AndersW, Medlem.Edholm};
 
        public static IEnumerable<String> HämtaVaktLoggFiler()
        {
            return Directory.GetFiles(@"c:\data\vaktlogg", "*.csv");
        }

        public static List<Loggpost> LäsVaktLogg(string fil)
        {
            var vaktLogg = new List<Loggpost>();
            if (!File.Exists(fil))
                return vaktLogg;

            TextReader reader = File.OpenText(fil);
            var rad = reader.ReadLine();
            while (rad != null)
            {
                var items = rad.Split(';');
                vaktLogg.Add(new Loggpost
                {
                    Plats = items[1],
                    Tidpunkt = Convert.ToDateTime(items[3] + " " + items[2])
                });
                rad = reader.ReadLine();
            }
            reader.Close();
            reader.Dispose();

            return vaktLogg;
        }

        public static List<Loggpost> LäsVaktLoggFörBryggplats(string bryggplatsId)
        {
            return LäsVaktLogg(@"c:\data\vaktlogg\" + bryggplatsId + ".csv");
        }

        public static void UppdateraMedlem(MarkusModel.Medlem medlem)
        {
            var medlemmar = HämtaAllaMedlemmar().ToList();
            medlemmar.RemoveAll(item => item.Id == medlem.Id);
            medlemmar.Add(medlem);
            SparaMedlemmar(medlemmar);
        }
        public static IEnumerable<MarkusModel.Medlem> HämtaAllaMedlemmar()
        {
            return FilHanterare.Läs<MarkusModel.Medlem>(new MarkusModel.Medlem().FilNamn());
        }
        public static IEnumerable<Bryggplats> HämtaAllaBryggplatser()
        {
            return FilHanterare.Läs<Bryggplats>(new Bryggplats().FilNamn());
        }
        public static IEnumerable<Bryggplats> HämtaAllaBryggplatserFörMedlem(MarkusModel.Medlem medlem)
        {
            return FilHanterare.Läs<Bryggplats>(new Bryggplats().FilNamn()).Where(_ => _.MedlemsId == medlem.Id);
        }
        public static void SparaMedlemsKalender(MarkusModel.Medlem medlem, IEnumerable<DateTime> datumLista)
        {
            var bryggplats = FilHanterare.Läs<Bryggplats>(new Bryggplats().FilNamn()).FirstOrDefault(_ => _.MedlemsId == medlem.Id);

            if (bryggplats == null) return;

            var medlemsLedigatider = from datum in datumLista
                                select new LedigBryggplats {BryggplatsId = bryggplats.Id, Dag = datum};
            var ledigaBryggplatser = FilHanterare.Läs<LedigBryggplats>(new LedigBryggplats().FilNamn()).ToList();
            ledigaBryggplatser.RemoveAll(_ => _.BryggplatsId == bryggplats.Id);
            ledigaBryggplatser.AddRange(medlemsLedigatider);
        }
        private static void SparaMedlemmar(IEnumerable<MarkusModel.Medlem> medlemmar)
        {
            FilHanterare.Spara(medlemmar, new MarkusModel.Medlem().FilNamn());
        }
    }
}