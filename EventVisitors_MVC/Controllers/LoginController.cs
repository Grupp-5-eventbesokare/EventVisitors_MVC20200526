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
using System.Web.UI.WebControls;

namespace EventVisitors_MVC.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult LoginUser()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(LoginClass login)
        {
            if (login.Email == null || login.Password == null)
            {
                ModelState.AddModelError("", "Du måste fylla i både användarnamn och lösenord");
                return View();
            }

            bool validUser = false;

            validUser = checkUser(login);

            if (validUser == true)
            {    
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(login.Email, false);
               
            }
            ModelState.AddModelError("", "Inloggningen ej godkänd");
            return View();

        }

        private bool checkUser(LoginClass inlogg)
        {
            using (var client = new HttpClient())
            {
                LoginClass login = new LoginClass { Email = inlogg.Email, Password = inlogg.Password };

                client.BaseAddress = new Uri("http://193.10.202.76/api/");
                var response = client.PostAsJsonAsync("visitorlogin", login).Result;
                if (response.IsSuccessStatusCode)
                {
                    string svarFrånLogin = response.Content.ReadAsStringAsync().Result;

                    LoginClass objektFrånWS = JsonConvert.DeserializeObject<LoginClass>(svarFrånLogin);

                    if (objektFrånWS != null)
                    {
                        bool ValidProfile = false;
                        ValidProfile = CheckUserProfile(objektFrånWS.id); //objektFrånWS.id är från login gruppen, vilket är lagrat som User_Id i vår databas
                        if (ValidProfile == true)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false; //Om  svaret inte är IsSuccessStatusCode så kan vi inte godkänna inloggningen
            }
        }
       

        private bool CheckUserProfile(int objektfrånWS) //Metod för att kontrollera mot vår egna databas, och få fram ett id på användaren
        {
            using (var client = new HttpClient())
            {

                LoginClass anvandareAttKolla = new LoginClass { User_Id = objektfrånWS }; //Skickar in logingruppens id (Vårat user_Id)

                client.BaseAddress = new Uri("http://193.10.202.82/MyProfile/api/CheckUser/"); //Ta bort api om route fungerar

                var response = client.PostAsJsonAsync("MyProfile", anvandareAttKolla).Result;
                if (response.IsSuccessStatusCode)
                {
                    var svarFrånRest = response.Content.ReadAsStringAsync().Result;
                    var jsonResult = JsonConvert.DeserializeObject(svarFrånRest).ToString(); // Gör deserialize två gånger, då det annars uppstår ett felmeddelande om att det inte går att konvertera datatyperna
                    LoginClass objektProfile = JsonConvert.DeserializeObject<LoginClass>(jsonResult);


                    if (objektProfile !=null) //Kontrollerar så objektet tillbaka inte är null
                    {
                        if (objektProfile.User_Id == objektfrånWS) //Om logingruppens id matchar vårat User_Id från databasen
                        {
                            
                            Session["UserProfile"] = objektProfile.id;
                            Session["User_Id_Profile"] = objektProfile.User_Id;
                            return true;

                        }
                        return false;
                    }
                    return false;
                }

                return false;
            }

        }
    }
}


    



    


