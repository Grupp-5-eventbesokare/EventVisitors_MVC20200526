using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace EventVisitors_MVC.Models
{
    public class EventsClass
    {
        public int Event_id { get; set; }

        public string Event_Name { get; set; }

        public bool Event_Active { get; set; }

        public int Event_Arranger_Id { get; set; }

        public Event_Organizer Event_Organizer { get; set; }

        public bool Event_Seeking_Volunteers { get; set; }

        public int Event_Location_Id { get; set; }

        public Event_Facility Event_Facility { get; set; } 

        public string Event_Description { get; set; }

        public string Event_Imagelink { get; set; }

        public int Event_Ticket_Price { get; set; }

        public string Event_Start_Datetime { get; set; }

        public string Event_End_Datetime { get; set; }

        public string Event_Create_Datetime { get; set; }

        public string User_Type { get; set; }

        public Event_Category Event_Category { get; set; }
    }

    public class Event_Category
    {
        public int Category_Id { get; set; }

        public string Category_Name { get; set; }

    }

    public class Event_Organizer
    {
        public string Email { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Phone { get; set; }
    }

    public class Event_Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Place Event_Place { get; set; }

    }
    public class Place
    {
        public string City { get; set; }

        public int Id { get; set; }

        public string Place_Name { get; set; }
    }
}