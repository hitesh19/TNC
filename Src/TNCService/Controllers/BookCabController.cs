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
    public class BookCabController : ApiController
    {
        // POST api/BookCabController

        /// <summary>
        /// To Book a cab for the Passenger
        /// </summary>
        /// <param name="passangerId"></param>
        /// <param name="current_Lat"></param>
        /// <param name="current_Lon"></param>
        /// <param name="cab_Type"></param>
        /// <param name="destination_lat"></param>
        /// <param name="destination_Lon"></param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message</param> 
        /// </returns>
        public Response Post([FromBody]BookCab value)
        {

            AppDao dbobj = new AppDao();
            Response responseobj = new Response();
            Identifier resultObj = new Identifier();
            SelectedDriver selectedCabObj = new SelectedDriver();
            Person selectedDriver = new Person();
            BookCabResponse returnObj = new BookCabResponse();
            try
            {
                //To Fetch the vehicle_Type id for the cab type
                string query = "SELECT Id FROM Vehicle_Type WHERE type = '" + value.cab_Type + "'";               
                resultObj = dbobj.GetVehicleTypeId(query);

                //To Fetch the cab details if available 
                query = "SELECT person_id as id, id AS vehicle_id , registration_number,ideal_location_lat AS driver_lat,ideal_location_lon AS driver_lon,passenger_capacity  FROM Vehicle WHERE is_active = 1 AND person_id != " + value .passangerId+ " AND ride_in_progress = 0 ORDER BY (("+value.current_Lat+" - ideal_location_lat) * ("+value.current_Lat+" - ideal_location_lat) + ("+value.current_Lon+" - ideal_location_lon) * ("+value.current_Lon+" - ideal_location_lon)) LIMIT 1; ";               
                selectedCabObj = dbobj.GetCab(query);

                //Insert the vehicle details in the vehicle location table as the cab has been booked
                query = "INSERT INTO Vehicle_Location Values(" + selectedCabObj.driver_lat + "," + selectedCabObj.driver_lon + "," + value.current_Lat + "," + value.current_Lon + "," + (selectedCabObj.passenger_capacity - 1) + "," + selectedCabObj.vehicle_id + "," + resultObj.Id + ")";
                dbobj.Execute(query);

                //Update Ride in Progress status
                query = "UPDATE Vehicle SET ride_in_progress = 1 WHERE is_active = 1 AND person_id = "+ selectedCabObj.Id + " AND id = "+ selectedCabObj.vehicle_id + "";
                dbobj.Execute(query);

                //Get the driver Name and phone number for the passenger 
                query = "SELECT first_name,last_name,primary_phone_number FROM Person WHERE  Id = " + selectedCabObj.Id + "";                
                selectedDriver = dbobj.GetDriverDetails(query);

                //Get the estimated Fare 
                double estimatedFare = dbobj.EstimatedFare(value.current_Lat, value.current_Lon, value.destination_lat, value.destination_Lon, value.cab_Type);
               
                returnObj.first_name = selectedDriver.first_name;
                returnObj.last_name = selectedDriver.last_name;
                returnObj.driver_phone_number = selectedDriver.primary_phone_number;
                returnObj.estimated_fare = estimatedFare;

                responseobj.status = "Success";
                responseobj.message = JsonConvert.SerializeObject(returnObj);
            }
            catch (Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "Booking a cab Failed with error -> " + ex.Message;
            }
     
            return responseobj;
        }

        // PUT api/BookCabController
        /// <summary>
        /// To Update the realTime location of the cab for sharing cab service.
        /// </summary>
        /// <param name="vehicle_id"></param>
        /// <param name="location_lat"></param>
        /// <param name="location_Lon"></param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message</param> 
        /// </returns>
        public Response Put([FromBody]RealTimeLocationUpdate value)
        {
            AppDao dbobj = new AppDao();
            Response responseobj = new Response();
            try
            {
                string query = "UPDATE Vehicle_Location SET realTime_current_location_lat = " + value.location_lat + ",realTime_current_location_lon = " + value.location_Lon + "  WHERE Vehicle_id = " + value.vehicle_id + "";
                dbobj.Execute(query);

                responseobj.status = "Sucess";
                responseobj.message = "Cab Location Updated SuccussFully";
            }
            catch (Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "Updating the real time location of cab Failed with error -> " + ex.Message;
            }

            return responseobj;
        }
    }
}