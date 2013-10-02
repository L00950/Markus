using System.Net.Mail;

public static class Gmail
{
    public static SmtpClient GmailSmtpKlient()
    {
        return new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("markus.linderback@gmail.com", "jtk001jtk001"),
                EnableSsl = true
            };
    }
    public static SmtpClient TeliaSmtpKlient()
    {
        return new SmtpClient("mailout.telia.com", 465) // alternativt 465
        {
            Credentials = new System.Net.NetworkCredential("markus.linderback@telia.com", "Markus01"),
            EnableSsl = true
        };
    }
}