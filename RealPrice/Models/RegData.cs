using System;
using System.Collections.Generic;

namespace RealPrice.Models
{
    public partial class RegData
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string SellType { get; set; }
        public string District { get; set; }
        public string PBuild { get; set; }
        public string PLocation { get; set; }
        public int? Count { get; set; }
        public double? PUprice { get; set; }
        public double? PDayFromBuild { get; set; }
        public double? PLanda { get; set; }
        public double? PBuildR { get; set; }
        public double? PBuildL { get; set; }
        public double? PBuildB { get; set; }
        public double? PBuildP { get; set; }
        public double? PRule { get; set; }
        public double? PRmnote { get; set; }
        public DateTime? Modifydate { get; set; }
    }
}
