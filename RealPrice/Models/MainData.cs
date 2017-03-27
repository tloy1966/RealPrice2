using System;
using System.Collections.Generic;

namespace RealPrice.Models
{
    public partial class MainData
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string SellType { get; set; }
        public string District { get; set; }
        public string CaseT { get; set; }
        public string Location { get; set; }
        public decimal? Landa { get; set; }
        public string CaseF { get; set; }
        public string LandaX { get; set; }
        public string LandaY { get; set; }
        public DateTime? Sdate { get; set; }
        public string Scnt { get; set; }
        public string Sbuild { get; set; }
        public string Tbuild { get; set; }
        public string Buitype { get; set; }
        public string Pbuild { get; set; }
        public string Mbuild { get; set; }
        public DateTime? Fdate { get; set; }
        public decimal? Farea { get; set; }
        public int? BuildR { get; set; }
        public int? BuildL { get; set; }
        public int? BuildB { get; set; }
        public string BuildP { get; set; }
        public string Rule { get; set; }
        public bool? Furniture { get; set; }
        public long? Tprice { get; set; }
        public decimal? Uprice { get; set; }
        public string Parktype { get; set; }
        public decimal? Parea { get; set; }
        public int? Pprice { get; set; }
        public string Rmnote { get; set; }
        public string Id2 { get; set; }
        public bool? IsActive { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }
}
