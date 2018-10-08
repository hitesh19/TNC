using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNCService.Models
{
    public class Person
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string primary_phone_number { get; set; }
    }

    public class Driver:Vehicle
    {
        public Person userInfo { get; set; }
    }    
}