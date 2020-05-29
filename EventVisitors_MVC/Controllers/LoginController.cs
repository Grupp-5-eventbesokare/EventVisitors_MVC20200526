using EventVisitors_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EventVisitors_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginUser()
        {

            return View();
        }

        //Skickar värderna som användaren skriver in
        [HttpPost] 
        public ActionResult LoginUser(ProfilesClass inlogg)
        {
            using (var client = new HttpClient())
            {
                ProfilesClass login = new ProfilesClass { Profile_Email = inlogg.Profile_Email, Profile_Password = inlogg.Profile_Password };
                client.BaseAddress = new Uri("http://193.10.202.76/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("visitorlogin", login).Id;
                //postTask.Wait();
                //var result = postTask.Result;
                inlogg.Profile_User_Id = postTask;
                ProfilesClass b = new ProfilesClass{ Profile_User_Id=inlogg.Profile_User_Id};
                checkUser(b);

                if (postTask == inlogg.Profile_User_Id)
                {
                    Session["Namn"] = inlogg.Profile_Firstname;
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                return View(inlogg);
            }

        }

        public async Task<ActionResult> checkUser(ProfilesClass b)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19779/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/MyProfile/" + b);
                return View(Res);
            }
        }
    }
}
