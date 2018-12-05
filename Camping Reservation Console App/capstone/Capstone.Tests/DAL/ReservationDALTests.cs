using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Capstone.DAL;

namespace Capstone.Tests.DAL
{
    [TestClass()]
    public class ReservationDALTests : All_SqlDALTests
    {
        [TestMethod()]
        public void MakeReservationTest()
        {

            // Arrange 
            ReservationDAL rDAL = new ReservationDAL(connectionString);

            //Act
            Reservation reservation = new Reservation
            {
                CampId = 1,
                SiteId = 4,
                Name = "The Donner Party",
                FromDate = DateTime.Now.AddDays(2),
                ToDate = DateTime.Now.AddDays(4)
            };

            int didCreate = rDAL.BookReservation(reservation);

            //Assert
            Assert.AreEqual(9, didCreate);
        }

        [TestMethod()]
        public void AllReservations30DaysTest()
        {
            // Arrange 
            ReservationDAL sDAL = new ReservationDAL(connectionString);

            //Act
            IList<Reservation> site = sDAL.AllReservations30Days(1);

            //Assert
            int sites = site.Count;
            Assert.AreEqual(7, sites);
        }
        
    }
}
