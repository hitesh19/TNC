using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNCService.Models
{
    public class Response
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class BookCabResponse
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string driver_phone_number { get; set; }
        public double estimated_fare { get; set; }
    } 
}