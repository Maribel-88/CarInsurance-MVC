using CarInsurance.Models;
using CarInsurance.Models.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insurees = db.Insurees;
                var insureeQuoteVms = new List<InsureeQuoteVm>();
                // List<Insuree> insuree = db.Insurees.ToList(); // Creates a List of all quotes from the DB.

                //var insureeQuoteVm = new List<InsureeQuoteVm>(); // Instantiates a View Model List, DB info is mapped into it.
                // Maps pertinent info from the DB to the View Model.
                foreach (var insuree in insurees)
                {
                    InsureeQuoteVm insureeQuoteVm = new InsureeQuoteVm();
                    insureeQuoteVm.FirstName = insuree.FirstName;
                    insureeQuoteVm.LastName = insuree.LastName;
                    insureeQuoteVm.EmailAddress = insuree.EmailAddress;
                    insureeQuoteVm.DateOfBirth = insuree.DateOfBirth;
                    insureeQuoteVm.CarYear = insuree.CarYear;
                    insureeQuoteVm.CarMake = insuree.CarMake;
                    insureeQuoteVm.EmailAddress = insuree.EmailAddress;
                    insureeQuoteVm.DateOfBirth = insuree.DateOfBirth;
                    insureeQuoteVm.CarYear = insuree.CarYear;
                    insureeQuoteVm.CarMake = insuree.CarMake;
                    insureeQuoteVm.Quote = CalcQuote(insureeQuoteVm);

                    insureeQuoteVms.Add(insureeQuoteVm); // Add the quote into the View Model.
                    db.SaveChanges();
                }

                

                return View(insureeQuoteVms);
            }
        }


        private decimal CalcQuote(InsureeQuoteVm insureeQuoteVm)
        {
            decimal total = 50m; // base of $50

            TimeSpan timespan = DateTime.Now - insureeQuoteVm.DateOfBirth;
            int years = Convert.ToInt32(timespan.Days) / 365;
            if (years < 25)
            {
                if (years < 18) total += 100; // Add 100 if under 18 years old.
                if (years > 18 || years < 26) total += 25; // Add 25 if under 25 but over 18 years old.
                else total += 25; // Add 25 if over 26 years old.
            }
            if (years > 100) total += 25; // Add $25 if over 100 years old.

            if (insureeQuoteVm.CarYear < 2000 || insureeQuoteVm.CarYear > 2015) total += 25; // Add $25 if car older than 2000 or newer than 2015.

            if (insureeQuoteVm.CarMake.ToLower() == "porsche")
            {
                total += 25; // Add $25 if car is a Porsche.
            if (insureeQuoteVm.CarModel.ToLower() == "911 carrera") total += 25; // Add another $25 if Porsche is a 911 Carrera.
            }
            for (int i = 0; i < insureeQuoteVm.SpeedingTickets; i++)
            {
                total += 10; //Add $10 for every speeding ticket.
            }
            if (insureeQuoteVm.DUI) total = total * 1.25m; // Add 25% if they have a DUI.
            if (insureeQuoteVm.CoverageType) total = total * 1.5m; // Add 50% for Full Coverage.

            total = Decimal.Ceiling(total * 100); //Ensure total is rounded up.
            total = total / 100; // Ensure total only shows two decimal places.

            return total;
        }
    }
}