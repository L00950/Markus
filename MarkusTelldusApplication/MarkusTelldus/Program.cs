using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using MarkusModel;
using TelldusWrapper;

// se https://github.com/telldus/telldus/blob/master/bindings/dotnet/example/BasicListDevicesNetExample.zip

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var markusTelldus = new MarkusTelldus();
            markusTelldus.Kör(args);
        }
    }

    public class StaticDatum
    {
        public DateTime SenasteMail;
        public List<Enhet> EnheterAttLyssnaPå;
    }

    public class Enhet
    {
        public string Namn;
        public string Protokoll;
        public string Id;
        public string Huskod;
        public string Enhetskod;
        public DateTime SenasteLarm;
    }

    public class Kamera
    {
        public string Id;
        public string Url;
        public string User;
        public string Password;
    }

    public class MarkusTelldus
    {
        private static readonly Object Locker = new Object();

        private static DateTime SenasteMail = DateTime.MinValue;
        private static bool debug = false;

        const string TERMOMETER = "Termometer";


        private static readonly List<Enhet> EnheterAttLyssnaPå = new List<Enhet>
                {
                    new Enhet{Namn="PIR Entre", Huskod = "10429994", Enhetskod = "10", SenasteLarm = DateTime.Now},
                    new Enhet{Namn="PIR Baksida", Huskod = "10427074", Enhetskod = "10", SenasteLarm = DateTime.Now},
                    new Enhet{Namn=TERMOMETER, Protokoll = "fineoffset", Id = "162"},
                    new Enhet{Namn="Garagedörr", Huskod = "54888742", Enhetskod = "4", SenasteLarm = DateTime.Now}
                };

        private static readonly List<Kamera> Kameror = new List<Kamera>
            {
                new Kamera
                    {
                        Id = "Entre", 
                        Password = "jtk001", 
                        User = "admin", 
                        Url = "http://192.168.1.84/image.jpg"
                    },
                new Kamera
                    {
                        Id = "Vardagsrum",
                        Password = "jtk001",
                        User = "admin",
                        Url = "http://192.168.1.83/img/snapshot.cgi"
                    }
            };

        public void Kör(string[] args)
        {
            if (args.Contains("debug"))
                debug = true;

            var obj = new TelldusNETWrapper();

            Console.WriteLine("Startar");

            var antal = TelldusNETWrapper.tdGetNumberOfDevices();
            Console.WriteLine("Antal enheter:{0}", antal);
            for (var i = 0; i < antal; i++)
            {
                var deviceId = TelldusNETWrapper.tdGetDeviceId(i);
                var deviceTypeId = TelldusNETWrapper.tdGetDeviceType(deviceId);
                var name = TelldusNETWrapper.tdGetName(deviceId);
                var model = TelldusNETWrapper.tdGetModel(deviceId);
                var lastSendCommand = TelldusNETWrapper.tdLastSentCommand(deviceId,
                                                    TelldusNETWrapper.TELLSTICK_TURNON |
                                                    TelldusNETWrapper.TELLSTICK_TURNOFF);
                var lastSendValue = TelldusNETWrapper.tdLastSentValue(deviceId);
                var protocoll = TelldusNETWrapper.tdGetProtocol(deviceId);

                var husKod = TelldusNETWrapper.tdGetDeviceParameter(deviceId, "house", "");
                var enhetsKod = TelldusNETWrapper.tdGetDeviceParameter(deviceId, "unit", "");


                Console.WriteLine("DeviceId:{0} DeviceTyp:{1} Namn:{2} Huskod:{3} Enhetskod:{4}", deviceId, deviceTypeId,
                                  name, husKod, enhetsKod);
            }
            TelldusNETWrapper.tdClose();

            int eventId;
            eventId = obj.tdRegisterRawDeviceEvent(RawListeningCallbackFuntion, null);
            //eventId = obj.tdRegisterDeviceEvent(DeviceEventFunction, null);

            Console.WriteLine("Redo att ta emot request...");
            var tryckt = Console.ReadKey();
            while (tryckt.KeyChar != 'q')
            {
                Console.Clear();
                if (tryckt.KeyChar == 't')
                {
                    MeddelaDetektion();
                }
                tryckt = Console.ReadKey();
            }

            Console.WriteLine("Avslutar");
            obj.unregisterCallback(eventId);
        }

        private int DeviceEventFunction(int deviceid, int method, string data, int callbackid, object obj)
        {
            Console.WriteLine("DeviceId: {0} Method: {1} Data: {2} CallbackId: {3} Tid: {4}", deviceid, method, data, callbackid, DateTime.Now.ToString("HH:mm:ss,fff"));
            return TelldusNETWrapper.TELLSTICK_SUCCESS;
        }

        private int RawListeningCallbackFuntion(string data, int controllerId, int callbackId, Object obj)
        {
            if(debug)
                Console.WriteLine("Request {0} {1}", DateTime.Now.ToString("HH:mm:ss.fff"), data);

            try
            {
                new Thread(() => HanteraRequest(data)).Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            catch
            {
                Console.WriteLine("Okänt exception...");
            }
            return TelldusNETWrapper.TELLSTICK_SUCCESS;
        }

        private void HanteraRequest(string data)
        {
            var keypair = data.Split(';');
            var huskod = "";
            var enhetskod = "";
            var kommando = "";
            var protokoll = "";
            var id = "";
            var temp = "";
            foreach (var s in keypair)
            {
                var värde = s.Split(':');
                if (värde[0].Equals("house", StringComparison.CurrentCultureIgnoreCase))
                    huskod = värde[1];
                if (värde[0].Equals("unit", StringComparison.CurrentCultureIgnoreCase))
                    enhetskod = värde[1];
                if (värde[0].Equals("method", StringComparison.CurrentCultureIgnoreCase))
                    kommando = värde[1];
                if (värde[0].Equals("protocol", StringComparison.CurrentCultureIgnoreCase))
                    protokoll = värde[1];
                if (värde[0].Equals("id", StringComparison.CurrentCultureIgnoreCase))
                    id = värde[1];
                if (värde[0].Equals("temp", StringComparison.CurrentCultureIgnoreCase))
                    temp = värde[1];
            }

            var enhet = HittaEnhet(huskod, enhetskod, protokoll, id);
            if (enhet == null)
                return;

            if (enhet.Namn == TERMOMETER)
                Console.WriteLine("Tid: {0} Temp: {1}", DateTime.Now.ToString("HH:mm:ss"), temp);

            if (EnheterAttLyssnaPå.Any(_ => _.Huskod == huskod && _.Enhetskod == enhetskod))
            {
                var now = DateTime.Now;
                var fortsätt = true;
                lock (Locker)
                {
                    if (now - SenasteMail < new TimeSpan(0, 0, 4))
                        fortsätt = false;
                    else
                        SenasteMail = DateTime.Now;
                }
                if (!fortsätt)
                    return;

                var tidpunkt = DateTime.Now.ToString("HH:mm:ss,fff");
                Console.WriteLine(huskod + " " + enhetskod + " " + kommando + " " + tidpunkt);

                if (huskod == "10429994" && enhetskod == "10" && kommando == "turnon")
                {
                    Console.WriteLine("PIR Entre PÅ");
                    new Thread(() => HanteraEvent("Larm från entre " + tidpunkt)).Start();
                }
                if (huskod == "10427074" && enhetskod == "10" && kommando == "turnon")
                {
                    Console.WriteLine("PIR Inne PÅ");
                    new Thread(() => HanteraEvent("Larm från baksidan " + tidpunkt)).Start();
                }
                if (huskod == "54888742" && enhetskod == "4" && kommando == "turnon")
                {
                    Console.WriteLine("Garagedörr öppnas");
                    new Thread(() => HanteraEvent("Larm från garagedörr " + tidpunkt)).Start();
                }
                else if (debug)
                    Console.WriteLine("Annat kommando: " + enhetskod + " " + huskod + " " + kommando);
            }
            else if(debug)
                Console.WriteLine("Okänd enhet: {0}", data);
        }

        private Enhet HittaEnhet(string huskod, string enhetskod, string protokoll, string id)
        {
            var termomteter = EnheterAttLyssnaPå.First(_ => _.Namn == TERMOMETER);
            if (protokoll == termomteter.Protokoll && id == termomteter.Id)
                return termomteter;

            return EnheterAttLyssnaPå.FirstOrDefault(_ => _.Huskod == huskod && _.Enhetskod == enhetskod);
        }

        private void HanteraEvent(string meddelande)
        {
            Console.WriteLine("Startar skicka mail");
            var larm = FilHanterare.Läs<Larm>(@"c:\data\larm.txt").FirstOrDefault();
            if (larm != null && larm.Aktiverat)
            {
                new Thread(HämtaBilder).Start();
                new Thread(MeddelaDetektion).Start();
                var client = Gmail.GmailSmtpKlient();
                client.Send("markus@linderback.com", "markus@linderback.com", "Larm Lidingö", meddelande);
            }
            else
            {
                Console.WriteLine("Larm inaktivt, inget mail skickat");
            }
            Console.WriteLine("Färdig med att skicka mail");
        }

        private void MeddelaDetektion()
        {
            Console.WriteLine("Meddela detektion start: " + DateTime.Now.ToString("HH:mm:ss,fff"));
            Thread.Sleep(2000);
            if(TelldusNETWrapper.tdLastSentCommand(12,
                                                    TelldusNETWrapper.TELLSTICK_TURNON |
                                                    TelldusNETWrapper.TELLSTICK_TURNOFF) == TelldusNETWrapper.TELLSTICK_TURNOFF)
                TelldusNETWrapper.tdTurnOn(12);
            Thread.Sleep(2000);
            if (TelldusNETWrapper.tdLastSentCommand(12,
                                                    TelldusNETWrapper.TELLSTICK_TURNON |
                                                    TelldusNETWrapper.TELLSTICK_TURNOFF) == TelldusNETWrapper.TELLSTICK_TURNON)
                TelldusNETWrapper.tdTurnOff(12);
            Console.WriteLine("Meddela detektion slut: " + DateTime.Now.ToString("HH:mm:ss,fff"));
        }

        private void HämtaBilder()
        {
            try
            {
                var start = DateTime.Now;
                var mapp = start.ToString("yyyyMMdd_HHmmssfff");
                while (DateTime.Now - start < new TimeSpan(0, 0, 10))
                {
                    HämtaBild(mapp, Kameror.First(_ => _.Id == "Entre"));
                    HämtaBild(mapp, Kameror.First(_ => _.Id == "Vardagsrum"));
                    Thread.Sleep(1000);
                }
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void HämtaBild(string mapp, Kamera kamera)
        {
            var folder = @"c:\data\larm\" + mapp;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            try
            {
                byte[] lnFile;

                var lxRequest = (HttpWebRequest)WebRequest.Create(kamera.Url);
                lxRequest.Credentials = new NetworkCredential(kamera.User, kamera.Password);
                lxRequest.Timeout = 300;
                using (var lxResponse = (HttpWebResponse)lxRequest.GetResponse())
                {
                    using (var lxBr = new BinaryReader(lxResponse.GetResponseStream()))
                    {
                        using (var lxMs = new MemoryStream())
                        {
                            byte[] lnBuffer = lxBr.ReadBytes(1024);
                            while (lnBuffer.Length > 0)
                            {
                                lxMs.Write(lnBuffer, 0, lnBuffer.Length);
                                lnBuffer = lxBr.ReadBytes(1024);
                            }
                            lnFile = new byte[(int)lxMs.Length];
                            lxMs.Position = 0;
                            lxMs.Read(lnFile, 0, lnFile.Length);
                            lxMs.Close();
                            lxBr.Close();
                        }
                    }
                    lxResponse.Close();
                }

                var filnamn = String.Format(folder + @"\{0}_{1}.jpg", DateTime.Now.ToString("yyyyMMdd_HHmmssfff"), kamera.Id);
                using (var fileStream = new FileStream(filnamn, FileMode.Create))
                {
                    fileStream.Write(lnFile, 0, lnFile.Length);
                    fileStream.Close();
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine("Timeout exception från " + kamera.Id + ": " + exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception från " + kamera.Id + ": " + exception.Message);
            }

        }
    }
}
