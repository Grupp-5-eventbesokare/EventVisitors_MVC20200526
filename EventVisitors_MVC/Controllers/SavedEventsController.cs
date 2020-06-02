using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventVisitors_MVC.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.EnterpriseServices;

namespace EventVisitors_MVC.Controllers
{
    public class SavedEventsController : Controller
    {
        // GET: Home
        string BaseUrlBookings = "http://193.10.202.81"; // Eventgruppens IP-adress
        string BaseUrlPlaces = "http://193.10.202.78"; // /EventLokal/api/Places/1

        public async Task<ActionResult> Index(string selectedCategory)
        {
            List<EventsClass> EventsList = new List<EventsClass>();
            //if (Session[Anvandare] != null)
            //{
            //    Anvandare aktuelanvandarefransession = (Anvandare)Session["Anvandare"]
            //}
            using (var ApiClient = new HttpClient())
            {
                ApiClient.BaseAddress = new Uri(BaseUrlBookings);
                ApiClient.DefaultRequestHeaders.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await ApiClient.GetAsync("/BookingService/api/Bookings/User/1"); // Eventgruppens Controller och Get-Metod

                if (Res.IsSuccessStatusCode)
                {
                    var settings = new JsonSerializerSettings // Detta fungerar för att ignorera Null-värden
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var Response = Res.Content.ReadAsStringAsync().Result;
                    EventsList = JsonConvert.DeserializeObject<List<EventsClass>>(Response, settings);
                }

                foreach (var item in EventsList.ToList())
                {
                    DateTime odate = Convert.ToDateTime(item.Event_End_Datetime);
                    DateTime date1 = DateTime.Now;
                    DateTime date2 = odate;
                    int result = DateTime.Compare(date1, date2);

                    if (result > 0)
                    {
                        EventsList.Remove(item);
                    }

                }

                Session["ArrangerId"] = EventsList.Select(m => m.Event_Arranger_Id);

                var kategori = from s in EventsList select s;

                switch (selectedCategory)
                {
                    case "":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Volontär":
                        kategori = kategori.Where(s => s.User_Type.Contains(selectedCategory));
                        break;
                    default:
                        kategori = kategori.OrderBy(s => s.Event_Name);
                        break;
                }

                return View(kategori.ToList());

                // ToDo:
                // Kontrollera om eventet är aktivt eller inte
                // Kontrollera om eventet söker volontär och i sådana fall visa en anmälnings-knapp för det
                // Hämta id för arrangör och hämta arrangörsinfo från platsgruppen (Post)
                // Hämta id för platser och hämta platsinfo från platsgruppen (Post)

            }
        }

        public async Task<ActionResult> GetArrangers() // Försök med att hämta ut en specifik arrangör kopplad till ett specifikt event
        {
            List<OrganizerClass> ArrangerList = new List<OrganizerClass>();
            using (var ApiClient = new HttpClient())
            {
                ApiClient.BaseAddress = new Uri(BaseUrlPlaces);
                ApiClient.DefaultRequestHeaders.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await ApiClient.GetAsync("/EventLokal/api/Organizers/"); // Hårdkodar bara in ett id

                if (Res.IsSuccessStatusCode)
                {
                    var settings = new JsonSerializerSettings // Detta fungerar för att ignorera Null-värden
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var Response = Res.Content.ReadAsStringAsync().Result;
                    ArrangerList = JsonConvert.DeserializeObject<List<OrganizerClass>>(Response);

                    var ArrangerName = ArrangerList.Select(m => m.OrganizerId.ToString() == Session["Arranger_Id"]);

                    return RedirectToAction("Index", ArrangerList);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult AvAnmalan(int eventId, ProfilesClass person, BookingClass bokning)
        {

            //int personId = person.Profile_Id;

            using (var client = new HttpClient())
            {

                int uId = 27;
                int eId = eventId;

                //var User_Type = "Besökare";
                client.BaseAddress = new Uri("http://193.10.202.81");
                var response = client.DeleteAsync("/BookingService/api/Bookings/User/" + uId + "/Event/" +eId ).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Success");

                }
                else
                    Console.Write("Error");

                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ActionResult AvAnmalanVolonter(int eventId, ProfilesClass person, BookingClass bokning)
        {
            //int personId = person.Profile_Id;

            using (var client = new HttpClient())
            {
                int uId = 28;
                int eId = eventId; ;

                //var User_Type = "Besökare";
                client.BaseAddress = new Uri("http://193.10.202.81");
                var response = client.DeleteAsync("/BookingService/api/Bookings/User/" + uId + "/Event/" + eId).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Success");

                }
                else
                    Console.Write("Error");

                return RedirectToAction("Index");

            }

        }


    }


}