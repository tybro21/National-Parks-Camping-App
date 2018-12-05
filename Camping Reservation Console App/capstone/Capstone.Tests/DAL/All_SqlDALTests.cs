using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
namespace Capstone.Tests.DAL
{
    [TestClass()]
    public class All_SqlDALTests
    {
        private TransactionScope tran;

        public string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand(@"
DROP TABLE reservation;
DROP TABLE site;
DROP TABLE campground;
DROP TABLE park;

CREATE TABLE park (
  park_id integer identity NOT NULL,
  name varchar(80) NOT NULL,          
  location varchar(50) NOT NULL,     
  establish_date date NOT NULL,       
  area integer NOT NULL,              
  visitors integer NOT NULL,          
  description varchar(500) NOT NULL, 
  CONSTRAINT pk_park_park_id PRIMARY KEY (park_id)
);

CREATE TABLE campground (
  campground_id integer identity NOT NULL,
  park_id integer NOT NULL,            
  name varchar(80) NOT NULL,           
  open_from_mm integer NOT NULL,       
  open_to_mm integer NOT NULL,         
  daily_fee money NOT NULL,
  CONSTRAINT pk_campground_campground_id PRIMARY KEY (campground_id)
);

CREATE TABLE site (
  site_id integer identity NOT NULL,
  campground_id integer NOT NULL,
  site_number integer NOT NULL,                   
  max_occupancy integer NOT NULL DEFAULT 6,       
  accessible bit NOT NULL DEFAULT 0, 
  max_rv_length integer NOT NULL DEFAULT 0, 
  utilities bit NOT NULL DEFAULT 0,               
  CONSTRAINT pk_site_site_number_campground_id PRIMARY KEY(site_id)
);

 CREATE TABLE reservation(
  reservation_id integer identity NOT NULL,
  site_id integer NOT NULL,
  name varChar(80) NOT NULL,
  from_date date NOT NULL,
  to_date date NOT NULL,
  create_date DATETIME DEFAULT GETDATE(),
  CONSTRAINT pk_reservation_reservation_id PRIMARY KEY(reservation_id)
);
--Crystal Lake
INSERT INTO park(name, location, establish_date, area, visitors, description)
VALUES('Crystal Lake', 'The Hills', '1984-10-30', 13389, 129, ' A beautiful Park for hiking, camping, swimming, and getting murdered in very gruesome and elaborate ways by a machete weilding nutjob!');

--Crystal Lake Campgrounds
INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(1, 'Sleepaway Camp', 4, 11, 35.00);
INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(1, 'Camp Firewood', 5, 9, 30.00);


--Crystal Lake Sleepaway Camp Sites(Tent, Camper)
INSERT INTO site(site_number, campground_id) VALUES(1, 1);
INSERT INTO site(site_number, campground_id, accessible) VALUES(2, 1, 1);
INSERT INTO site(site_number, campground_id, accessible, utilities) VALUES(3, 1, 1, 1);
                
--Crystal Lake Sleepaway Camp Sites(RV/ Trailer 20ft)
INSERT INTO site(site_number, campground_id, max_rv_length) VALUES(4, 1, 20);
INSERT INTO site(site_number, campground_id, max_rv_length, utilities) VALUES(5, 1, 20, 1);
INSERT INTO site(site_number, campground_id, max_rv_length, accessible, utilities) VALUES(6, 1, 20, 1, 1);

--Crystal Lake Camp Firewood Sites(RV/ Trailer 35ft)
INSERT INTO site(site_number, campground_id, max_rv_length) VALUES(1, 2, 35);
INSERT INTO site(site_number, campground_id, max_rv_length, utilities) VALUES(2, 2, 35, 1);
INSERT INTO site(site_number, campground_id, max_rv_length, accessible, utilities) VALUES(3, 2, 35, 1, 1);

--Crystal Lake Camp Firewood Sites(Tent)
INSERT INTO site(site_number, campground_id) VALUES(4, 2);
INSERT INTO site(site_number, campground_id, accessible) VALUES(5, 2, 1);
INSERT INTO site(site_number, campground_id, accessible, utilities) VALUES(6, 2, 1, 1);


--Reservations
-- Crystal Lake Sites(1 - 12)
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(1, 'Smith Family Reservation', GETDATE() - 2, GETDATE() + 2);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(1, 'Lockhart Family Reservation', GETDATE() - 6, GETDATE() - 3);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(2, 'Jones Reservation', GETDATE() - 2, GETDATE() + 2);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(3, 'Bauer Family Reservation', GETDATE(), GETDATE() + 2);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(4, 'Eagles Family Reservation', GETDATE() + 5, GETDATE() + 10);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(7, 'Aersomith Reservation', GETDATE() - 3, GETDATE() + 2);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(9, 'Claus Family Reservation', GETDATE(), GETDATE() + 1);
INSERT INTO reservation(site_id, name, from_date, to_date) VALUES(10, 'Astley Family Reservation', GETDATE() + 14, GETDATE() + 21)", conn);

                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }
    }
}
