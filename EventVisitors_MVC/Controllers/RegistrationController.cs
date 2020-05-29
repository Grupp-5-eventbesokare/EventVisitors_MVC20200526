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

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult RegistrationUser()
        {

            return View();
        }

        [HttpPost] //Skickar värderna som användaren skriver in
        public ActionResult RegistrationUser(ProfilesClass registration)
        {
            registration.Profile_Role = "Besökare"; // Besökare blir standardroll för alla som registrerar sig
            using (var client = new HttpClient())
            {
                ProfilesClass p = new ProfilesClass{ Profile_Email = registration.Profile_Email, Profile_Firstname = registration.Profile_Firstname, Profile_Lastname = registration.Profile_Lastname, Profile_Password=registration.Profile_Password, Profile_Role =registration.Profile_Role};
                client.BaseAddress = new Uri("http://193.10.202.76/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("visitor", registration).Id;//.Id;  Kolla med grupp1 att det är rätt metod

                //postTask.Wait();
                //var result = postTask.Result; // 

                if (postTask != 0)
                {
                    registration.Profile_User_Id = postTask;
                    //SaveProfile k = new SaveProfile; //Kalla på metoden
                    ProfilesClass b = new ProfilesClass { Profile_Email = registration.Profile_Email, Profile_Firstname = registration.Profile_Firstname, Profile_Lastname = registration.Profile_Lastname, Profile_PhoneNr =registration.Profile_PhoneNr, Profile_Birthday = registration.Profile_Birthday, Profile_Role =registration.Profile_Role, Profile_User_Id=registration.Profile_User_Id};
                    SaveProfile(b);
                    return RedirectToAction("LoginUser", "Login");
                }
                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                return View(registration);
            }
        }

        private void SaveProfile(ProfilesClass newProfile)
        {
            using (var client = new HttpClient())

                client.BaseAddress = new Uri("http://localhost:19779/api/");
                var postTask = client.PostAsJsonAsync("MyProfile", newProfile);

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



