using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportService.Web.Models.Masters
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int VehicleTypeID { get; set; }
        public string VehicleNo { get; set; }
        public int MyProperty { get; set; }
        public int OwnerID { get; set; }
        public string DocumentPath { get; set; }
        public int IsActive { get; set; }
    }
}