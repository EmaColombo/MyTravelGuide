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
    public class TravelGuideServices
    {
        public static List<TravelGuides> GetList()
        {
            using (Modelo context = new Modelo())
            {
                return context.TravelGuides.ToList();
            }
        }

        public static bool Create(TravelGuides travelg, HttpPostedFileBase file)
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
                    travelg.Image = uriimage;
                }

                File.Delete(savedFileName);
            }
            using (Modelo context = new Modelo())
            {
                context.TravelGuides.Add(travelg);
                context.SaveChanges();
            }
            return true;
        }
    }
}
