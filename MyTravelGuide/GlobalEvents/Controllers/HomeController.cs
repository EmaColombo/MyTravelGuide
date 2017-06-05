using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using WebMatrix.WebData;
using GlobalEvents.Filters;
using RepositorioClases;
using System.Web.Security;
using System.Data.SqlClient;

namespace GlobalEvents.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(String returnURL)
        {
            ViewBag.returnURL = returnURL;
            if (WebSecurity.IsAuthenticated) // Si ya esta autenticado y quiere ingresar igualmente a esta página, redirecionamos a index.
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, String returnURL)
        { 
            ViewBag.returnURL = returnURL;
            if (!WebSecurity.UserExists(model.Email))
            {
                ModelState.AddModelError("", "El nombre de usuario o la contraseña son incorrectos.");
                return View();
            }

            // Verifica que no este bloqueado, eliminado, ni a falta de verificación token.
            using (Modelo context = new Modelo())
            {
                Users users = context.Users.Where(u => u.Email == model.Email).FirstOrDefault();
                if (users != null)
                {
                    if (users.Estado == UserState.Bloqueado || users.Estado == UserState.Eliminado || users.Estado == UserState.Inactivo)
                    {
                        ModelState.AddModelError("", "La cuenta se encuentra " + users.Estado.ToString());
                        return View();
                    }
                }
            }

            if (ModelState.IsValid && WebSecurity.Login(model.Email, model.Password, model.Recordarme))
            {
                if (Url.IsLocalUrl(returnURL))
                {
                    return Redirect(returnURL);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "El nombre de usuario o la contraseña son incorrectos.");
                return View();
            }
        }

        [HttpGet]
        [MyAuthorize]
        public ActionResult Logout(String returnURL)
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");

            /* Ver como manejar si la página no esta autorizada luego de desloguear.
            if (Url.IsLocalUrl(returnURL))
            {
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            */
        }

        [HttpGet]
        public ActionResult VerifyAccount(ChangesModel model)
        {
            return View(model);
        }

        [HttpGet]
        public ActionResult ValidarToken(String Token = "")
        {
            if (Token == "")
            {
                return Errores.MostrarError(DatosErrores.ErrorParametros);
            }
            ChangesModel model = new ChangesModel()
            {
                Cambio = Changes.Verificacion_Cuenta,
                Success = WebSecurity.ConfirmAccount(Token)
            };
            return RedirectToAction("VerifyAccount", model);
        }

        public FullModel SearchResults(string Busqueda)
        {
            FullModel model = new FullModel();
            if (Busqueda != null)
            {
                using (var modelo = new Modelo())
                {
                    //Get student name of string type
                    var resultado = modelo.Database.SqlQuery<FullSearchModel>("sp_BusquedaFullText @Busqueda", new SqlParameter("Busqueda", Busqueda));
                    //Or can call SP by following way
                    //var courseList = ctx.Courses.SqlQuery("exec GetCoursesByStudentId @StudentId ", idParam).ToList<Course>();
                    
                    model.Lista = resultado.ToList();
                    model.searchString = Busqueda;
                    
                }
            }
            return model;
        }

        [HttpGet]
        public ActionResult Search()
        {
           return View(new FullModel());
        }

        [HttpPost]
        public ActionResult Search(FullModel viewModel)
        {
            FullModel Result = SearchResults(viewModel.searchString);
            return View("Search", Result);
        }



    }
}