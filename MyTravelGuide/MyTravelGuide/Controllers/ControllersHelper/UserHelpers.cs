using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalEvents.Helpers
{
    public static class UserHelpers
    {
        /// <summary>
        /// Obtiene el nombre de usuario pasandole el ID.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getNameUser(this HtmlHelper html, string id)
        {
            List<RepositorioClases.Users> Usuario = Servicios.UserService.Get(Int32.Parse(id));
            String Name = "";
            if (Usuario.Count > 0)
                Name = Usuario.First().Usuario != null ? (Usuario.First().Usuario.Contains("@") ? Usuario.First().Usuario.Substring(0, Usuario.First().Usuario.IndexOf("@")) : Usuario.First().Usuario ) : Usuario.First().Nombre + Usuario.First().Apellido;
            return Name == "" ? "Anónimo" : Name;
        }
    }
}