using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.Models
{
    public class Park
    {
        public int Rownumber { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public double Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

    }
}
