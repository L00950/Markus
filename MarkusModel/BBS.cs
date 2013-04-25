using System;
using System.Collections.Generic;
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

    public static class MedlemsRegister
    {
        public static void UppdateraMedlem(Medlem medlem)
        {
            var medlemmar = HämtaAllaMedlemmar().ToList();
            medlemmar.RemoveAll(item => item.Id == medlem.Id);
            medlemmar.Add(medlem);
            SparaMedlemmar(medlemmar);
        }
        public static IEnumerable<Medlem> HämtaAllaMedlemmar()
        {
            return FilHanterare.Läs<Medlem>(new Medlem().FilNamn());
        }
        public static IEnumerable<Bryggplats> HämtaAllaBryggplatser()
        {
            return FilHanterare.Läs<Bryggplats>(new Bryggplats().FilNamn());
        }
        public static IEnumerable<Bryggplats> HämtaAllaBryggplatserFörMedlem(Medlem medlem)
        {
            return FilHanterare.Läs<Bryggplats>(new Bryggplats().FilNamn()).Where(_ => _.MedlemsId == medlem.Id);
        }
        public static void SparaMedlemsKalender(Medlem medlem, IEnumerable<DateTime> datumLista)
        {
            var bryggplats = FilHanterare.Läs<Bryggplats>(new Bryggplats().FilNamn()).FirstOrDefault(_ => _.MedlemsId == medlem.Id);

            if (bryggplats == null) return;

            var medlemsLedigatider = from datum in datumLista
                                select new LedigBryggplats {BryggplatsId = bryggplats.Id, Dag = datum};
            var ledigaBryggplatser = FilHanterare.Läs<LedigBryggplats>(new LedigBryggplats().FilNamn()).ToList();
            ledigaBryggplatser.RemoveAll(_ => _.BryggplatsId == bryggplats.Id);
            ledigaBryggplatser.AddRange(medlemsLedigatider);
        }
        private static void SparaMedlemmar(IEnumerable<Medlem> medlemmar)
        {
            FilHanterare.Spara(medlemmar, new Medlem().FilNamn());
        }
    }
}