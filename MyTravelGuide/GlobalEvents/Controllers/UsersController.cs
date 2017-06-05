using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Security;
using System.Web.Mvc;
using RepositorioClases;
using ViewModels;
using WebMatrix.WebData;
using Servicios;
using GlobalEvents.Filters;


namespace GlobalEvents.Controllers
{
    [MyAuthorize]
    [InitializeSimpleMembership]
    public class UsersController : Controller
    {
        private Modelo db = new Modelo();

        // GET: Users

        public ViewResult Listado()
        {
            return View(UserService.Get(null).Select(u => new ListUserViewModel()
            {
                Email = u.Email,
                Id = u.Id,
                Usuario = u.Usuario,
                Apellido = u.Apellido,
                Name = u.Nombre,
                Estado = u.Estado
            }).ToList());
        }

        //
        // GET: /User/Details/5
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            if (id == null || UserService.Get(id).Count == 0)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            ListUserViewModel user = UserService.Get((int)id).Select(u => new ListUserViewModel()
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Nombre,
                Apellido = u.Apellido,
                Usuario = u.Usuario,
                Estado = u.Estado,
                Comentarios = CommentsService.ObtenerComentariosUser((int)id).Count,
                Eventos = EventsService.ObtenerEventos(null).Where(z => z.IdUser == id).ToList().Count,
                EventosAsistidos = EventsService.ObtenerEventosAsistidos((int)id).Count,
                ListaDeDeseos = EventsService.ObtenerEventosDeseados((int)id).Count,
                Rank = Rolls.ObtenerRankPorUsuario((int)id) 
            }).FirstOrDefault();

            return View(user);
        }

        //
        // GET: /User/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View(new CreateUserModel());
        }

        //
        // POST: /User/Create

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(CreateUserModel user)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.UserExists(user.Email))
                {
                    ModelState.AddModelError("", "El usuario que intenta registrar ya existe.");
                    return View(user);
                }

                // El usuario va a ser siempre el email, mientras no lo cambie.
                //WebSecurity.CreateUserAndAccount(user.Email, user.Password, new { Email = user.Email}, false);
                String Token = WebSecurity.CreateUserAndAccount(user.Email, user.Password, new { Email = user.Email, Estado = 1 }, true);
                Roles.AddUserToRole(user.Email, "Usuario");
                Email em = new Email();
                em.enviarToken(user.Email, Token);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "El nombre de usuario o la contraseña son incorrectos.");
            user.Password = ""; // Se blanquea la pass.
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int? id)
        {
            // Solo va a poder editar su propio usuario o todos si es admin
            if (id == 0 ||
                (WebSecurity.CurrentUserId != id &&
                !Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin")))
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }

            Users user = UserService.Get(id).Select(u => new Users()
            {
                Email = u.Email,
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Usuario = u.Usuario,
                Estado = u.Estado
            }).FirstOrDefault();

            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(Users user)
        {
            // Solo va a poder editar su propio usuario o todos si es admin
            if (user == null ||
                (WebSecurity.CurrentUserId != user.Id &&
                !Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin")))
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }

            // Solo va a poder cambiar el estado si es admin, por más que lo fuerze.
            if (!Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin"))
            {
                user.Estado = UserService.Get(user.Id).FirstOrDefault().Estado;
            }

            if (ModelState.IsValid)
            {
                UserService.Edit(new Users()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Usuario = user.Usuario,
                    Estado = user.Estado
                });

                return RedirectToAction("Details", "Users", new { id = user.Id });
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5
        // Solo va a poder eliminar un administrador
        [MyAuthorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Users user = UserService.Get(id).Select(u => new Users()
            {
                Email = u.Email,
                Id = u.Id,
                Nombre = u.Nombre,
                Usuario = u.Usuario,
                Apellido = u.Apellido
            }).FirstOrDefault();

            return View(user);
        }

        //
        // POST: /User/Delete/5
        // Solo va a poder eliminar un administrador
        [MyAuthorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserService.Delete(new Users()
            {
                Id = id
            });

            return RedirectToAction("Listado", "Users");
        }

        public ActionResult ChangePassword(int id)
        {
            // Solo va a poder cambiar su propio usuario o todos si es admin
            if (UserService.Get(id).Count == 0 ||
                (WebSecurity.CurrentUserId != id &&
                !Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin")))
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }

            EditPasswordModel user = UserService.Get(id).Select(u => new EditPasswordModel()
            {
                Usuario = u.Usuario
            }).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult ChangePassword(EditPasswordModel editModel)
        {
            // Solo va a poder editar su propio usuario o todos si es admin
            if (UserService.Get(WebSecurity.GetUserId(editModel.Usuario)).Count == 0 ||
                (WebSecurity.CurrentUserName != editModel.Usuario &&
                !Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin")))
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }

            Users user = UserService.Get(WebSecurity.GetUserId(editModel.Usuario)).Select(u => new Users()
            {
                Email = u.Email,
                Id = u.Id,
            }).FirstOrDefault();

            if (!WebSecurity.ChangePassword(user.Email, editModel.actualPassword, editModel.newPassword))
            {
                ModelState.AddModelError("", "La contraseña ingresada es incorrecta.");
                return View(editModel);
            }
            return RedirectToAction("Index", "Home");
        }


        [MyAuthorize]
        public ActionResult ReportarUsuario(int id)
        {
            if (UserService.Get(id).Count == 0)
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            UsersReportes reporte = new UsersReportes { UserId = (int)id };
            return View("ReportarUsuario", reporte);
        }

        [HttpPost]
        [MyAuthorize]
        public ActionResult ReportarUsuario(UsersReportes reporte)
        {
            reporte.IdUsuario = WebSecurity.CurrentUserId;
            ReportServices.CreateReporte(reporte);
            UserService.CambiarEstadoUsuario(reporte.UserId, UserState.Reportado);
            return RedirectToAction("Details", "Users", new { id = reporte.UserId });
        }


        [MyAuthorize(Roles = "Admin")]
        public ActionResult UsuariosReportados()
        {
            var comments = ReportServices.ObtenerUsuariosReportados();
            List<UsersModeracionModel> Lista = new List<UsersModeracionModel>();
            foreach (UsersReportes reporte in comments)
            {
                UsersModeracionModel user = new UsersModeracionModel();
                user.ReporteId = reporte.ReporteId;
                user.UserIdReportado = reporte.UserId;
                user.Usuario = UserService.Get(user.UserIdReportado).FirstOrDefault().Usuario;
                user.Fecha = reporte.Fecha;
                user.IdUsuario = reporte.IdUsuario;
                user.Observacion = reporte.Observacion;
                user.Usuario = UserService.Get(user.IdUsuario).FirstOrDefault().Usuario;
                Lista.Add(user);
                user = null;
            }
            return View(@"UsuariosReportados", Lista);
        }
    }
}
