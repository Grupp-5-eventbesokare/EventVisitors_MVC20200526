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
            //if (Session[Anvandare] != null)
            //{
            //    Anvandare aktuelanvandarefransession = (Anvandare)Session["Anvandare"]
            //}
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
                        kategori = kategori.OrderBy(s => s.Event_Arranger_Id);
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

        public ActionResult Anmalan (int Event_Id, int Profile_Id)
        {
            {
                using (var client = new HttpClient())
                {
                    
                    client.BaseAddress = new Uri("http://193.10.202.77");
                    var response = client.PostAsJsonAsync("/api/Bookings/" + Event_Id, Profile_Id ).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                        return View();
                    }
                    else
                        Console.Write("Error");
                    return View();

                }

            }
        }

        public ActionResult VolonterAnmalan(int Event_Id, int Profile_Id, string Volonter)
        {
            {

                using (var client = new HttpClient())
                {
                    var User_Type = "Volonteer";
                    client.BaseAddress = new Uri("http://193.10.202.77");
                    var response = client.PostAsJsonAsync("/api/Bookings/Event/" + Event_Id + "/Volounteer", Profile_Id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                      
                    }
                    else
                        Console.Write("Error");
                   
                    var response2 = client.PostAsJsonAsync(" / api / Bookings / Event / " + Event_Id + " / Volounteer", User_Type).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                        return View();
                    }
                    else
                        Console.Write("Error");
                    return View();

                }

            }
        }

        /*string BaseUrlBooking = "http://localhost:8080"; // Här tänker jag att vi ska POST:a till ApplyToEvent (bokningsgruppen)
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromUri] int User_Id, int Event_Id, string User_Type)
        {
            int U_id = 1;
            U_id = User_Id;
            int E_id = 2;
            E_id = Event_Id;
            string U_Type = "Besökare";
            U_Type = User_Type;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var success = true;
            List<object> lista = new List<object>();
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var requestContents in provider.Contents)
            {
                if (requestContents.Headers.ContentDisposition.Name == "customer")
                {
                    Customer q1 = JsonConvert.DeserializeObject<Customer>(requestContents.ReadAsStringAsync().Result);
                    lista.Add(q1);
                }
                else if (requestContents.Headers.ContentDisposition.Name == "order")
                {
                    Order q1 = JsonConvert.DeserializeObject<Order>(requestContents.ReadAsStringAsync().Result);
                    lista.Add(q1);
                }
            }
            List<object> newshowlista = lista;
            HttpResponseMessage result = Request.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, success);
            return result;
        } */

    }
    

}