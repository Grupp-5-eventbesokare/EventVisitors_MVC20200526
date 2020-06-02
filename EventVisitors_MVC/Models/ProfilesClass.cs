using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventVisitors_MVC.Models
{
    public class ProfilesClass
    {
        public int Profile_Id { get; set; }

        public string Profile_Firstname { get; set; }

        public string Profile_Lastname { get; set; }

        public DateTime Profile_Birthday { get; set; }

        public string Profile_PhoneNr { get; set; }

        public string Profile_Email { get; set; }

        public string Profile_Password { get; set; }

        public string Profile_Role { get; set; }

        public int Profile_User_Id { get; set; }
    }
}