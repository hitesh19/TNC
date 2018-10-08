using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TNCService.DAO;
using TNCService.Models;

namespace TNCService.Controllers
{
    public class PassengerController : ApiController
    {
        // POST api/PassengerController
        
        /// <summary>
        /// To Register a new Passenger
        /// </summary>
        /// <param name="first_name">First Name of the Passenger</param>
        /// <param name="last_name">Last Name of the Passenger</param>
        /// <param name="primary_phone_number">Phone Number of the Passenger</param>
        /// <returns>
        ///     <param name="status">Status of the API request</param> 
        ///     <param name="message">Response message</param> 
        /// </returns>
        public Response Post([FromBody]Person value)
        {
            AppDao dbobj = new AppDao();
            Person newUser = new Person();
            Response responseobj = new Response();
            DriverController userInfo = new DriverController();
            try
            {

                responseobj = userInfo.Get(value.first_name, value.last_name);

                if (responseobj.status == "Sucess" && !responseobj.message.Contains("No user with"))
                {
                    responseobj.status = "Failed";
                    responseobj.message = "Passanger already Exists";
                }
                else
                {
                    newUser.first_name = value.first_name;
                    newUser.last_name = value.last_name;
                    newUser.primary_phone_number = value.primary_phone_number;

                    dbobj.Create(newUser);

                    responseobj.status = "Sucess";
                    responseobj.message = "User added Successfully";
                }
                
            }
            catch(Exception ex)
            {
                responseobj.status = "Failed";
                responseobj.message = "User adding Failed with error -> "+ex.Message;
            }
           
            return responseobj;          
        }
    }
}