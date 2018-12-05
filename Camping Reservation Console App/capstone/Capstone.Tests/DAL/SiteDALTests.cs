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
    public class SiteDALTests : All_SqlDALTests
    { 

        [TestMethod()]
        public void SearchForSiteTest()
        {
            
            // Arrange 
            SiteDAL sDAL = new SiteDAL(connectionString);

            //Act
            IList<CampSite> site = sDAL.SearchForSite(1,1, DateTime.Now.AddDays(4), DateTime.Now.AddDays(8));
            
            //Assert
            int sites = site.Count;
            Assert.AreEqual(4, sites);
        }

        [TestMethod()]
        public void SearchForSiteTotalTest()
        {
            // Arrange 
            SiteDAL sDAL = new SiteDAL(connectionString);
          
            //Act
            double p = 0;
            IList<CampSite> site = sDAL.SearchForSiteTotalAmt(1, 1, DateTime.Now.AddDays(4), DateTime.Now.AddDays(8));
            foreach (CampSite s in site)
            {
                p = s.Fee;
            }
            //Assert           
            Assert.AreEqual(140.00D, p);
        }
    }
    
}
