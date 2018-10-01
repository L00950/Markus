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
                if(medlemIn.SåldDatumSpecified) continue;

                var medlemUt = medlemmarUt.FirstOrDefault(_ => _.Id == medlemIn.MedlNr) ?? new Medlem();

                medlemUt.Id = medlemIn.MedlNr;
                medlemUt.Namn = medlemIn.Förnamn + " " + medlemIn.Efternamn;
                medlemUt.Email = medlemIn.email;
                medlemUt.Gata = medlemIn.Adress;
                medlemUt.PostNummer = medlemIn.Postnr;
                medlemUt.Ort = medlemIn.Postadress;
                medlemUt.Lösenord = medlemUt.Lösenord ?? (medlemIn.Kajplats ?? medlemIn.Upplag).Replace(" ", "");
                medlemUt.Tel = medlemIn.TelBostad;
                medlemUt.Mobil = medlemIn.TelMobil;
                if(!medlemsLista.Contains(medlemUt))
                    medlemsLista.Add(medlemUt);

                foreach (var medlemsPlats in medlemmarIn.Markus.Where(_ => _.MedlNr == medlemUt.Id && (_.Kajplats != null || _.Upplag != null) && _.SåldDatumSpecified == false))
                {
                    var bryggPlats = new Bryggplats
                        {
                            Id = medlemsPlats.Kajplats ?? medlemsPlats.Upplag,
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

            // Fil med bryggplatser till scannern
            TextWriter writer = File.CreateText(@"c:\data\bryggplats.txt");
            foreach(var bryggplats in bryggLista)
                writer.WriteLine(bryggplats.Id + ";" + medlemsLista.FirstOrDefault(_ => _.Id == bryggplats.MedlemsId)?.Namn);
            writer.Close();
            writer.Dispose();

            // Fil med bryggplatser till scannern
            writer = File.CreateText(@"c:\data\till_pärmen.txt");
            foreach (var bryggplats in bryggLista)
                writer.WriteLine(medlemsLista.FirstOrDefault(_ => _.Id == bryggplats.MedlemsId)?.Namn + ";" + bryggplats.Id + ";" + medlemsLista.FirstOrDefault(_ => _.Id == bryggplats.MedlemsId)?.Namn);
            writer.Close();
            writer.Dispose();

            System.Console.ReadLine();
        }
    }
}