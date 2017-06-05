using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using Servicios;
using RepositorioClases;
using GlobalEvents.Filters;
using WebMatrix.WebData;

namespace GlobalEvents.Controllers
{
    [InitializeSimpleMembership]
    [MyAuthorize]
    public class CommentsController : Controller
    {
        
        public ActionResult ComentariosReportados()
        {
            var comments = ReportServices.ObtenerComentariosReportados();
            List<ComentariosEventosModeracionModel> Lista = new List<ComentariosEventosModeracionModel>();
            foreach(CommentsReportes reporte in comments)
            {
                ComentariosEventosModeracionModel comentario = new ComentariosEventosModeracionModel();
                comentario.CommentId = reporte.CommentId;
                comentario.EventId = CommentsService.GetById(comentario.CommentId).EventId;
                comentario.Evento = EventsService.ObtenerEventos(comentario.EventId).FirstOrDefault().NombreEvento;
                comentario.Fecha = reporte.Fecha;
                comentario.IdUsuario = reporte.IdUsuario;
                comentario.Observacion = reporte.Observacion;
                comentario.Usuario = UserService.Get(comentario.IdUsuario).FirstOrDefault().Usuario;
                Lista.Add(comentario);
                comentario = null;
            }
            return View(@"ComentariosReportados", Lista);
        }

        public ActionResult Listado()
        {
            List<RepositorioClases.Comments> Lista = CommentsService.ObtenerComentarios();
            return View(Lista);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public PartialViewResult Create(CommentsModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Se pueden hacer 10 comentarios por día.
                int CantidadComentariosEnDia = CommentsService.ObtenerComentarios(WebSecurity.CurrentUserId)
                    .Where(z => z.Fecha.Year == DateTime.Now.Year)
                    .Where(z => z.Fecha.Month == DateTime.Now.Month)
                    .Where(z => z.Fecha.Day == DateTime.Now.Day)
                    .Count();
                if (CantidadComentariosEnDia < 10)
                    CommentsService.Create(viewModel.Comment, viewModel.IdEvento, WebSecurity.CurrentUserId);
                else
                    ModelState.AddModelError("", "Has llegado al límite para crear de 10 comentarios por día.");
            }

            var comments = CommentsService.ObtenerComentarios(viewModel.IdEvento).Select(u => new ViewModels.Comments()
            {
                iDUsuario = u.iDUsuario,
                CommentId = u.CommentId,
                EventId = u.EventId,
                Comentario = u.Comentario,
                ComentarioPadre = u.ComentarioPadre,
                Like = u.Like,
                UnLike = u.UnLike,
                Fecha = u.Fecha
            }).ToList();

            viewModel = new CommentsModel
            {
                CommentsList = comments,
                Comment = string.Empty
            };

            return PartialView(@"~/Views/Events/CommentsView.cshtml", viewModel);
        }

        public ActionResult ReportarComentario(int id) 
        {
            CommentsReportes reporte = new CommentsReportes{ CommentId = (int)id };
            if (CommentsService.GetById((int)id) == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            return View("ReportarComentario", reporte);
        }

        [HttpPost]
        public ActionResult ReportarComentario(CommentsReportes reporte)
        {
            if (CommentsService.GetById(reporte.CommentId) == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            reporte.IdUsuario = WebSecurity.CurrentUserId;
            ReportServices.CreateReporte(reporte);
            RepositorioClases.Comments comentario = CommentsService.GetById(reporte.CommentId);
            CommentsService.CambiarEstadoComentario(reporte.CommentId, Estado.Reportado);
            return RedirectToAction("Details", "Events", new { id = comentario.EventId });
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            RepositorioClases.Comments comment = CommentsService.GetById(id);
            if (comment == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            ViewModels.Comments comentario = new ViewModels.Comments();
            comentario.Comentario = comment.Comentario;
            comentario.CommentId = comment.CommentId;
            comentario.Estado = comment.Estado;
            comentario.EventId = comment.EventId;
            comentario.Fecha = comment.Fecha;
            comentario.ComentarioPadre = comment.ComentarioPadre;
            comentario.FechaUltimaActualizacion = comment.FechaUltimaActualizacion;
            comentario.iDUsuario = comment.iDUsuario;
            Users user = UserService.Get(comment.iDUsuario).FirstOrDefault();
            comentario.Usuario = user.Nombre;
            return View(comentario);
        }

        [HttpGet]
        public ViewResult ResponderComentarios(string Commentid)
        {
            //Commentid = 2;
            RepositorioClases.Comments coment = CommentsService.GetById(Convert.ToInt64(Commentid));
            var comments = CommentsService.ObtenerComentarios(coment.EventId).Select(u => new ViewModels.Comments()
            {
                iDUsuario = u.iDUsuario,
                CommentId = u.CommentId,
                EventId = u.EventId,
                Comentario = u.Comentario,
                ComentarioPadre = u.ComentarioPadre,
                Like = u.Like,
                UnLike = u.UnLike,
                Fecha = u.Fecha
            }).ToList();

            var viewModel = new CommentsModel
            {
                CommentsList = comments,
                Comment = string.Empty,
                IdComment = Convert.ToInt64(Commentid),
                IdEvento = coment.EventId
                
            };
            return View("ResponderComentarios", viewModel);
        }

        [HttpPost]
        public ActionResult ResponderComentarios(CommentsModel viewModel)
        {
            using (var context = new Modelo())
            {
                var comment = new RepositorioClases.Comments();
                comment.Comentario = viewModel.Comment;
                comment.ComentarioPadre = viewModel.IdComment;
                comment.Estado = Estado.Activo;
                comment.EventId = viewModel.IdEvento;
                comment.Fecha = DateTime.Now;
                comment.FechaUltimaActualizacion = DateTime.Now;
                comment.iDUsuario = WebSecurity.CurrentUserId;
                comment.Like = 0;
                comment.UnLike = 0;
                context.Comments.Add(comment);
                context.SaveChanges();
            }

            
            return RedirectToAction("Details", "Events", new { id = viewModel.IdEvento });
        }
    }
}
