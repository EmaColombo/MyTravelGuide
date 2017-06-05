using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositorioClases;

namespace Servicios
{
    public static class ReportServices
    {
        /// <summary>
        /// Obtener Lista Eventos Reportados
        /// </summary>
        /// <returns></returns>
        public static List<EventsReportes> ObtenerEventosReportados()
        {
            using (Modelo context = new Modelo())
            {
                return context.EventsReportes.Where(z => z.Resuelto == false).ToList();
            }
        }

        /// <summary>
        /// Obtener lista de comentarios reportados
        /// </summary>
        /// <returns></returns>
        public static List<CommentsReportes> ObtenerComentariosReportados()
        {
            using (Modelo context = new Modelo())
            {
                return context.CommentsReportes.Where(z => z.Resuelto == false).ToList();
            }
        }

        public static List<UsersReportes> ObtenerUsuariosReportados()
        {
            using (Modelo context = new Modelo())
            {
                return context.UsersReportes.Where(z => z.Resuelto == false).ToList();
            }
        }

        /// <summary>
        /// Reportar un evento
        /// </summary>
        /// <param name="reporte">Evento</param>
        public static void CreateReporte(EventsReportes reporte)
        {
            using (Modelo context = new Modelo())
            {
                context.EventsReportes.Add(new EventsReportes()
                {
                    EventId = reporte.EventId,
                    Fecha = DateTime.Now,
                    IdUsuario = reporte.IdUsuario,
                    Observacion = reporte.Observacion,
                    Resuelto = false
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Reportar un comentario
        /// </summary>
        /// <param name="reporte">Comentario</param>
        public static void CreateReporte(CommentsReportes reporte)
        {
            using (Modelo context = new Modelo())
            {
                context.CommentsReportes.Add(new CommentsReportes()
                {
                    CommentId = reporte.CommentId,
                    Fecha = DateTime.Now,
                    IdUsuario = reporte.IdUsuario,
                    Observacion = reporte.Observacion,
                    Resuelto = false
                });
                context.SaveChanges();
            }
        }

        public static void CreateReporte(UsersReportes reporte)
        {
            using (Modelo context = new Modelo())
            {
                context.UsersReportes.Add(new UsersReportes()
                {
                    UserId = reporte.UserId,
                    Fecha = DateTime.Now,
                    IdUsuario = reporte.IdUsuario,
                    Observacion = reporte.Observacion,
                    Resuelto = false
                });
                context.SaveChanges();
            }
        }

        public static void ResolverReportesComentarios(int idComentario)
        {
            using (Modelo context = new Modelo())
            {
                List<CommentsReportes> reportes = context.CommentsReportes
                    .Where(z => z.CommentId == idComentario)
                    .Where(z => z.Resuelto == false)
                    .ToList();
                foreach(var a in reportes)
                {
                    a.Resuelto = true;
                }
                context.SaveChanges();
            }
        }

        public static void ResolverReportesEventos(int idEvento)
        {
            using (Modelo context = new Modelo())
            {
                List<EventsReportes> eventos = context.EventsReportes
                    .Where(z => z.EventId == idEvento)
                    .Where(z => z.Resuelto == false)
                    .ToList();
                foreach (var a in eventos)
                {
                    a.Resuelto = true;
                }
                context.SaveChanges();
            }
        }

        public static Reportes ObtenerReportesPorUsuario(int UserId)
        {
            Reportes reportes = new Reportes();
            using (Modelo context = new Modelo())
            {
                reportes.Comentarios = context.CommentsReportes.Where(z => z.IdUsuario == UserId).ToList();
                reportes.Eventos = context.EventsReportes.Where(z => z.IdUsuario == UserId).ToList();
                reportes.Usuarios = context.UsersReportes.Where(z => z.IdUsuario == UserId).ToList();
            }
            return reportes;
        }
    }
}
