using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace MarkusConsoleApplication
{
    public static class Spanien
    {
        public static void HämtaStatus()
        {
            try
            {
                var request = WebRequest.Create("http://192.168.10.1/api/monitoring/traffic-statistics");
                var response = request.GetResponse();
                if (((HttpWebResponse) response).StatusCode == HttpStatusCode.OK)
                {
                    var stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    var xml = reader.ReadToEnd().Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    var upload = Convert.ToUInt64(doc.SelectSingleNode("//response/TotalUpload").InnerText);
                    var download = Convert.ToUInt64(doc.SelectSingleNode("//response/TotalDownload").InnerText);

                    var trafikdata = MarkusModel.FilHanterare.Läs<Trafik>(new Trafik().FilNamn()).ToList();
                    trafikdata.Add(new Trafik {Tidpunkt = DateTime.Now, UpLoad = upload, DownLoad = download});
                    MarkusModel.FilHanterare.Spara<Trafik>(trafikdata, new Trafik().FilNamn());
                }
                response.Close();
            }
            catch (Exception exception)
            {
            }
        }
    }

    [Serializable]
    public class Trafik : MarkusModel.IFilKlass
    {
        public DateTime Tidpunkt { get; set; }
        public UInt64 UpLoad { get; set; }
        public UInt64 DownLoad { get; set; }

        public UInt64 Total()
        {
            return UpLoad + DownLoad;
        }
        public string FilNamn()
        {
            return @"c:\data\trafik.txt";
        }
    }
}