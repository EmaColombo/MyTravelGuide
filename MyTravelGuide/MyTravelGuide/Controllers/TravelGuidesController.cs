using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RepositorioClases;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Web.Helpers;
using Newtonsoft.Json;
using Servicios;
using ViewModels;
using static ViewModels.ExtraModels;

namespace MyTravelGuide.Controllers
{
    public class TravelGuidesController : Controller
    {
        private Modelo db = new Modelo();


        // GET: TravelGuides
        public ActionResult Index()
        {            
            return View(db.TravelGuides.ToList());
        }

        // GET: TravelGuides/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelGuides travelGuides = db.TravelGuides.Find(id);
            if (travelGuides == null)
            {
                return HttpNotFound();
            }
            return View(travelGuides);
        }

        // GET: TravelGuides/Create
        public ActionResult Create()
        {
            TravelGuideModel tg = new TravelGuideModel();
            tg.Model = new TravelGuidesViewModel();
            List<ExtraModels.CountryListModel> Lista = GetCountriesItems().ToList();
            tg.Model.Countries = Lista;
            tg.Model.EndDate = DateTime.Now;
            tg.Model.StartDate = DateTime.Now;
            return View(tg);
        }

        // POST: TravelGuides/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TravelGuideId,TravelGuideName,Description,StartDate,EndDate,IdUser,Direccion,Image,EventState,CreationDate")] TravelGuides travelGuides)
        {
            if (ModelState.IsValid)
            {
                db.TravelGuides.Add(travelGuides);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(travelGuides);
        }

        // GET: TravelGuides/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelGuides travelGuides = db.TravelGuides.Find(id);
            if (travelGuides == null)
            {
                return HttpNotFound();
            }
            return View(travelGuides);
        }

        // POST: TravelGuides/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TravelGuideId,TravelGuideName,Description,StartDate,EndDate,IdUser,Direccion,Image,EventState,CreationDate")] TravelGuides travelGuides)
        {
            if (ModelState.IsValid)
            {
                db.Entry(travelGuides).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(travelGuides);
        }

        // GET: TravelGuides/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelGuides travelGuides = db.TravelGuides.Find(id);
            if (travelGuides == null)
            {
                return HttpNotFound();
            }
            return View(travelGuides);
        }

        // POST: TravelGuides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TravelGuides travelGuides = db.TravelGuides.Find(id);
            db.TravelGuides.Remove(travelGuides);
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

        private List<CountryListModel> GetCountriesItems()
        {
            var allcountries = ExtraServices.GetCountries().Select(c=> new CountryListModel()
            {
                countryname = c.CountryName,
                id = c.CountryId
            });
            

            return allcountries.ToList();

        }
    }
}
