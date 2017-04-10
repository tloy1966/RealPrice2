using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealPrice.Models
{
    public class Geo
    {
        public string statName { get; set; }
        public string formattedAddress { get; set; }

        public double lat { get; set; }
        public double lng { get; set; }

        public string place_id { get; set; }
    }
}
