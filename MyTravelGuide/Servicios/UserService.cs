
using RepositorioClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Acciones comunes sobre usuarios.
    /// </summary>
    public static class UserService
    {
        /// <summary>
        /// Creación de usuario
        /// </summary>
        /// <param name="user"></param>
        public static void Create(Users user)
        {
            using (Modelo context = new Modelo())
            {
                context.Users.Add(new Users()
                {
                    Email = user.Email,
                    Usuario = user.Usuario,
                    Id = user.Id,
                    Estado = user.Estado
                });

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene usuarios menos lo que tienen estado eliminados.
        /// </summary>
        /// <param name="id">Busca por id (opcional)</param>
        /// <returns>Lista de usuario/s</returns>
        public static List<Users> Get(int? id)
        {
            using (Modelo context = new Modelo())
            {
                return context.Users
                    .Where(u => id.HasValue ?
                        id.Value == u.Id :
                        true)
                    .Where(z => z.Estado != UserState.Eliminado)
                    .ToList();
            }
        }

        /// <summary>
        /// Editar usuario
        /// </summary>
        /// <param name="user">Usuario a editar</param>
        public static void Edit(Users user)
        {
            using (Modelo context = new Modelo())
            {
                Users users = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                // FirstOrDefault va a intentar recuperar el registro que cumpla la condición
                // si no encuentra ninguno, devuelve NULL, de ahí el siguiente IF.
                if (users != null)
                {
                    users.Email = user.Email;
                    users.Usuario = user.Usuario;
                    users.Apellido = user.Apellido;
                    users.Nombre = user.Nombre;
                    users.Estado = user.Estado;
                }

                // el objeto en memoria persiste los cambios en la base de datos cuando hago un save sobre el contexto.
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Eliminar usuario (Marca como eliminado).
        /// </summary>
        /// <param name="user">Usuario a eliminar</param>
        public static void Delete(Users user)
        {
            using (Modelo context = new Modelo())
            {
                Users users = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                // FirstOrDefault va a intentar recuperar el registro que cumpla la condición
                // si no encuentra ninguno, devuelve NULL, de ahí el siguiente IF.
                if (users != null)
                    users.Estado = UserState.Eliminado;

                // el objeto en memoria persiste los cambios en la base de datos cuando hago un save sobre el contexto.
                context.SaveChanges();
            }
        }
    }
}
