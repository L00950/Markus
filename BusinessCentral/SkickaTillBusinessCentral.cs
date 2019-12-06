using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace BusinessCentral
{
    public class Test
    {
        const string url = "https://api.businesscentral.dynamics.com/v1.0/d24bf728-804a-454f-8a5c-356c2729031a/api/beta/companies(40532c34-3519-475b-a46f-e559fc0b1557)/journals(c94206ed-0229-4efa-aea5-b8dee270120f)/journalLines";
        const string urlBokför = "https://api.businesscentral.dynamics.com/v1.0/d24bf728-804a-454f-8a5c-356c2729031a/api/beta/companies(40532c34-3519-475b-a46f-e559fc0b1557)/journals(c94206ed-0229-4efa-aea5-b8dee270120f)/Microsoft.NAV.post";

        public void Run()
        {
            try
            {
                Console.WriteLine($"Påbörjar jobbet");

                var starttid = DateTime.Now;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Basic TUFSS1VTLkxJTkRFUkJBQ0s6RGxCaHE0a1cvQUVTb3RlU1BFV3daMnlXZWlzbms3UWdiMHJmTjVVOFlPWT0=");

                Console.WriteLine($"Kontrollerar att inget finns i journalen");
                var result = client.GetAsync(url).Result;
                if(!result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Fel vid läsande av journal " + result.Content.ReadAsStringAsync().Result);
                    return;
                }
                var con = result.Content.ReadAsStringAsync().Result;
                // Fulkontroll så länge
                if(con.Length > 684)
                {
                    Console.WriteLine("Troligen inte en tom journal, avbryter för säkerhets skull. Journal: " + result.Content.ReadAsStringAsync().Result);
                    return;
                }


                var sr = new System.IO.StreamReader("20191205.json");
                var input = sr.ReadToEnd();
                sr.Close();



                var verifikationer = JsonConvert.DeserializeObject<BCVerifikation[]>(input);

                // Lägg till rader i journal
                var felVidSkickande = false;
                var antalSkickade = 0;
                foreach(BCVerifikation verifikation in verifikationer)
                {
                    var json = JsonConvert.SerializeObject(verifikation);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    result = client.PostAsync(url, content).Result;
                    if(!result.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Fel vid skickande av post " + result.Content.ReadAsStringAsync().Result);
                        felVidSkickande = true;
                        break;
                    }
                    antalSkickade++;
                }

                Console.WriteLine($"Totalt antal poster: {verifikationer.Length}");
                Console.WriteLine($"Skickade poster: {antalSkickade}");

                // Bokför journal
                if(!felVidSkickande)
                {
                    var r = client.PostAsync(urlBokför, new StringContent("", Encoding.UTF8, "application/json")).Result;
                    if(r.IsSuccessStatusCode)
                        Console.WriteLine($"Bokfört!");
                    else
                        Console.WriteLine($"Bokföringen misslyckades!!!! {r.Content.ReadAsStringAsync().Result}");
                }
                Console.WriteLine($"Total tid: {DateTime.Now-starttid}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    public class BCVerifikation
    {
        public string lineNumber { get; set; }
        public string accountNumber { get; set; }
        public string postingDate { get; set; }
        public string description { get; set; }
        public string documentNumber { get; set; }
        public double amount { get; set; }
        public BCDimension[] dimensions { get; set; }
    }

    public class BCDimension
    {
        public string code { get; set; }
        public string valueCode { get; set; }
    }
}