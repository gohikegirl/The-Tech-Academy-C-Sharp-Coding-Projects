using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceProject.ViewModels
{
    public class SubscribersVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> CarYear { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public Nullable<bool> DUI { get; set; }
        public Nullable<int> NumOfTickets { get; set; }
        public string CoverageType { get; set; }
        public decimal Quote { get; set; }
        public Nullable<System.DateTime> Removed { get; set; }
    }
}