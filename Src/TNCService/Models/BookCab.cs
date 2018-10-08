using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNCService.Models
{
    public class BookCab
    {
        public int passangerId { get; set; }
        public double current_Lat { get; set; }
        public double current_Lon { get; set; }
        public string cab_Type { get; set; }
        public double destination_lat { get; set; }
        public double destination_Lon { get; set; }

    }

    public class RideCompleted
    {
        public int passanger_id { get; set; }
        public int vehicle_id { get; set; }
        public double start_location_lat { get; set; }
        public double start_location_Lon { get; set; }
        public double end_location_lat { get; set; }
        public double end_location_Lon { get; set; }
        public double final_fare { get; set; }
        public double estimated_fare { get; set; }
    }

    public class RealTimeLocationUpdate
    {
        public int vehicle_id { get; set; }
        public double location_lat { get; set; }
        public double location_Lon { get; set; }
    }

}