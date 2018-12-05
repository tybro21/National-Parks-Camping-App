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
    public class CampgroundDALTests : All_SqlDALTests
    {
        [TestMethod()]
        public void ShowCampgroundsAtParkTest()
        {
            // Arrange 
            CampgroundDAL cDAL = new CampgroundDAL(connectionString);

            //Act
            IList<CampGround> campGround = cDAL.DisplayCampGrounds(1);
            
            //Assert
            int grounds = campGround.Count;
            Assert.AreEqual(2, grounds);
        }

        [TestMethod()]
        public void NoCampgroundsAtParkTest()
        {
            // Arrange 
            CampgroundDAL cDAL = new CampgroundDAL(connectionString);

            //Act
            IList<CampGround> cGround = cDAL.DisplayCampGrounds(2);

            //Assert
            int grounds = cGround.Count;
            Assert.AreEqual(0, grounds);
        }
    }
}
