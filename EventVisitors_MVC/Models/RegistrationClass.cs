﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventVisitors_MVC.Models
{
    public class RegistrationClass
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }      
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
