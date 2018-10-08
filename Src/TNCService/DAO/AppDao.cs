using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using TNCService.Models;

namespace TNCService.DAO
{
    public class AppDao
    {
        
        private SQLiteConnection sql_con;
        private DataSet DS = new DataSet();
        private List<DataTable> DT = new List<DataTable>();

        private void SetConnection()
        {
            sql_con = new SQLiteConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TNC.db"));
        }

        public void Create(object obj)
        {
            SetConnection();

            sql_con.Insert(obj);
        }

        public Identifier GetDriverId (string query)
        {

            SetConnection();
            Identifier obj = new Identifier();

            obj = sql_con.Query<Identifier>(query).FirstOrDefault();

            return obj;
        }

        public Identifier GetVehicleTypeId(string query)
        {

            SetConnection();
            Identifier obj = new Identifier();

            obj = sql_con.Query<Identifier>(query).FirstOrDefault();

            return obj;
        }

        public SelectedDriver GetCab(string query)
        {

            SetConnection();
            SelectedDriver obj = new SelectedDriver();

            obj = sql_con.Query<SelectedDriver>(query).FirstOrDefault();

            return obj;
        }

        public void Execute(string query)
        {
            SetConnection();

            sql_con.Execute(query);
        }

        public Person GetDriverDetails(string query)
        {
            SetConnection();
            Person obj = new Person();

            obj = sql_con.Query<Person>(query).FirstOrDefault();

            return obj;
        }
        
        public double EstimatedFare(double current_lat,double current_lon, double destination_lat,double destination_lon, string cab_Type)
        {

            Random random = new Random();
            return Math.Abs(random.NextDouble() * (50 - 500) + 50);
        }

        public Response_Travel_History GetTravelDetails(string query)
        {
            SetConnection();
            Response_Travel_History obj = new Response_Travel_History();

            obj = sql_con.Query<Response_Travel_History>(query).FirstOrDefault();

            return obj;
        }

    }
}