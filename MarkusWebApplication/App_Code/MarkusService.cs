using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Services;
using MarkusModel;

/// <summary>
/// Summary description for MarkusService
/// </summary>
[WebService(Namespace = "http://linderback.com/markuswebapplication/markusservice")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class MarkusService : WebService {

    public MarkusService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [Serializable]
    public class Bilaga
    {
        public string Namn { get; set; }
        public byte[] Innehåll { get; set; }
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public bool SendMail(string username, string password, string to, string subject, string body, Bilaga[] bilagor, out string errormessage)
    {
        errormessage = "";
        try
        {
            // Kontrollera user och lösenord
            if (username.ToLower() != "markus" && password.ToLower() != "jtk001")
                throw new Exception("Wrong username or password");

            var message = new MailMessage();
            
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.Sender = new MailAddress("markus@linderback.com", "Markus Linderbäck");
            message.From = new MailAddress("markus@linderback.com", "Markus Linderbäck");

            var folder = @"c:\data\" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (bilagor.Any())
                Directory.CreateDirectory(folder);

            foreach (var bilaga in bilagor)
            {
                var filnamn =  folder + @"\" + bilaga.Namn;
                var fil = new FileStream(filnamn, FileMode.CreateNew);
                fil.Write(bilaga.Innehåll, 0, bilaga.Innehåll.Count());
                fil.Close();

                var attachment = new Attachment(filnamn);
                message.Attachments.Add(attachment);
            }

            var smtpClient = new SmtpClient("smtp.bredband.net")
                                 {
                                     UseDefaultCredentials = false,
                                     Credentials = new System.Net.NetworkCredential("b248634", "jtk001")
                                 };
            smtpClient.Send(message);

            return true;
        }
        catch (Exception e)
        {
            errormessage = e.Message;
        }
        return false;
    }

    [WebMethod]
    public string SendSMS(string sender, string receiver, string message)
    {
        //        SmartSMS.SmSService sms = new SmartSMS.SmSService();
        //        return sms.SendSmsGold("L00950", "jtk001", sender, receiver, message); 
        return "";
    }

    [WebMethod]
    public void CreateWebPage(string filename, string htmlcontent)
    {
        var fs = new FileStream(@"c:\inetpub\wwwroot\" + filename, FileMode.Create);
        var ba = System.Text.Encoding.UTF8.GetBytes(htmlcontent);
        fs.Write(ba, 0, ba.Length);
        fs.Close();
    }

    [WebMethod]
    public void Premier(ClaesStatistik statistik)
    {
        FilHanterare.Spara(new List<ClaesStatistik> { statistik }, @"c:\data\statistik.txt");
    }
}
