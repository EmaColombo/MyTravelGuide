using RepositorioClases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        public static bool AddCityImage(long cityid, HttpPostedFileBase file)
        {
            String uriimage = null;
            if (file != null)
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
                string savedFileName = Path.Combine(path, Path.GetFileName(file.FileName));
                file.SaveAs(savedFileName);

                Imagenes imagenes = new Imagenes();
                ImageUploadResult result = imagenes.subirImagen(savedFileName);
                if (result.Status == "OK")
                {
                    uriimage = result.Uri;
                }

                File.Delete(savedFileName);

                ImagesCities image = new ImagesCities();
                image.CityId = cityid;
                image.RutaImagen = uriimage;
                using (Modelo context = new Modelo())
                {
                    context.ImagesCities.Add(image);                 
                    context.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Cities GetCityById(long CityId)
        {
            using(Modelo context = new Modelo())
            {
                return context.Cities.SingleOrDefault(c => c.Id == CityId);
            }
        }

        public static List<ImagesCities> GetImagesByCityId(long CityId)
        {
            using(Modelo context = new Modelo())
            {
                return context.ImagesCities.Where(c => c.CityId == CityId).ToList();
            }
        }
    }
}
