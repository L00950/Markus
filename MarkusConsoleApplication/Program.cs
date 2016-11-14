using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MarkusConsoleApplication
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Any(_ => _.Equals("BBS")))
                LäsInMedlemmar.Kör();
            else if (args.Any(_ => _.Equals("UppdateraKalender")))
            {
                var kalender = new MarkusModel.Kalender();
                kalender.SkapaKalendrarFörMonteRojo();
                kalender.SkapaKalendrarFörRyda();
            }
            else if (args.Any(_ => _.Equals("maila")))
            {
                try
                {
                    var smtp = Gmail.TeliaSmtpKlient();
                    smtp.Send("markus.linderback@telia.com", "markus.linderback@danica.se", "Test", "Hej!");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (args.Any(_ => _.Equals("json")))
            {
                var o = new TestKlass { Datum = DateTime.Now, Double = 123.456, Sträng = "Markus Linderbäck" };
                var ser = new JavaScriptSerializer();
                Console.WriteLine(ser.Serialize(o));
            }
            else if (args.Any(_ => _.Equals("PDF")))
            {
                TestaLäsaPdf();
            }
            else if (args.Any(_ => _.Equals("test")))
            {
                TcpClient client = new TcpClient();
                client.Connect(Dns.GetHostAddresses("markuspi")[0], 8089); //Connect to the server on our local host IP address, listening to port 3000
                
                NetworkStream clientStream = client.GetStream();
                var buffer = Encoding.ASCII.GetBytes("cache");
                clientStream.Write(buffer, 0, buffer.Count());

                System.Threading.Thread.Sleep(1000); //Sleep before we get the data for 1 second
                while (clientStream.DataAvailable)
                {
                    byte[] inMessage = new byte[2048];
                    int bytesRead = 0;
                    try
                    {
                        bytesRead = clientStream.Read(inMessage, 0, 2048);
                    }
                    catch { /*Catch exceptions and handle them here*/ }

                    ASCIIEncoding encoder = new ASCIIEncoding();
                    Console.WriteLine(encoder.GetString(inMessage, 0, bytesRead));
                }

                client.Close();
                System.Threading.Thread.Sleep(10000); //Sleep for 10 seconds


                ConsoleApplication.AsynchronousClient.StartClient();


                byte[] bytes = new byte[2048];
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostAddresses("markuspi"));
                IPAddress ipAddress = Dns.GetHostAddresses("markuspi")[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8089);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("cache");

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch(Exception exception)
                {
                }
            }
        }

        public static void TestaLäsaPdf()
        {
            var reader = new iTextSharp.text.pdf.PdfReader(@"c:\dokument\test.pdf");
            foreach(var field in reader.AcroFields.Fields)
            {
                var key = field.Key;
                var value = reader.AcroFields.GetField(key);
                Console.WriteLine(key + ": " + value);
            }
            var stream = new System.IO.MemoryStream();
            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, stream) { FormFlattening = true };
            stamper.Close();

            var hashcode = reader.GetHashCode();
        }
    }
    public class TestKlass
    {
        public string Sträng;
        public DateTime Datum;
        public double Double;
    }
}
