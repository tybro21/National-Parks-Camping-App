using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkDAL
    {
        private string connectionString;
        private string SQL_GetAllParks = "SELECT ROW_NUMBER() OVER(ORDER BY name ASC) AS Row#, park_id, name FROM park";
        private string SQL_DisplayParkInfo = "SELECT* FROM park WHERE park_id = @park_id;";

        public ParkDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> GetAllParks() //main menu listing of parks
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_GetAllParks;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();

                        park.Rownumber = Convert.ToInt32(reader["Row#"]);
                        park.ParkId = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);

                        parks.Add(park);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return parks;
        }

        public IList<Park> DisplayParkInfo(int parkID) //refers to sub menu
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_DisplayParkInfo;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@park_id", parkID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                       
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToDouble(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);

                        parks.Add(park);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return parks;
        }
    


    }
}
