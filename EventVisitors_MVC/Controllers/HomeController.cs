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
    public class HomeController : Controller
    {
        // GET: Home
        string BaseUrlEvents = "http://193.10.202.77"; // Eventgruppens IP-adress
        string BaseUrlPlaces = "http://193.10.202.78"; // /EventLokal/api/Places/1
        
        public async Task<ActionResult> Index(string selectedCategory)
        {
            List<EventsClass> EventsList = new List<EventsClass>();
            using (var ApiClient = new HttpClient())
            {
               ApiClient.BaseAddress = new Uri(BaseUrlEvents);
               ApiClient.DefaultRequestHeaders.Clear();
               ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               HttpResponseMessage Res = await ApiClient.GetAsync("/EventService/Api/Events"); // Eventgruppens Controller och Get-Metod

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
                    case "Musik":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Bingo":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Festival":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Loppis":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Klädbytardag":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Övrigt":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Högtid":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Kultur":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Utbildning":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Tävling":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Utställning":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Motion":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Film":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Barn & Familj":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Marknad":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Konsert":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Ospecificerad":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "Sport":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    case "":
                        kategori = kategori.Where(s => s.Event_Category.Category_Name.Contains(selectedCategory));
                        break;
                    default:
                        kategori = kategori.OrderBy(s => s.Event_Organizer.Id);
                        break;
                }

                return View(kategori.ToList());

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
                HttpResponseMessage Res = await ApiClient.GetAsync("/EventLokal/api/Organizers/"); 

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
        public ActionResult Anmalan (int eventId, ProfilesClass person, BookingClass bokning)
        {        
                using (var client = new HttpClient())
                {
                string id = Session["User_Id_Profile"].ToString();
                int ProfileId = Int32.Parse(id);
                int uId = ProfileId;
                int eId = eventId;
                BookingClass b = new BookingClass { User_Id = uId, Event_Id = eId, User_Type = "Besökare" };

                    client.BaseAddress = new Uri("http://193.10.202.81");
                    var response = client.PostAsJsonAsync("/BookingService/api/Bookings/", b).Result;
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
        public ActionResult VolonterAnmalan(int eventId, ProfilesClass person, BookingClass bokning)
        {
                using (var client = new HttpClient())
                {

                string id = Session["User_Id_Profile"].ToString();
                int User_ProfileId = Int32.Parse(id);
                BookingClass b = new BookingClass { User_Id = User_ProfileId, Event_Id = eventId, User_Type = "Volontär" };

                client.BaseAddress = new Uri("http://193.10.202.81");
                    var response = client.PostAsJsonAsync("/BookingService/api/Bookings/", b).Result;
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