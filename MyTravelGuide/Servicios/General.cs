using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Servicios
{
    public class Email
    {
        public bool enviarToken(String email, String token)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("informacion.globalevents@gmail.com", "uccnpwqadocomhvy"),
                    EnableSsl = true
                };

                client.Send("informacion.globalevents@gmail.com", email, "Confirma tu cuenta", "Ingresa a la siguiente URL para verificar su cuenta: " +
                   "http://" + (HttpContext.Current.Request.Url.Authority.Contains("localhost") ? HttpContext.Current.Request.Url.Authority : HttpContext.Current.Request.Url.Host) + "/Home/ValidarToken?Token=" + token);
                return true;
            }
            catch
            { return false; }
        }
    }
}
