using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class CampGround
    {
        
        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpenFromMonth { get; set; }
        public int CloseFromMonth { get; set; }
        public double DailyFee { get; set; }

    }
}
