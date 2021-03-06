﻿using System;
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
    [Authorize]
    public class ProfileController : Controller
    {
        
        // GET: Profile
        string BaseUrl = "http://193.10.202.82/MyProfile/api/Profiles/";
        public async Task<ActionResult> Index()
        {

            string id = Session["UserProfile"].ToString();
            int ProfileId = Int32.Parse(id);

            ProfilesClass Profile;


            using (var ApiClient = new HttpClient())
            {
                ApiClient.BaseAddress = new Uri(BaseUrl);
                ApiClient.DefaultRequestHeaders.Clear();
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await ApiClient.GetAsync("GetProfile/" + ProfileId);  

                if (Res.IsSuccessStatusCode)
                {
                    var settings = new JsonSerializerSettings // Detta fungerar för att ignorera Null-värden
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var Response = Res.Content.ReadAsStringAsync().Result;
                  Profile = JsonConvert.DeserializeObject<ProfilesClass>(Response, settings);


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