using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Servicios;
using GlobalEvents.Filters;
using RepositorioClases;

namespace GlobalEvents.Controllers
{
    [InitializeSimpleMembership]
    [MyAuthorize(Roles = "Admin")]
    public class ManageController : Controller
    {   
        public ActionResult Reportes()
        {
            List<CommentsReportes> Comentarios = ReportServices.ObtenerComentariosReportados();
            List<EventsReportes> Eventos = ReportServices.ObtenerEventosReportados();
            List<UsersReportes> Usuarios = UserService.ObtenerUsuariosReportados();
            var a = new Reportes();
            a.Comentarios = Comentarios;
            a.Eventos = Eventos;
            a.Usuarios = Usuarios;
            return View(a);
        }

        [HttpGet]
        /// <summary>
        /// el id es el del comentario
        /// </summary>
        public ActionResult CommentReported(int? id)
        {
            if (id == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            // Obtiene el comentario.
            Comments comment = CommentsService.GetById((int)id);
            // Si no hay comentario con ese id.
            if (comment == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }

            // Genera objeto con comentario y todos los reportes.
            CommentsDetailsReport reporte = new CommentsDetailsReport();
            reporte.Comentario = comment;
            List<CommentsReportes> Reportes = ReportServices.ObtenerComentariosReportados().Where(z => z.CommentId == id).Where(z => z.Resuelto == false).ToList();
            reporte.Reportes = Reportes;
            return View(reporte);
        }

        [HttpPost]
        public ActionResult CommentReported([Bind(Prefix = "Comentario", Include = "CommentId,Estado")] Comments comm)
        {
            if (CommentsService.GetById(comm.CommentId) == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            CommentsService.CambiarEstadoComentario(comm.CommentId, comm.Estado);
            return RedirectToAction("Details", "Comments", new { id = comm.CommentId });
        }

        [HttpGet]
        public ActionResult CerrarReporteComentario(int ComentarioId)
        {
            if (CommentsService.GetById(ComentarioId) == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            ReportServices.ResolverReportesComentarios(ComentarioId);
            return RedirectToAction("Details", "Comments", new { id = ComentarioId });
        }

        ///  Parte de eventos

        [HttpGet]
        /// <summary>
        /// el id es el del comentario
        /// </summary>
        public ActionResult EventReported(int id)
        {
            // Obtiene el evento.
            Events evento = EventsService.ObtenerEventos((long)id).SingleOrDefault();
            if (evento == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            // Genera objeto con evento y todos los reportes.
            EventsDetailsReport reporte = new EventsDetailsReport();

            reporte.Eventos = evento;
            List<EventsReportes> Reportes = ReportServices.ObtenerEventosReportados().Where(z => z.EventId == id).Where(z => z.Resuelto == false).ToList();
            reporte.Reportes = Reportes;
            return View(reporte);
        }

        [HttpPost]
        public ActionResult EventReported([Bind(Prefix = "Eventos", Include = "Id,Estado")] Events even)
        {
            EventsService.CambiarEstadoEvento(even.Id, even.Estado);
            return RedirectToAction("Details", "Events", new { id = even.Id });
        }

        [HttpGet]
        public ActionResult CerrarReporteEventos(int? EventoId)
        {
            if (EventoId == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            ReportServices.ResolverReportesEventos((int)EventoId);
            return RedirectToAction("Details", "Events", new { id = EventoId });
        }

        //// Parte de usuarios

        [HttpGet]
        /// <summary>
        /// el id es el del usuario
        /// </summary>
        public ActionResult UserReported(int id)
        {
            // Obtiene el evento.
            Users usuario = UserService.Get(id).SingleOrDefault();
            if (usuario == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            // Genera objeto con evento y todos los reportes.
            UsersDetailReport reporte = new UsersDetailReport();

            reporte.Usuarios = usuario;
            List<UsersReportes> Reportes = ReportServices.ObtenerUsuariosReportados().Where(z => z.IdUsuario == id).Where(z => z.Resuelto == false).ToList();
            reporte.Reportes = Reportes;
            return View(reporte);
        }

        [HttpPost]
        /// <summary>
        /// Se realiza cuando se hace un post con el sumbit en esa vista
        /// </summary>
        public ActionResult UserReported([Bind(Prefix = "Users", Include = "Id,Estado")] Users user)
        {
            UserService.CambiarEstadoUsuario(user.Id, user.Estado);
            return RedirectToAction("Details", "Users", new { id = user.Id });
        }

        [HttpGet]
        public ActionResult CerrarReporteUsuario(int? UserId)
        {
            if (UserId == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            UserService.ResolverReportesUsuarios((int)UserId);
            return RedirectToAction("Details", "Users", new { id = UserId });
        }

    }
}
