using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class ReservationDAL
    {
        private string connectionString;
        private string SQL_BookReservations = @"INSERT INTO [dbo].[reservation]
                                               ([site_id]
                                               ,[name]
                                               ,[from_date]
                                               ,[to_date]
                                               ,[create_date])
                                         VALUES
                                               ((SELECT site_id FROM site s 
                                                WHERE site_number = @site_id AND campground_id = @camp_id)
                                               ,@name
                                               ,@from_date
                                               ,@to_date
		                                       ,CURRENT_TIMESTAMP
                                               );
                                        SELECT SCOPE_IDENTITY () As NewID";
        private string SQL_Reservation30Days = @"DECLARE @park_id int = @park_id1;
                                                DECLARE @requested_start datetime = GETDATE();
                                                DECLARE @requested_end datetime= DATEADD(day, 30, GETDATE());

                                                Select DISTINCT r.reservation_id, r.name, CAST(r.from_date as date) as arrive, CAST(r.to_date as date) as leave, p.name as Park
                                                FROM campground c, park p, reservation r, site s
                                                WHERE --s.campground_id = @campground_id
                                                p.park_id = @park_id
                                                AND p.park_id = c.park_id
                                                AND r.site_id = s.site_id
                                                AND c.campground_id = s.campground_id
                                                AND r.reservation_id IN
                                                (SELECT reservation_id from reservation
                                                WHERE @requested_start <= to_date
                                                AND @requested_end >= from_date)
                                                ORDER BY leave;";

        public ReservationDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //books the reservation and returns the reservation ID
        public int BookReservation(Reservation bookReservation)
        {
            int newResId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_BookReservations;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@camp_id", bookReservation.CampId);
                    cmd.Parameters.AddWithValue("@site_id", bookReservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", bookReservation.Name);
                    cmd.Parameters.AddWithValue("@from_date", bookReservation.FromDate);
                    cmd.Parameters.AddWithValue("@to_date", bookReservation.ToDate);

                    SqlDataReader resReader = cmd.ExecuteReader();
                    if (resReader.HasRows)
                    {
                        resReader.Read();
                        newResId = Convert.ToInt32(resReader["NewID"]);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return newResId;
        }

        //queries all reservations in upcoming rolling 30 days from current day -- BONUS
        public IList<Reservation> AllReservations30Days(int parkId)
        {
            List<Reservation> upcomingReservations = new List<Reservation>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_Reservation30Days;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@park_id1", parkId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation res = new Reservation();

                        res.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                        res.Name = Convert.ToString(reader["name"]);
                        res.FromDate = Convert.ToDateTime(reader["arrive"]);
                        res.ToDate = Convert.ToDateTime(reader["leave"]);
                        res.ParkReservation = Convert.ToString(reader["Park"]);
                       
                        upcomingReservations.Add(res);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return upcomingReservations;
        }
    }
}
