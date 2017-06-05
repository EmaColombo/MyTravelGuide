using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
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

    /// <summary>
    /// Clase para manejar imagenes en Cludinary.
    /// </summary>
    public class Imagenes
    {
        static Account account = new Account("hrr6lj3xm", "857819398585847", "R4Vf2S8YjWh9bFo-YAl1UPAkgqk");
        static Cloudinary cloudinary = new Cloudinary(account);

        /// <summary>
        /// Sube una imagen a Cludinary.
        /// </summary>
        /// <param name="Path">Ruta de imagen</param>
        /// <param name="GenerateId">Id con el que se identificará la imagen</param>
        /// <returns></returns>
        public bool subirImagen(String Path, String GenerateId)
        {
            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(Path),
                PublicId = GenerateId
            };

            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Sube imagen sin id específico, se genera automático.
        /// </summary>
        /// <param name="Path">Ruta de imagen</param>
        /// <returns>Diccionario con código de respuesta, si es 200 es ok y si es 200, el Id de la imagen generada</returns>
        public ImageUploadResult subirImagen(String Path)
        {
            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(@Path)
            };

            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            ImageUploadResult result = new ImageUploadResult();
            result.Status = uploadResult.StatusCode.ToString();
            result.Uri= uploadResult.Uri.ToString();
            return result;
        }

        /// <summary>
        /// Obtiene url de la imagen a partir de su nombre completo (Con extension).
        /// </summary>
        /// <param name="NombreImagen">El nombre con el que se guardo la imagen</param>
        /// <returns>URL dónde se encuentra la imagen</returns>
        public string getUrl(String NombreImagen)
        {
            //return cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", NombreImagen, Format));
            return cloudinary.Api.UrlImgUp.BuildUrl(NombreImagen);
        }
    }

    public class ImageUploadResult
    {
        public string Status { get; set; }
        public string Uri { get; set; }
    }

    public static class Rolls
    {
        /// <summary>
        /// A partir del perfil de usuario va a obtener si el evento se va a destacar o no.
        /// Solo se destacará aquellos eventos que el usuario sea destacado.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool ObtenerSiEventoEsDestacado(int UserId)
        {
            RepositorioClases.Users usuario = UserService.Get(UserId).SingleOrDefault();
            return usuario.UserDestacado.HasValue ? (bool)usuario.UserDestacado : false;
        }

        /// <summary>
        /// Obtiene si el evento esta habilitado o necesita aprobación de administrador para verse.
        /// Solo se le habilitará a las personas que tengan más de 5 eventos en estado habilitado.
        /// De lo contrario, se pasará a revisión, tanto si tiene eliminados como reportados o bloqueados
        /// o más pendientes de aprobación.
        /// </summary>
        /// <param name="UserId">Usuario creador del evento</param>
        /// <returns>True = Evento habilitado, False = Evento en estado pendiente de habilitación.</returns>
        public static bool ObtenerEstadoEventoPorUsuario(int UserId)
        {
            RepositorioClases.Users usuario = UserService.Get(UserId).SingleOrDefault();
            List<RepositorioClases.Events> eventos = EventsService.ObtenerEventos(null)
                .Where(z => z.IdUser == UserId)
                .Where(z => z.Estado == RepositorioClases.EventState.Habilitado)
                .ToList();
            return eventos.Count > 5;
        }

        /// <summary>
        /// Este método lo que hace es validar que se cumplan X reglas para destacarlo o no.
        /// Estas 'reglas' son varias y solo lo destaca, pero si no cumplió algunos normas,
        /// como tener comentarios bloqueados o eventos, más le costará que vuelva a ser destacado.
        /// </summary>
        /// <param name="UserId">Usuario a evaluar</param>
        public static void VerificarUsuarioParaDestacar(int UserId)
        {
            RepositorioClases.Users usuario = UserService.Get(UserId).SingleOrDefault();
            // Eventos creados por el usuario.
            List<RepositorioClases.Events> eventos = EventsService.ObtenerEventos(null)
                .Where(z => z.IdUser == UserId)
                .ToList();
            // Comentarios creados por el usuario.
            List<RepositorioClases.Comments> comentarios = CommentsService.ObtenerComentarios()
                .Where(z => z.iDUsuario == UserId)
                .ToList();
            // Reportes creados por el usuario.
            //List<RepositorioClases.Reportes> reportes = ReportServices.ObtenerReportesPorUsuario(UserId).
                //ToList();

            long Rank = 0;
            Rank += eventos.Where(z => z.Estado == RepositorioClases.EventState.Habilitado).Count();
            Rank -= eventos.Where(z => z.Estado == RepositorioClases.EventState.Bloqueado).Count() * 5;
            Rank -= eventos.Where(z => z.Estado == RepositorioClases.EventState.Eliminado).Count() * 3;
            Rank += comentarios.Where(z => z.Estado == RepositorioClases.Estado.Activo).Count() / 20;
            Rank -= comentarios.Where(z => z.Estado == RepositorioClases.Estado.Bloqueado).Count() * 5;
            Rank -= comentarios.Where(z => z.Estado == RepositorioClases.Estado.Eliminado).Count();

            if (Rank > 20)
                EventsService.DestacarUsuario(UserId, true);
        }

        public static long ObtenerRankPorUsuario(int UserId)
        {
            RepositorioClases.Users usuario = UserService.Get(UserId).SingleOrDefault();
            // Eventos creados por el usuario.
            List<RepositorioClases.Events> eventos = EventsService.ObtenerEventos(null)
                .Where(z => z.IdUser == UserId)
                .ToList();
            // Comentarios creados por el usuario.
            List<RepositorioClases.Comments> comentarios = CommentsService.ObtenerComentarios()
                .Where(z => z.iDUsuario == UserId)
                .ToList();
            // Reportes creados por el usuario.
            //List<RepositorioClases.Reportes> reportes = ReportServices.ObtenerReportesPorUsuario(UserId).
            //ToList();

            long Rank = 0;
            Rank += eventos.Where(z => z.Estado == RepositorioClases.EventState.Habilitado).Count();
            Rank -= eventos.Where(z => z.Estado == RepositorioClases.EventState.Bloqueado).Count() * 5;
            Rank -= eventos.Where(z => z.Estado == RepositorioClases.EventState.Eliminado).Count() * 3;
            Rank += comentarios.Where(z => z.Estado == RepositorioClases.Estado.Activo).Count() / 20;
            Rank -= comentarios.Where(z => z.Estado == RepositorioClases.Estado.Bloqueado).Count() * 5;
            Rank -= comentarios.Where(z => z.Estado == RepositorioClases.Estado.Eliminado).Count();

            return Rank;
        }
    }
}
