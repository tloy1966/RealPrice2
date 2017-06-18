using System;
using System.Collections.Generic;

namespace RealPrice.Models
{
    public partial class Location
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Location1 { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public double? Avg { get; set; }
        public double? DisMrt { get; set; }
        public double? DurationMrt { get; set; }
    }
}
