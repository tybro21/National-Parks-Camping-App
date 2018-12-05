using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Capstone.DAL
{
    public class SiteDAL
    {
        private string connectionString;
        private string SQL_ReservationSearch = @"DECLARE @campground_id int = @campground_id1;
                                                DECLARE @park_id int = @park_id1;
                                                DECLARE @requested_start datetime = @from_date1;
                                                DECLARE @requested_end datetime= @to_date1;

                                               Select DISTINCT TOP 5 s.site_number, s.campground_id, s.max_occupancy, s.accessible, s.max_rv_length, c.daily_fee, CAST((c.daily_fee * DATEDIFF(day, @requested_start, @requested_end)) AS decimal) AS TotalFee
                                                FROM site s, campground c, park p, reservation r
                                                WHERE s.campground_id = @campground_id
                                                AND p.park_id = @park_id
                                                AND p.park_id = c.park_id
                                                AND c.campground_id = s.campground_id
                                                AND s.site_id NOT IN
                                                (SELECT site_id from reservation
                                                WHERE from_date BETWEEN  @requested_start and @requested_end
                                                AND to_date BETWEEN  @requested_start and @requested_end)
                                                AND c.campground_id IN
                                                (SELECT campground_id from campground
                                                WHERE DATEPART(mm,@requested_start) >= open_from_mm
                                                AND DATEPART(mm, @requested_end) <= open_to_mm)
                                                AND s.site_id = r.site_id
                                                AND @requested_start> GETDATE();";
        //added in parameters to filter out dates in offseason
        public SiteDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //checks for date range
        //lists all the sites after they have been
        //left as variables to pass through instead of an object because parameters are from different models&
        //include class level variables from CampingCLI
        public IList<CampSite> SearchForSite(int parkID, int campGroundID, DateTime from_date, DateTime to_date)
        {
            List<CampSite> openSites = new List<CampSite>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_ReservationSearch;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@park_id1", parkID);
                    cmd.Parameters.AddWithValue("@campground_id1", campGroundID);
                    cmd.Parameters.AddWithValue("@from_date1", from_date);
                    cmd.Parameters.AddWithValue("@to_date1", to_date);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CampSite site = new CampSite();

                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Fee = Convert.ToDouble(reader["daily_fee"]);
                        

                        openSites.Add(site);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return openSites;
        }
        //lists the campsites available, with the total fee for the stay
        public IList<CampSite> SearchForSiteTotalAmt(int parkID, int campGroundID, DateTime from_date, DateTime to_date)
        {
            List<CampSite> openSites = new List<CampSite>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_ReservationSearch;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@park_id1", parkID);
                    cmd.Parameters.AddWithValue("@campground_id1", campGroundID);
                    cmd.Parameters.AddWithValue("@from_date1", from_date);
                    cmd.Parameters.AddWithValue("@to_date1", to_date);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CampSite site = new CampSite();

                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Fee = Convert.ToDouble(reader["TotalFee"]);

                        openSites.Add(site);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return openSites;
        }
    }
}
