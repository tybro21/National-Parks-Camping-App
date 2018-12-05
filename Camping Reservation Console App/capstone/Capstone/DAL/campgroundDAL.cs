using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;
        private string SQL_DisplayCampGrounds = "SELECT * FROM campground WHERE park_id = @park_id;";

        public CampgroundDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<CampGround> DisplayCampGrounds(int parkID) //gets all the campgrounds
        {
            List<CampGround> campground = new List<CampGround>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_DisplayCampGrounds;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@park_id", parkID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CampGround camp = new CampGround();

                        camp.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        camp.Name = Convert.ToString(reader["name"]);
                        camp.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        camp.CloseFromMonth = Convert.ToInt32(reader["open_to_mm"]);
                        camp.DailyFee = Convert.ToDouble(reader["daily_fee"]);

                        campground.Add(camp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return campground;
        }
    }
}
