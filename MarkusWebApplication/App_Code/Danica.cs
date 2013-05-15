using System;
using System.Web;

namespace MarkusWebApplication
{
    public static class Danica
    {
        public static HttpCookie SkapaNyCookie()
        {
            return new HttpCookie("userNameDanica") { Expires = DateTime.Now.AddMinutes(1) };
        }
    }
}
