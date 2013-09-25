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
}