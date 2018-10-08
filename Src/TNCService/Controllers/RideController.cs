using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TNCService.DAO;
using TNCService.Models;

namespace TNCService.Controllers
{
    public class RideController : ApiController
    {

        // GET api/RideController

        /// <summary>
        /// To fetch the travel history for the passenger and driver
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="type">Cab Type</param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message</param> 
        /// </returns>
        public Response Get(int Id, string type)
        {
            AppDao dbobj = new AppDao();
            Response responseobj = new Response();
            Response_Travel_History resultobj = new Response_Travel_History();

            try
            {
                string query;
                //Fetch the travel history for the specfied person_id
                if (string.Compare(type, "Passenger") == 0)
                {
                    //Passenger is given the vehicle id
                    query = "SELECT COUNT(Vehicle_id) as number_of_rides, start_location_lat,start_location_lon,end_location_lat,end_location_lon FROM Travel_History WHERE Vehicle_id = " + Id+"";
                    resultobj = dbobj.GetTravelDetails(query);
                }
                else if(string.Compare(type, "Driver") == 0)
                {
                    //Driver has given the passenger id
                    query = "SELECT COUNT(Passenger_id) as number_of_rides, start_location_lat,start_location_lon,end_location_lat,end_location_lon FROM Travel_History WHERE Passenger_id = " +Id + "";
                    resultobj = dbobj.GetTravelDetails(query);
                }

                responseobj.status = "Sucess";
                responseobj.message = JsonConvert.SerializeObject(resultobj);
            }
            catch (Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "Travel history Failed with error -> " + ex.Message;
            }
            return responseobj;           
        }

        // POST api/RideController
        /// <summary>
        /// To update the travel history table and other tables as the ride has been completed
        /// </summary>
        /// <param name="passanger_id"></param>
        /// <param name="vehicle_id"></param>
        /// <param name="start_location_lat"></param>
        /// <param name="start_location_Lon"></param>
        /// <param name="end_location_lat"></param>
        /// <param name="end_location_Lon"></param>
        /// <param name="final_fare"></param>
        /// <param name="estimated_fare"></param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message</param> 
        /// </returns>
        public Response Post([FromBody]RideCompleted value)
        {
            AppDao dbobj = new AppDao();
            Response responseobj = new Response();
            try
            {

                //Update the data as the cab ride is completed
                string query = "INSERT INTO Travel_History VALUES(" + value.passanger_id + "," + value.vehicle_id + "," + value.start_location_lat + "," + value.start_location_Lon + "," + value.end_location_lat + "," + value.end_location_Lon + "," + value.estimated_fare + "," + value.final_fare + ")";
                dbobj.Execute(query);

                //delete record from vehicle location as the cab is nt avaiable for sharing
                query = "DELETE FROM Vehicle_Location WHERE vehicle_id = " + value.vehicle_id + "";
                dbobj.Execute(query);

                //update the Vehicle table to make the cab status as avaiable
                query = "UPDATE Vehicle SET ride_in_progress = 0,ideal_location_lat ="+value.end_location_lat+", ideal_location_lon="+value.end_location_Lon+" WHERE is_active = 1 AND person_id = " + value.passanger_id + " AND id = " + value.vehicle_id + "";
                dbobj.Execute(query);

                responseobj.status = "Sucess";
                responseobj.message = "Ride Completed SuccussFully";
            }
            catch (Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "Completing a cab Failed with error -> " + ex.Message;
            }

            return responseobj;
        }
    }
}