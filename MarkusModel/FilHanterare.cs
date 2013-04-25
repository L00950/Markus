using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MarkusModel
{
    public interface IFilKlass
    {
        string FilNamn();
    }

    public static class FilHanterare
    {
        public static void SkapaInitialFil<T>(string filnamn)
        {
            Spara(new List<T>(), filnamn);
        }

        public static IEnumerable<T> Läs<T>(string filnamn)
        {
            TextReader reader = File.OpenText(filnamn);
            var serializer = new XmlSerializer(typeof(List<T>));
            var lista = (List<T>)serializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
            return lista;
        }
        public static void Spara<T>(IEnumerable<T> objects, string filnamn)
        {
            TextWriter writer = File.CreateText(filnamn);
            var serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(writer, objects);
            writer.Close();
            writer.Dispose();
        }
    }
}
