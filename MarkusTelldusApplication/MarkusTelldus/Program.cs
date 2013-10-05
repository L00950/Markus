using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Huskod;
        public string Enhetskod;
    }

    public class MarkusTelldus
    {
        //static DateTime _senasteMail;
        static readonly StaticDatum Locker = new StaticDatum
            {
                EnheterAttLyssnaPå = new List<Enhet>{new Enhet{Huskod = "10429994", Enhetskod = "10"}},
                SenasteMail = DateTime.MinValue
            };

        public void Kör()
        {
            //_senasteMail = DateTime.MinValue;

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
            lock (Locker)
            {
                try
                {
                    var keypair = data.Split(';');
                    var huskod = "";
                    var enhetskod = "";
                    var kommando = "";
                    foreach (var s in keypair)
                    {
                        var värde = s.Split(':');
                        if (värde[0].Equals("house", StringComparison.CurrentCultureIgnoreCase))
                            huskod = värde[1];
                        if (värde[0].Equals("unit", StringComparison.CurrentCultureIgnoreCase))
                            enhetskod = värde[1];
                        if (värde[0].Equals("method", StringComparison.CurrentCultureIgnoreCase))
                            kommando = värde[1];
                    }

                    if (!Locker.EnheterAttLyssnaPå.Any(_ => _.Huskod == huskod && _.Enhetskod == enhetskod))
                        return TelldusNETWrapper.TELLSTICK_SUCCESS;


                    var now = DateTime.Now;
                    if (now - Locker.SenasteMail < new TimeSpan(0, 0, 2))
                        return TelldusNETWrapper.TELLSTICK_SUCCESS;

                    Locker.SenasteMail = DateTime.Now;

                    var tidpunkt = DateTime.Now.ToString("HH:mm:ss.fff");
                    Console.WriteLine(huskod + " " + enhetskod + " " + kommando + " " + tidpunkt);

                    if (huskod == "10429994" && enhetskod == "10" && kommando == "turnon")
                    {
                        Console.WriteLine("PIR Entre PÅ");
                        var tråd = new System.Threading.Thread(() => SkickaMail("Larm från entre " + tidpunkt));
                        tråd.Start();
                    }
                    else if (huskod == "10429994" && enhetskod == "10" && kommando == "turnoff")
                        Console.WriteLine("PIR Entre AV");
                    else
                    {
                        Console.WriteLine("Annat kommando: " + enhetskod + " " + huskod + " " + kommando);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }

            //Console.WriteLine("data:{0} controllerId:{1} callbackId:{2} obj:{3}", data, controllerId, callbackId, obj);
            return TelldusNETWrapper.TELLSTICK_SUCCESS;
        }

        private void SkickaMail(string meddelande)
        {
            Console.WriteLine("Startar skicka mail");
            var client = Gmail.GmailSmtpKlient();
            client.Send("markus@linderback.com", "markus@linderback.com", "Larm Lidingö", meddelande);
            Console.WriteLine("Färdig med att skicka mail");
        }
    }
}
