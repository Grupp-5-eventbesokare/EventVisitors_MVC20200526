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

namespace EventVisitors_MVC.Controllers
{
   /* [Authorize]*/ // Då denna controller endast ska vara tillgänglig om man är inloggad är åtkomsträttigheten "Authorize"

    public class ProfileController : Controller
    {
        // GET: Profile
        string BaseUrl = "http://localhost:19779";
        public async Task<ActionResult> Index()
        { 
            ProfilesClass Profile;
            using (var ApiClient = new HttpClient())
            {
                ApiClient.BaseAddress = new Uri(BaseUrl);
                ApiClient.DefaultRequestHeaders.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await ApiClient.GetAsync("/api/MyProfile/1"); // Hårdkodar bara in ett id

                if (Res.IsSuccessStatusCode)
                {
                    var settings = new JsonSerializerSettings // Detta fungerar för att ignorera Null-värden
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    Profile = JsonConvert.DeserializeObject<ProfilesClass>(Response);

                    return View(Profile);
                }
                else
                {
                    return View();
                }

            }

        }            
        
    }
}