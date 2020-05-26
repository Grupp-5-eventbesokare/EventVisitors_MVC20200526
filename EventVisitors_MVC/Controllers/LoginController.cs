using EventVisitors_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

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
        public ActionResult LoginUser(RegistrationClass inlogg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:19779/api/Login");

                //HTTP POST
                var postTask = client.PostAsJsonAsync("Login", inlogg);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Anropa API för att påbörja Sessionen
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                return View(inlogg);
            }


        }
    }
}
