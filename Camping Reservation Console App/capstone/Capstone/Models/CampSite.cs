using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class CampSite
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }
        public double Fee { get; set; }
        public double TotalFee { get; set; }
        
    }
}
