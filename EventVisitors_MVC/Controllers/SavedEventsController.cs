using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace EventVisitors_MVC.Controllers
{
    /* [Authorize] */ // Då denna controller endast ska vara tillgänglig om man är inloggad är åtkomsträttigheten "Authorize"

    public class BookingsClass
    {
        public int User_Id { get; set; }

        public int Event_Id { get; set; }

        public string User_Type { get; set; }
    }
    
    class SavedEventsController
    {

        static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {

                BookingsClass b = new BookingsClass { User_Id = 1, Event_Id = 2, User_Type = "Besökare" };
                client.BaseAddress = new Uri("http://localhost:8080/");
                var response = client.PostAsJsonAsync("BookingService/ApplyToBooking", b).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Success");
                }
                else
                    Console.Write("Error");
            }
          
        }
    }
}

             

          
        
    

