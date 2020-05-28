using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventVisitors_MVC.Controllers
{
    public class SaveController : Controller
    {
        // Gjorde denna för att det inte gick att lägga till en vy på SavedEvents-controllen
        //Bara för att fixa design för Sparade event som visas i en lista.

        //Gjorde vyn utifrån EventsClass, vet inte om vi ska ha en annan klass eller det är det som skall vara?


        //Index för Sparade Event
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ApplyedAsVolonteer()
        {

            return View();
        }
    }
}