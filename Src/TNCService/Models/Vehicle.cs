using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNCService.Models
{
    public class Vehicle
    {
        public string registration_number { get; set; }
        public string vin { get; set; }
        public int passenger_capacity { get; set; }
        public bool ride_in_progress { get; set; }
        public int person_id { get; set; }
        public double ideal_location_lat { get; set; }
        public double ideal_location_lon { get; set; }
    }
    
    public class SelectedDriver: Identifier
    {
        public int vehicle_id { get; set; }
        public string registration_number { get; set; }
        public double driver_lat { get; set; }
        public double driver_lon { get; set; }
        public int passenger_capacity { get; set; }
    }
}