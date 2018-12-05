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
    public class ParkDALTests : All_SqlDALTests
    {
        [TestMethod()]
        public void GetAllParksTest()
        {
            // Arrange 
            ParkDAL pDAL = new ParkDAL(connectionString);

            //Act
            IList<Park> park = pDAL.GetAllParks();

            //Assert
            int parkTotal = park.Count;
            Assert.AreEqual(1, parkTotal);
        }

        [TestMethod()]
        public void DisplayParkInfoTest()
        {
            // Arrange 
            ParkDAL pDAL = new ParkDAL(connectionString);

            //Act
            IList<Park> park = pDAL.DisplayParkInfo(1);

            //Assert

            Assert.IsNotNull(park);
        }
    }
}
