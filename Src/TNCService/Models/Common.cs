using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNCService.Models
{
    public class Identifier
    {
        public string Id { get; set; }
    }

    public class Travel_History : Identifier
    {
        public string type { get; set; }
    }

    public class Response_Travel_History
    {
        public int number_of_rides { get; set; }
        public double start_location_lat { get; set; }
        public double start_location_lon { get; set; }
        public double end_location_lat { get; set; }
        public double end_location_lon { get; set; }

    }
}