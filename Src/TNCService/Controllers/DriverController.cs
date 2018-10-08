using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TNCService.DAO;
using TNCService.Models;

namespace TNCService.Controllers
{
    public class DriverController : ApiController
    {

        // GET api/DriverController

        /// <summary>
        /// To pass the Id of the person if the firstName and LastName exists
        /// </summary>
        /// <param name="firstName">First Name of the Passenger</param>
        /// <param name="lastName">Last Name of the Passenger</param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message if the Id exists or not</param> 
        /// </returns>
        public Response Get(string firstName, string lastName)
        {

            AppDao dbobj = new AppDao();
            Response responseobj = new Response();
            DataSet dsResult = new DataSet();

            try
            {
                string query = "SELECT Id FROM Person WHERE first_name = '"+ firstName + "' AND last_name = '"+ lastName + "'";

                Identifier resultObj = new Identifier();
                resultObj = dbobj.GetDriverId(query);

                if(resultObj != null)
                {
                    responseobj.status = "Sucess";
                    responseobj.message = resultObj.Id;
                }
                else
                {
                    responseobj.status = "Failed";
                    responseobj.message = "No user with the FirstName ='"+ firstName + "' and LastName = '"+ lastName + "' exists";
                }               
            }
            catch (Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "Driver adding Failed with error -> " + ex.Message;
            }
            return responseobj; 
        }

        // POST api/DriverController

        /// <summary>
        /// To register the driver 
        /// </summary>
        /// <param name="userInfo">
        ///     <param name="first_name"></param>
        ///     <param name="last_name"></param>
        ///     <param name="primary_phone_number"></param>
        /// </param>
        /// <param name="registration_number"></param>
        /// <param name="vin"></param>
        /// <param name="passenger_capacity"></param>
        /// <param name="ride_in_progress"></param>
        /// <param name="ideal_location_lat"></param>
        /// <param name="ideal_location_lon"></param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message</param> 
        /// </returns>
        public Response Post([FromBody]Driver obj)
        {
            AppDao dbobj = new AppDao();
            Person newUser = new Person();
            Vehicle newVehicle = new Vehicle();
            Response responseobj = new Response();
            try
            {
                int driverId;

                responseobj = Get(obj.userInfo.first_name, obj.userInfo.last_name);

                if(responseobj.status == "Sucess" && !responseobj.message.Contains("No user with"))
                {
                    responseobj.status = "Failed";
                    responseobj.message = "Driver already Exists";
                }
                else
                {
                    newUser.first_name = obj.userInfo.first_name;
                    newUser.last_name = obj.userInfo.last_name;
                    newUser.primary_phone_number = obj.userInfo.primary_phone_number;

                    dbobj.Create(newUser);

                    responseobj = Get(newUser.first_name, newUser.last_name);
                    driverId = Convert.ToInt32(responseobj.message);

                    newVehicle.ideal_location_lat = obj.ideal_location_lat;
                    newVehicle.ideal_location_lon = obj.ideal_location_lon;
                    newVehicle.passenger_capacity = obj.passenger_capacity;
                    newVehicle.vin = obj.vin;
                    newVehicle.registration_number = obj.registration_number;
                    newVehicle.ride_in_progress = false;
                    newVehicle.person_id = driverId;

                    dbobj.Create(newVehicle);

                    responseobj.status = "Sucess";
                    responseobj.message = "Driver added Successfully";
                }              
            }
            catch (Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "Driver adding Failed with error -> " + ex.Message;
            }

            return responseobj;
        }
    }
}