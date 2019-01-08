using InsuranceProject.Models;
using InsuranceProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuranceProject.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SignUp2(string firstName, string lastName, string carMake, string carModel, string coverageType, DateTime dob, int carYear, int numOfTickets, bool dui)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(carMake) ||
                string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(coverageType))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                decimal basePrice = 50;
                int age = ((DateTime.Now.Month-dob.Month)+12*(DateTime.Now.Year - dob.Year)) / 12;
                decimal ageAdj = 0;
                if (age < 18 || age > 100)
                {
                    ageAdj = 100;
                }
                else if (age<25 && age>18)
                {
                    ageAdj = 25;
                }
                decimal carYearAdj = 0;
                if (carYear < 2000 || carYear > 2015)
                {
                    carYearAdj = 25;
                }
                decimal carMakeAdj = 0;
                if (carMake == "porsche" || carMake == "Porsche")
                {
                    carMakeAdj = 25;
                }
                decimal carModelAdj = 0;
                if (carModel == "911 carerra" || carModel == "911 Carerra")
                {
                    carModelAdj = 50;
                }
                decimal tixAdj = 0;
                if (numOfTickets > 0)
                {
                    tixAdj = Convert.ToDecimal(10 * numOfTickets);
                }
                decimal prelimTotal = basePrice + ageAdj + carYearAdj + carMakeAdj + carModelAdj + tixAdj;
                decimal duiAdj = 0;
                if (dui == true)
                {
                    duiAdj = prelimTotal * 0.25m;
                }
                decimal finalTotal = prelimTotal + duiAdj;
                if (coverageType == "Full")
                {
                    finalTotal = prelimTotal * 1.50m;
                }
                decimal quote = finalTotal;
                ViewBag.Quote = quote;

                using (Insurance_ProjectEntities db = new Insurance_ProjectEntities())
                {
                    var subscriber = new Subscriber();
                    subscriber.FirstName = firstName;
                    subscriber.LastName = lastName;
                    subscriber.DOB = dob;
                    subscriber.CarYear = carYear;
                    subscriber.CarMake = carMake;
                    subscriber.CarModel = carModel;
                    subscriber.NumOfTickets = numOfTickets;
                    subscriber.DUI = dui;
                    subscriber.CoverageType = coverageType;
                    subscriber.Quote = quote;

                    db.Subscribers.Add(subscriber);
                    db.SaveChanges();
                }
                return View("Quote");
            }   
        }
               

        public ActionResult Quote()
        {
            

            return View();

        }

        public ActionResult Admin()
        {
            using (Insurance_ProjectEntities db = new Insurance_ProjectEntities())
            {

                var subscribers = (from c in db.Subscribers where c.Removed==null select c).ToList();//same function as before but using Linq
                var subscribersVMs = new List<SubscribersVM>();
                foreach (var subscriber in subscribers)
                {
                    var subscriberVM = new SubscribersVM();
                    subscriberVM.Id = subscriber.Id;
                    subscriberVM.FirstName = subscriber.FirstName;
                    subscriberVM.LastName = subscriber.LastName;
                    subscriberVM.DOB = subscriber.DOB;
                    subscriberVM.CarYear = subscriber.CarYear;
                    subscriberVM.CarMake = subscriber.CarMake;
                    subscriberVM.CarModel = subscriber.CarModel;
                    subscriberVM.NumOfTickets = subscriber.NumOfTickets;
                    subscriberVM.DUI = subscriber.DUI;
                    subscriberVM.CoverageType = subscriber.CoverageType;
                    subscriberVM.Quote = Convert.ToDecimal(subscriber.Quote);

                    subscribersVMs.Add(subscriberVM);

                }
                return View(subscribersVMs);

            }

        }

        [HttpPost]
        public ActionResult Remove (int Id)
        {
            using (Insurance_ProjectEntities db = new Insurance_ProjectEntities())
            {
                var subscriber = db.Subscribers.Find(Id);
                subscriber.Removed = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Admin");
        }
    }
}