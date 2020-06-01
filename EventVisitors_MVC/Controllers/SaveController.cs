using System;
using System.Net.Http;
using System.Web.Mvc;

namespace EventVisitors_MVC.Controllers
{
    public class SaveController : Controller
    {
        // Gjorde denna för att det inte gick att lägga till en vy på SavedEvents-controllen
        //Bara för att fixa design för Sparade event som visas i en lista.

        //Gjorde vyn utifrån EventsClass, vet inte om vi ska ha en annan klass eller det är det som skall vara?

        

        /*class SaveEventsController
        {
            static void Main(string[] args)
            {
                using (var client = new HttpClient())
                {

                    BookingClass b = new BookingClass { Profile_Id = 1, Event_Id = 2, Profile_Role = "Besökare" };
                    client.BaseAddress = new Uri("http://193.10.202.81/BookingService");
                    var response = client.PostAsJsonAsync("/api/Bookings", b).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                    }
                    else
                    {
                        Console.Write("Error");
                    }

                }

            }
        }

        //Index för Sparade Event
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ApplyedAsVolonteer()
        {

            return View();
        }*/
    }
}