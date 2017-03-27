using System;
using System.Collections.Generic;

namespace RealPrice.Models
{
    public partial class SummaryData
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string SellType { get; set; }
        public string District { get; set; }
        public string CaseT { get; set; }
        public string Location { get; set; }
        public DateTime? Sdate { get; set; }
        public double Avg { get; set; }
    }
}
