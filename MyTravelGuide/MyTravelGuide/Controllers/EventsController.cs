﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RepositorioClases;
using Servicios;
using MyTravelGuide.Filters;
using WebMatrix.WebData;
using ViewModels;
using System.Web.Security;

namespace MyTravelGuide.Controllers
{
    [InitializeSimpleMembership]
    public class EventsController : Controller
    {
        private Modelo db = new Modelo();

        // GET: Events
        public ActionResult Index()
        {
            // Por el momento acá va a ser destacados, después vemos si mostramos otros.
            List<Events> Lista = EventsService.ObtenerEventos(null, false).Where(z => z.Destacado == true).ToList();
            return View(Lista);
        }

        

        // GET: Events/Details/5
        [HttpGet]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            Events events = null;
            // Obtiene también si esta eliminado si es admin.
            if (Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
                events = EventsService.ObtenerEventos(id, true).FirstOrDefault();
            else
                events = EventsService.ObtenerEventos(id, false).FirstOrDefault();

            if (events == null)
                return Errores.MostrarError(DatosErrores.ErrorParametros);

            EventViewModel.EventModel EventModel = new EventViewModel.EventModel();
            EventViewModel.EventVM model = new EventViewModel.EventVM();
            model.Descripcion = events.Descripcion;
            model.Destacado = events.Destacado;
            model.Direccion = events.Direccion;
            model.Estado = events.Estado;
            model.FechaFin = events.FechaFin;
            model.FechaInicio = events.FechaInicio;
            model.Id = events.Id;
            model.IdCategoria = events.IdCategoria;
            model.IdUser = events.IdUser;
            model.lat = events.lat;
            model.lng = events.lng;
            model.NombreEvento = events.NombreEvento;
            model.RutaImagen = events.RutaImagen;
            model.HoraFin = events.HoraFin;
            model.HoraInicio = events.HoraInicio;
            EventModel.ViewModel = model;
            EventModel.Promedio = ObtenerPromedioPuntuacion((long)id);
            var punt = new PuntuacionesEventos();
            using (Modelo context = new Modelo())
            {
                punt = context.PuntuacionesEventos.SingleOrDefault(C => C.IdUsuario == WebSecurity.CurrentUserId && C.EventId == model.Id);   
            }

            if (punt != null)
            {
                EventModel.Puntuacion = punt.Puntuacion;
            }
            else
            {
                EventModel.Puntuacion = 0;
            }

            return View(EventModel);
        }

        [HttpGet]
        public ActionResult AsistenciaEvento(Int64 IDEvento)
        {
            InteresesEventos interes = EventsService.ObtenerInteresUsuarioEvento(WebSecurity.CurrentUserId, IDEvento);
            InteresesEventosModel intereseseventmodel = new InteresesEventosModel();
            InteresesViewModel intvm = new InteresesViewModel();
            if (interes != null)
            {
                intvm.Anulado = interes.Anulado;
                intvm.EventId = interes.EventId;
                intvm.Fecha = interes.Fecha;
                intvm.FechaAnulacion = interes.FechaAnulacion;
                intvm.IdInteres = interes.IdInteres;
                intvm.Tipo = interes.Tipo;
                intvm.UserId = interes.UserId;
            }
            intereseseventmodel.InteresUsuario = intvm;
            intereseseventmodel.Asistencias = EventsService.ObtenerAsistenciasEvento(IDEvento);
            intereseseventmodel.Interesados = EventsService.ObtenerInteresadosEvento(IDEvento);
            Events evento = EventsService.ObtenerEventos(IDEvento).FirstOrDefault();
            intereseseventmodel.FechaFin = evento.FechaFin;
            intereseseventmodel.IdEvento = IDEvento;
            
            return PartialView(@"~/Views/Events/AsistenciaEvento.cshtml", intereseseventmodel);
        }

        [HttpGet]
        [MyAuthorize(Roles ="Admin")]
        public ActionResult Listado()
        {
            List<Events> Lista = EventsService.ObtenerEventos(null, true);
            return View(Lista);
        }

        // GET: Events/Create
        [MyAuthorize]
        public ActionResult Create()
        {
            EventViewModel.EventCreateModel model = new EventViewModel.EventCreateModel { IdCategoria = Categories.Otros, FechaInicio = DateTime.Now, FechaFin = DateTime.Now};
            return View(model);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize]
        public ActionResult Create([Bind(Include = "NombreEvento,lat,lng,Descripcion,FechaInicio,FechaFin,Direccion,IdCategoria,RutaImagen,HoraInicio,HoraFin")]
        EventViewModel.EventCreateModel events, HttpPostedFileBase file, string HoraInicio, string HoraFin)
        {
            if (ModelState.IsValid)
            {
                // Se pueden hacer 3 eventos por día.
                // Se pueden hacer 10 comentarios por día.
                int CantidadEventosEnDia = EventsService.ObtenerEventos(WebSecurity.CurrentUserId, false)
                    .Where(z => z.FechaCreacion.Year == DateTime.Now.Year)
                    .Where(z => z.FechaCreacion.Month == DateTime.Now.Month)
                    .Where(z => z.FechaCreacion.Day == DateTime.Now.Day)
                    .Count();
                if (CantidadEventosEnDia < 3)
                {
                    TimeSpan Inicio = TimeSpan.Parse(HoraInicio);
                    TimeSpan Fin = TimeSpan.Parse(HoraFin);

                    Events evento = new Events()
                    {
                        Descripcion = events.Descripcion,
                        Direccion = events.Direccion,
                        FechaCreacion = DateTime.Now,
                        FechaFin = events.FechaFin,
                        FechaInicio = events.FechaInicio,
                        IdCategoria = events.IdCategoria,
                        IdUser = WebSecurity.CurrentUserId,
                        lat = events.lat,
                        lng = events.lng,
                        RutaImagen = events.RutaImagen,
                        HoraFin = events.HoraFin,
                        HoraInicio = events.HoraInicio,
                        NombreEvento = events.NombreEvento,
                        Estado = Rolls.ObtenerEstadoEventoPorUsuario(WebSecurity.CurrentUserId) ?
                                    States.EventState.Habilitado :
                                    States.EventState.Pendiente_De_Aprobacion,
                        Destacado = Rolls.ObtenerSiEventoEsDestacado(WebSecurity.CurrentUserId)
                    };

                    EventsService.Create(evento, file);
                    return RedirectToAction("Details", "Events", new { id = EventsService.ObtenerEventos(WebSecurity.CurrentUserId).Max(z => z.Id) });
                }
                else
                    ModelState.AddModelError("", "Has llegado al límite para crear de 3 eventos por día.");
            }

            return View(events);
        }

        // GET: Events/Edit/5
        [MyAuthorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            Events events = EventsService.ObtenerEventos(id).FirstOrDefault();
            if (events == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            // Solo si el usuario es el creador del evento o es administrador puede editarlo.
            if (events.IdUser == WebSecurity.CurrentUserId || Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
            {
                return View(events);
            }
            else
            {
                return Errores.MostrarError(DatosErrores.Permisos);
            }
            
        }

        // POST: Events/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize]
        public ActionResult Edit([Bind(Include = "Id,NombreEvento,lat,lng,Descripcion,FechaInicio,FechaFin,IdUser,Estado, Destacado, Direccion, IdCategoria, RutaImagen, HoraInicio, HoraFin")] Events events,
            HttpPostedFileBase file, string HoraInicio, string HoraFin)
        {
            if (ModelState.IsValid)
            {
                TimeSpan Inicio = TimeSpan.Parse(HoraInicio);
                TimeSpan Fin = TimeSpan.Parse(HoraFin);
                events.IdUser = WebSecurity.CurrentUserId;

                // Solo si el usuario es el creador del evento o es administrador puede editarlo.
                // Por las dudas que se haga un post directamente.
                if (events.IdUser == WebSecurity.CurrentUserId || Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
                {
                    // Solo va a poder cambiar el estado si es admin, por más que lo fuerze.
                    if (!Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
                    {
                        events.Estado = EventsService.ObtenerEventos(events.Id).FirstOrDefault().Estado;
                    }
                    EventsService.Edit(events, file, Inicio, Fin);
                    return RedirectToAction("Details", "Events", new { id = EventsService.ObtenerEventos(WebSecurity.CurrentUserId).Max(z => z.Id) });
                }
                else
                {
                    return Errores.MostrarError(DatosErrores.Permisos);
                }
            }
            return View(events);
        }

        // GET: Events/Delete/5
        [MyAuthorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            Events events = EventsService.ObtenerEventos(id).FirstOrDefault();
            if (events == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            if (events.IdUser == WebSecurity.CurrentUserId || Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
            {
                return View(events);
            }
            else
            {
                return Errores.MostrarError(DatosErrores.Permisos);
            }
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [MyAuthorize]
        public ActionResult DeleteConfirmed(long id)
        {
            Events events = EventsService.ObtenerEventos(id).FirstOrDefault();
            if (events == null)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            if (events.IdUser == WebSecurity.CurrentUserId || Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
            {
                EventsService.Delete(events);
                return RedirectToAction("Index");
            }
            else
            {
                return Errores.MostrarError(DatosErrores.Permisos);
            }
        }


        [HttpPost]
        [MyAuthorize]
        public ActionResult EnviarInteres(long id, int Tipo)
        {

            InteresesEventos interes = new InteresesEventos();
            using (Modelo context = new Modelo())
            {
                interes = context.InteresesEventos.SingleOrDefault(c => c.EventId == id && c.UserId == WebSecurity.CurrentUserId && c.Anulado == false);
                if (interes != null)
                {
                    if (interes.Tipo == Intereses.Me_Gusta && Tipo == 1)
                    {
                        context.InteresesEventos.Remove(interes);
                        context.SaveChanges();
                        interes = new InteresesEventos();
                        interes.EventId = id;
                        interes.Fecha = DateTime.Now;
                        if (Tipo == 1)
                            interes.Tipo = Intereses.Asistire;
                        else
                            interes.Tipo = Intereses.Me_Gusta;

                        interes.UserId = WebSecurity.CurrentUserId;
                        context.InteresesEventos.Add(interes);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.InteresesEventos.Remove(interes);
                        context.SaveChanges();
                    }
                    
                }
                else
                {
                    interes = new InteresesEventos();
                    interes.EventId = id;
                    interes.Fecha = DateTime.Now;
                    if (Tipo == 1)
                    {
                        interes.Tipo = Intereses.Asistire;
                    }
                    else
                    {
                        interes.Tipo = Intereses.Me_Gusta;
                    }
                    interes.UserId = WebSecurity.CurrentUserId;
                    context.InteresesEventos.Add(interes);
                    context.SaveChanges();
                }
                
            }
            

            return RedirectToAction("AsistenciaEvento", new { IdEvento = id });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [MyAuthorize]
        public ActionResult MapView()
        {
            return View();
        }


        public JsonResult GetEventsForMap(int? id, string lat, string lng)
        {
            using (Modelo context = new Modelo())
            {   
                double maxlng = Convert.ToDouble(lng.Replace(".",",")) + 0.05;
                double minlng = Convert.ToDouble(lng.Replace(".", ",")) - 0.05;
                double maxlat = Convert.ToDouble(lat.Replace(".", ",")) + 0.05;
                double minlat = Convert.ToDouble(lat.Replace(".", ",")) - 0.05;
                context.Configuration.LazyLoadingEnabled = false;
                List<Events> eventos = context.Events.Where(u => u.Estado == States.EventState.Habilitado && 
                u.FechaInicio.Day == DateTime.Now.Day && u.FechaInicio.Month == DateTime.Now.Month && u.FechaInicio.Year == DateTime.Now.Year).ToList();
                if (eventos.Count > 0)
                {
                    eventos = eventos.Where(c => Convert.ToDouble(c.lat.Replace(".", ",")) <= maxlat && Convert.ToDouble(c.lat.Replace(".", ",")) >= minlat).ToList();
                    eventos = eventos.Where(c => Convert.ToDouble(c.lng.Replace(".", ",")) <= maxlng && Convert.ToDouble(c.lng.Replace(".", ",")) >= minlng).ToList();
                }
                else
                {
                    eventos = new List<Events>();
                }
                return Json(
                eventos,
                JsonRequestBehavior.AllowGet);
            }
        }

        

        [MyAuthorize]
        public ActionResult ReportarEvento(int id)
        {
            if (EventsService.ObtenerEventos((long)id).Count == 0)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            EventsReportes reporte = new EventsReportes { EventId = (int)id };
            return View("ReportarEvento", reporte);
        }

        

        [HttpPost]
        [MyAuthorize]
        public void PuntuarEvento(long Id, int Puntuacion)
        {
            PuntuacionesEventos punt = new PuntuacionesEventos();
            using (Modelo context = new Modelo())
            {
                punt = context.PuntuacionesEventos.SingleOrDefault(C => C.IdUsuario == WebSecurity.CurrentUserId && C.EventId == Id);
                if(punt != null)
                {
                    punt.Puntuacion = Puntuacion;
                    context.SaveChanges();
                }
                else
                {
                    punt = new PuntuacionesEventos();
                    punt.EventId = Id;
                    punt.Puntuacion = Puntuacion;
                    punt.IdUsuario = WebSecurity.CurrentUserId;
                    context.PuntuacionesEventos.Add(punt);
                    context.SaveChanges();
                }
            }

            //return RedirectToAction("Details", new { id = Id });
        }

        public Decimal ObtenerPromedioPuntuacion(long id)
        {
            return EventsService.ObtenerPuntuacionPromedio(id);
        }
    }
}