using EventVisitors_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EventVisitors_MVC.Controllers
{
    public class RegistrationController : Controller
    {
        public ActionResult RegistrationUser()
        {

            return View();
        }

        [HttpPost] //Skickar värderna som användaren skriver in
        public async Task<ActionResult> RegistrationUser(ProfilesClass registration)
        {

            registration.Profile_Role = "Besökare"; // Besökare blir standardroll för alla som registrerar sig
            
            using (var client = new HttpClient())
            {
                RegistrationClass Registration = new RegistrationClass { Email = registration.Profile_Email, Firstname = registration.Profile_Firstname, Lastname = registration.Profile_Lastname, Password = registration.Profile_Password, Role = registration.Profile_Role };

                client.BaseAddress = new Uri("http://193.10.202.76/api/");

                HttpResponseMessage postTask = await client.PostAsJsonAsync("visitor", Registration);

                if (postTask.IsSuccessStatusCode)
                {
                    var result = postTask.Content.ReadAsStringAsync().Result;

                    registration.Profile_User_Id = Int32.Parse(result.ToString());

                    ProfilesClass b = new ProfilesClass { Profile_Email = registration.Profile_Email, Profile_Firstname = registration.Profile_Firstname, Profile_Lastname = registration.Profile_Lastname, Profile_PhoneNr =registration.Profile_PhoneNr, Profile_Birthday = registration.Profile_Birthday, Profile_Role =registration.Profile_Role, Profile_User_Id=registration.Profile_User_Id};
                    SaveProfile(b);
                    return RedirectToAction("LoginUser", "Login");
                }
                
                return View(registration);
            }
        }

        private void SaveProfile(ProfilesClass newProfile)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.82/MyProfile/api/");
                var postTask = client.PostAsJsonAsync("Profiles", newProfile);

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Profilen har sparats i db");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
        }
    }
}



