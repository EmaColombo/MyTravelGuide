using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RepositorioClases;
using Servicios;
using WebMatrix.WebData;
using MyTravelGuide.Filters;
using System.IO;


namespace MyTravelGuide.Controllers
{
    public class CitiesController : Controller
    {
        private Modelo db = new Modelo();

        // GET: Cities
        [HttpGet]
        public ActionResult Index(long id)
        {
            List<Cities> Lista = CitiesServices.GetCitiesByTravelGuideId(id);
            return View(Lista);
        }

        // GET: Cities/Details/5
        public ActionResult Details(long id)
        {
            ViewModels.CitiesModel model = new ViewModels.CitiesModel();
            model.City = CitiesServices.GetCityById(id);
            model.Images = CitiesServices.GetImagesByCityId(id);
            return View(model);
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.TravelGuides, "TravelGuideId", "TravelGuideName");
            return View();
        }

        // POST: Cities/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize]
        public ActionResult Create(Cities cities)
        {
            if (ModelState.IsValid)
            {
                cities.IdUser = WebSecurity.CurrentUserId;
                cities.CreationDate = DateTime.Now;
                CitiesServices.AddCity(cities);
                
            }

            return RedirectToAction("Details", "TravelGuides", cities.TravelGuideId);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cities cities = db.Cities.Find(id);
            if (cities == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.TravelGuides, "TravelGuideId", "TravelGuideName", cities.Id);
            return View(cities);
        }

        // POST: Cities/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CityName,lat,lng,Descripcion,IdUser,CreationDate,CountryId,TravelGuideId,CityType")] Cities cities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.TravelGuides, "TravelGuideId", "TravelGuideName", cities.Id);
            return View(cities);
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cities cities = db.Cities.Find(id);
            if (cities == null)
            {
                return HttpNotFound();
            }
            return View(cities);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Cities cities = db.Cities.Find(id);
            db.Cities.Remove(cities);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Geocoder(States.CityType cityType, long travelguideid)
        {
             return PartialView(@"~/Views/Cities/Geocodification.cshtml", new Cities() { CityType = cityType, TravelGuideId = travelguideid});
        }

        [HttpGet]
        public ActionResult CitiesList(long travelguideid)
        {
            List<Cities> Cities = CitiesServices.GetCitiesByTravelGuideId(travelguideid);
            ViewModels.CitiesListModel model = new ViewModels.CitiesListModel();
            model.Cities = Cities;
            model.TravelGuideId = travelguideid;
            
            return PartialView(@"~/Views/Cities/CitiesList.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddImageBD(HttpPostedFileBase file, long CityId)
        {
            CitiesServices.AddCityImage(CityId, file);
            return RedirectToAction("AddImage", "Cities", new {CityId = CityId });
        }

        //[HttpGet]
        public ActionResult AddImage(long? CityId)
        {
            ViewModels.ImagesCitiesModel model = new ViewModels.ImagesCitiesModel();
            model.City = CitiesServices.GetCityById((long)CityId);
            model.Images = CitiesServices.GetImagesByCityId((long)CityId);
            return PartialView(@"~/Views/Cities/AddImagesCities.cshtml", model);
        }
    }
}
