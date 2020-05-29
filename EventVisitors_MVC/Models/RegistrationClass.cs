using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventVisitors_MVC.Models
{
    public class RegistrationClass
    {
        public string Registration_Firstname { get; set; }
        public string Registration_Lastname { get; set; }      
        public string Registration_Email { get; set; }
        public string Registration_Password { get; set; }
        public string Registration_Role { get; set; }
    }
}
