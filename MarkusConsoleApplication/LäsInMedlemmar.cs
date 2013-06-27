using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MarkusModel;

namespace MarkusConsoleApplication
{
    static class LäsInMedlemmar
    {
        public static void Kör()
        {
            var serializer = new XmlSerializer(typeof (BBS.dataroot));
            var medlemmarIn = (BBS.dataroot)serializer.Deserialize(new StreamReader(@"c:\data\markus.xml"));
            var medlemmarUt = FilHanterare.Läs<Medlem>(new Medlem().FilNamn()).ToList();
            var medlemsLista = new List<Medlem>();
            var bryggLista = new List<Bryggplats>();
            foreach (var medlemIn in medlemmarIn.Markus)
            {
                if(medlemIn.Kajplats == null || medlemIn.SåldDatumSpecified) continue;

                var medlemUt = medlemmarUt.FirstOrDefault(_ => _.Id == medlemIn.MedlNr) ?? new Medlem();

                medlemUt.Id = medlemIn.MedlNr;
                medlemUt.Namn = medlemIn.Förnamn + " " + medlemIn.Efternamn;
                medlemUt.Email = medlemIn.email;
                medlemUt.Gata = medlemIn.Adress;
                medlemUt.PostNummer = medlemIn.Postnr;
                medlemUt.Ort = medlemIn.Postadress;
                medlemUt.Lösenord = medlemUt.Lösenord ?? medlemIn.Kajplats.Replace(" ", "");
                medlemUt.Tel = medlemIn.TelBostad;
                medlemUt.Mobil = medlemIn.TelMobil;
                if(!medlemsLista.Contains(medlemUt))
                    medlemsLista.Add(medlemUt);

                foreach (var medlemsPlats in medlemmarIn.Markus.Where(_ => _.MedlNr == medlemUt.Id && _.Kajplats != null && _.SåldDatumSpecified == false))
                {
                    var bryggPlats = new Bryggplats
                        {
                            Id = medlemsPlats.Kajplats,
                            MedlemsId = medlemIn.MedlNr,
                            Båt = medlemIn.Typbeteckning
                        };
                    if(bryggLista.All(_ => _.Id != bryggPlats.Id))
                        bryggLista.Add(bryggPlats);
                }
            }
            System.Console.WriteLine("Antal medlemmar: " + medlemsLista.Count());
            System.Console.WriteLine("Antal Bryggplatser: " + bryggLista.Count());
            FilHanterare.Spara(medlemsLista, new Medlem().FilNamn());
            FilHanterare.Spara(bryggLista, new Bryggplats().FilNamn());
            System.Console.ReadLine();
        }
    }
}