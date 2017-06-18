using System;
using System.Collections.Generic;

namespace RealPrice.Models
{
    public partial class Mrtgeo
    {
        public string Mrt { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string PlaceId { get; set; }
        public string Location { get; set; }
    }
}
