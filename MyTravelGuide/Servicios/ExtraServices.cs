using RepositorioClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public static class ExtraServices
    {
        public static List<Countries> GetCountries()
        {
            var Countries = new List<Countries>();
            using (Modelo context = new Modelo())
            {
                Countries = context.Countries.ToList();
                return Countries.ToList();
            }
        }

    }
}
