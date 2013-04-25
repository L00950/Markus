using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MarkusModel
{
    public enum Hus
    {
        LillaHuset = 1,
        Ettvåan = 2,
        Hebbes = 5,
        MonteRojo = 6
    }
    public class Bokning
    {
        public int Id { get; set; }
        public Hus Hus { get; set; }
        public string Person { get; set; }
        public string Kommentar { get; set; }
        public DateTime Ankomst { get; set; }
        public DateTime Avresa { get; set; }
    }
    public static class HanteraBokningar
    {
        private const string FilnamnsMall = "bokningar";
        private const string Filnamn = @"c:\data\" + FilnamnsMall + ".txt";

        public static bool FinnsFil()
        {
            return File.Exists(Filnamn);
        }
        public static string Säkerhetskopiera()
        {
            var filnamn = Filnamn.Replace(FilnamnsMall, FilnamnsMall + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            Spara(Läs().ToList(), filnamn);
            return filnamn;
        }
        public static string FilNamn()
        {
            return Filnamn;
        }
        public static FileInfo FilInformation()
        {
            return new FileInfo(Filnamn);
        }
        public static void LäsFrånFil()
        {
            var reader = File.OpenText(@"c:\historik.txt");
            var str = reader.ReadLine();
            var bokningar = new List<Bokning>();
            while(str != null)
            {
                var strArray = str.Split("\t".ToCharArray());
                var bokning = new Bokning
                                  {
                                      Ankomst = DateTime.Parse(strArray[2]),
                                      Avresa = DateTime.Parse(strArray[3]),
                                      Hus = (Hus) Convert.ToInt32(strArray[1]),
                                      Id = Convert.ToInt32(strArray[0]),
                                      Kommentar = strArray[5],
                                      Person = strArray[4]

                                  };
                if(Convert.ToInt32(strArray[1]) != 3)
                    bokningar.Add(bokning);
                str = reader.ReadLine();
            }
            // OBS. Tar bort gamla sparade bokningar
            Spara(bokningar);
        }
        public static int AntalBokningar()
        {
            return Läs().Count();
        }
        public static IEnumerable<Bokning> HämtaBokningar(Hus hus)
        {
            return Läs().Where(i => i.Hus == hus);
        }
        public static Bokning HämtaBokning(int id)
        {
            return Läs().FirstOrDefault(i => i.Id == id);
        }
        public static void SparaNyBokning(Bokning bokning)
        {
            var bokningar = Läs().ToList();
            var iDn = bokningar.Select(i => i.Id);
            var maxId = (iDn.Count() == 0) ? 0 : iDn.Max();

            bokning.Id = maxId + 1;

            bokningar.Add(bokning);
            Spara(bokningar);
        }
        public static void UppdateraBokning(Bokning bokning)
        {
            var bokningar = Läs().ToList();
            var index = bokningar.FindIndex(i => i.Id == bokning.Id);
            bokningar[index] = bokning;
            Spara(bokningar);
        }
        public static void TaBortBokning(int id)
        {
            var bokningar = Läs().ToList();
            var index = bokningar.FindIndex(i => i.Id == id);
            bokningar.RemoveAt(index);
            Spara(bokningar);
        }
        public static int Login(string användarnamn, string lösenord)
        {
            if (användarnamn.ToUpper() == "ULF" && lösenord.ToUpper() == "DINOMIN") return 1;
            
            return 0;
        }
        private static void Spara(IEnumerable<Bokning> bokningar, string filNamn = Filnamn)
        {
            var writer = File.CreateText(filNamn);
            var serializer = new XmlSerializer(typeof(List<Bokning>));
            serializer.Serialize(writer, bokningar);
            writer.Close();
            writer.Dispose();
        }

        private static IEnumerable<Bokning> Läs()
        {
            var reader = File.OpenText(Filnamn);
            var serializer = new XmlSerializer(typeof(List<Bokning>));
            var bokningar = (List<Bokning>)serializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
            return bokningar.OrderBy(i => i.Ankomst);
        }
    }
}
