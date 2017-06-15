using RepositorioClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public static class CitiesServices
    {

        public static bool AddCity(Cities city)
        {
            using (Modelo context = new Modelo())
            {
                context.Cities.Add(city);
                context.SaveChanges(); 
            }
            return true;
        }
    }
}
