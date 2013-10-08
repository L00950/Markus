using System;
using System.Collections.Generic;
using System.Linq;
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
            markusTelldus.Kör();
        }
    }

    public class StaticDatum
    {
        public DateTime SenasteMail;
        public List<Enhet> EnheterAttLyssnaPå;
    }

    public class Enhet
    {
        public string Protokoll;
        public string Id;
        public string Huskod;
        public string Enhetskod;
    }

    public class MarkusTelldus
    {
        static readonly StaticDatum Locker = new StaticDatum
            {
                EnheterAttLyssnaPå = new List<Enhet>
                {
                    new Enhet{Huskod = "10429994", Enhetskod = "10"},
                    new Enhet{Protokoll = "fineoffset", Id = "162"}
                },
                SenasteMail = DateTime.MinValue
            };

        public void Kör()
        {
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

            var eventId = obj.tdRegisterRawDeviceEvent(RawListeningCallbackFuntion, null);

            Console.WriteLine("Redo att ta emot request...");
            Console.ReadKey();

            Console.WriteLine("Avslutar");
            obj.unregisterCallback(eventId);
        }
        
        private int RawListeningCallbackFuntion(string data, int controllerId, int callbackId, Object obj)
        {
            Console.WriteLine("Request {0} {1}", DateTime.Now.ToString("HH:mm:ss.fff"), data);
            //lock (Locker)
            //{
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
                    //}

                    //Console.WriteLine("data:{0} controllerId:{1} callbackId:{2} obj:{3}", data, controllerId, callbackId, obj);
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

            if (Locker.EnheterAttLyssnaPå.Any(_ => _.Huskod == huskod && _.Enhetskod == enhetskod))
            {
                var now = DateTime.Now;
                if (now - Locker.SenasteMail < new TimeSpan(0, 0, 4))
                    return;

                Locker.SenasteMail = DateTime.Now;

                var tidpunkt = DateTime.Now.ToString("HH:mm:ss.fff");
                Console.WriteLine(huskod + " " + enhetskod + " " + kommando + " " + tidpunkt);

                if (huskod == "10429994" && enhetskod == "10" && kommando == "turnon")
                {
                    Console.WriteLine("PIR Entre PÅ");
                    new Thread(() => SkickaMail("Larm från entre " + tidpunkt)).Start();
                }
                else if (huskod == "10429994" && enhetskod == "10" && kommando == "turnoff")
                    Console.WriteLine("PIR Entre AV");
                else
                    Console.WriteLine("Annat kommando: " + enhetskod + " " + huskod + " " + kommando);
            }
            else if (Locker.EnheterAttLyssnaPå.Any(_ => _.Protokoll == protokoll && _.Id == id))
                Console.WriteLine("Tid: {0} Temp: {1}", DateTime.Now.ToString("HH:mm:ss"), temp);
            else
                Console.WriteLine("Okänd enhet: {0}", data);
        }

        private void SkickaMail(string meddelande)
        {
            Console.WriteLine("Startar skicka mail");
            var larm = FilHanterare.Läs<Larm>(@"c:\data\larm.txt").FirstOrDefault();
            if (larm != null && larm.Aktiverat)
            {
                var client = Gmail.GmailSmtpKlient();
                client.Send("markus@linderback.com", "markus@linderback.com,camilla@linderback.com", "Larm Lidingö", meddelande);
            }
            else
            {
                Console.WriteLine("Larm inaktivt, inget mail skickat");
            }
            Console.WriteLine("Färdig med att skicka mail");
        }
    }
}
