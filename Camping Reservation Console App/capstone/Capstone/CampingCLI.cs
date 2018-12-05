using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;
using System.Globalization;

namespace Capstone
{
    public class CampingCLI
    {
        const string DatabaseConnection = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";
        int tempParkId;
        int tempCampId;
        DateTime arriveDate;
        DateTime departDate;

        public void RunCLI()
        {
            DisplayHeader();
            DisplayParks();
        }

        private void DisplayParks()
        {  //this diplays all of the parks in the DB

            ParkDAL dal = new ParkDAL(DatabaseConnection);
            IList<Park> parks = dal.GetAllParks();
            Console.WriteLine("PARK LISTING"+Environment.NewLine);
            Console.WriteLine(("").PadRight(5) + ("Park ID").PadRight(10) + ("Name").PadRight(5));
            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine((park.Rownumber.ToString()).PadRight(5)+park.ParkId.ToString().PadRight(10) + (park.Name).PadRight(30));
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.WriteLine();
            Console.WriteLine("Q".PadRight(5) + "Quit");
            Console.WriteLine();
            ChooseDisplayParkInfo();
        }

        private void ChooseDisplayParkInfo()
        {  //this shows park information based on choice
            tempParkId = CLIHelper.GetInteger("Please enter in the Park ID for more information:");
            ParkDAL dal = new ParkDAL(DatabaseConnection);

            IList<Park> parks = dal.DisplayParkInfo(tempParkId);
            Console.WriteLine("Park Information:");
            Console.WriteLine();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {

                    Console.WriteLine(park.Name.ToString()+ Environment.NewLine + "Location: " + park.Location + Environment.NewLine + "Established: " + park.EstablishDate.ToShortDateString() + Environment.NewLine + "Area: " + (park.Area.ToString("N0")) + " sq km" + Environment.NewLine + "Annual Vistors: " + park.Visitors.ToString("N0"));
                    Console.WriteLine();
                    Console.WriteLine(park.Description);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.WriteLine();
            DisplayParkOptions();
        }

        private void DisplayParkOptions()
        { //menu to show options within the park selected above
            Console.WriteLine("Select a Command:");
            Console.WriteLine("1) View Campgrounds" + Environment.NewLine + "2) Search for Reservation" + Environment.NewLine + "3) Return to Previous Screen"+ Environment.NewLine + "4) View Upcoming Reservations");

            CampgroundDAL camp = new CampgroundDAL(DatabaseConnection);
            
            while (true)
            {
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GetCampGrounds();
                        break;

                    case "2":
                        SearchForSite();
                        break;

                    case "3":
                        DisplayParks();
                        break;
                    case "4":
                        AllReservations30Days();
                        break;

                    default:
                        Console.WriteLine("Please enter a valid command.");
                        break;
                }
            }
        }

        private void GetCampGrounds()
        {
            CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);
            IList<CampGround> camps = dal.DisplayCampGrounds(tempParkId);
            Console.WriteLine();
            Console.WriteLine("Campgrounds in this park:");
            Console.WriteLine();
            Console.WriteLine(("Id").PadRight(15) + ("Name").PadRight(30) + ("Open").PadRight(15) + ("Close").PadRight(15) + ("Daily Fee").PadRight(5));
            if (camps.Count > 0)
            {
                foreach (CampGround camp in camps)
                {                   
                    Console.WriteLine("#" +(camp.CampgroundId.ToString()).PadRight(15)+ (camp.Name.ToString()).PadRight(30)+ (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camp.OpenFromMonth)).PadRight(15) + (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camp.CloseFromMonth)).PadRight(15) + "$"+(camp.DailyFee.ToString()).PadRight(15));                  
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        private void SearchForSite()
        {
            Console.Clear(); //clears the screen and displays the campground options
            Console.WriteLine();
            Console.WriteLine("**Please note, if your reservation date is during the park off-season, campgrounds will be unavailable to book**");
            GetCampGrounds();
            Console.WriteLine();
             
            //searches to book reservation
            tempCampId = CLIHelper.GetInteger("Which campground would you like to reserve? (enter C to cancel and go back to park listing)");          
            
            arriveDate = CLIHelper.GetDateTime("What is the arrival date? __/__/____");
            departDate = CLIHelper.GetDateTime("What is the departure date? __/__/____");
            
            SiteDAL dal = new SiteDAL(DatabaseConnection);
          
            IList<CampSite> campSites = dal.SearchForSiteTotalAmt(tempParkId, tempCampId, arriveDate, departDate);

            Console.WriteLine();
            Console.WriteLine(("Site No.").PadRight(15) + ("Max Occup.").PadRight(30) + ("Accessible?").PadRight(15) + ("Max RV Length").PadRight(15) + ("Utility").PadRight(15) + ("Fee").PadRight(15));
            if (campSites.Count > 0)
            {
                foreach (CampSite site in campSites)
                {
                    Console.WriteLine((site.SiteNumber.ToString()).PadRight(15) + (site.MaxOccupancy.ToString()).PadRight(30) + (site.Accessible.ToString()).PadRight(15) + (site.MaxRVLength.ToString()).PadRight(15) + (site.Utilities.ToString()).PadRight(15)+ "$"+(site.Fee.ToString()).PadRight(15));
                }
                BookReservation();
            }
            else
            {
                Console.WriteLine("**** NO SPOTS OPEN TO RESERVE ****");
                Console.WriteLine("Press Y to go back to park options" + Environment.NewLine + "Press N to Exit");
                string choice = Console.ReadLine();
                switch (choice.ToLower())
                {
                    case "y":
                        DisplayParkOptions();
                        break;
                    case "n":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please Choose a valid option.");
                        break;
                }
            }
        }

        private void BookReservation()
        {
            int tempSiteId = CLIHelper.GetInteger("Which site should be reserved (enter C to cancel and go back to park listing)? __");
            
            string tempResName = CLIHelper.GetString("What name should the reservation be made under? __");
            ReservationDAL dal = new ReservationDAL(DatabaseConnection);
            Reservation bookReservation = new Reservation
            {
                CampId = tempCampId,
                SiteId = tempSiteId,
                Name = tempResName,
                FromDate = arriveDate,
                ToDate = departDate
            };
            Console.WriteLine();
            int ReservationId = dal.BookReservation(bookReservation);
            if (ReservationId > 0)
            {          
                    Console.WriteLine("Your reservation confirmation number is: " + ReservationId);
                    Console.WriteLine("Thank you for booking! Please press enter to return to the Parks Menu.");
                    Console.ReadLine();
                    Console.Clear();
                    DisplayParks();

            }
            else
            {
                Console.WriteLine("**** Reservation Could Not Be Booked ****");
            }           
        }

        private void AllReservations30Days()
        {
            Console.WriteLine("All upcoming reservations (30 Days)"+ Environment.NewLine);
            ReservationDAL dal = new ReservationDAL(DatabaseConnection);
            CLIHelper cli = new CLIHelper();
            IList<Reservation> reservations = dal.AllReservations30Days(tempParkId);

            Console.WriteLine();
            Console.WriteLine(("Reservation Id").PadRight(15) + ("Name").PadRight(30) + ("Arrive").PadRight(15) + ("Leave").PadRight(15) + ("Park Name").PadRight(5));
            if (reservations.Count > 0)
            {
                foreach (Reservation res in reservations)
                {
                    Console.WriteLine("#" + (res.ReservationId.ToString()).PadRight(15) + (res.Name.ToString()).PadRight(30) + (res.FromDate.ToShortDateString()).PadRight(15) + (res.ToDate.ToShortDateString()).PadRight(15) + (res.ParkReservation.ToString()).PadRight(15));
                }
            }
            else
            {
                Console.WriteLine("**** No Reservations Found ****");
            }
            DisplayParkOptions();
        }


        private void DisplayHeader()
        {
            Console.WriteLine(@" _   _       _   _                   _   _____           _       _____                      _             ");
            Console.WriteLine(@"| \ | |     | | (_)                 | | |  __ \         | |     / ____|                    (_)            ");
            Console.WriteLine(@"|  \| | __ _| |_ _  ___  _ __   __ _| | | |__) |_ _ _ __| | __ | |     __ _ _ __ ___  _ __  _ _ __   __ _ ");
            Console.WriteLine(@"| . ` |/ _` | __| |/ _ \| '_ \ / _` | | |  ___/ _` | '__| |/ / | |    / _` | '_ ` _ \| '_ \| | '_ \ / _` |");
            Console.WriteLine(@"| |\  | (_| | |_| | (_) | | | | (_| | | | |  | (_| | |  |   <  | |___| (_| | | | | | | |_) | | | | | (_| |");
            Console.WriteLine(@"|_| \_|\__,_|\__|_|\___/|_| |_|\__,_|_| |_|   \__,_|_|  |_|\_\  \_____\__,_|_| |_| |_| .__/|_|_| |_|\__, |");
            Console.WriteLine(@"                                                                                     | |             __/ |");
            Console.WriteLine(@" _____                                _   _                _____           _         |_|            |___/ ");
            Console.WriteLine(@"|  __ \                              | | (_)              / ____|         | |                             ");
            Console.WriteLine(@"| |__) |___  ___  ___ _ ____   ____ _| |_ _  ___  _ __   | (___  _   _ ___| |_ ___ _ __ ___               ");
            Console.WriteLine(@"|  _  // _ \/ __|/ _ \ '__\ \ / / _` | __| |/ _ \| '_ \   \___ \| | | / __| __/ _ \ '_ ` _ \              ");
            Console.WriteLine(@"| | \ \  __/\__ \  __/ |   \ V / (_| | |_| | (_) | | | |  ____) | |_| \__ \ ||  __/ | | | | |             ");
            Console.WriteLine(@"|_|  \_\___||___/\___|_|    \_/ \__,_|\__|_|\___/|_| |_| |_____/ \__, |___/\__\___|_| |_| |_|             ");
            Console.WriteLine(@"                                                                  __/ |                                   ");
            Console.WriteLine(@"                                                                 |___/                                    ");
            Console.WriteLine();

        }
    }
}
