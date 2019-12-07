using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportService.Web.Models.Activity
{
    public class RouteDetails
    {
        public int RouteID { get; set; }
        public int TripID { get; set; }
        public int CityID { get; set; }
        public decimal FreeHeight { get; set; }
        public decimal FreeWidth { get; set; }
        public decimal FreeLength { get; set; }
        public decimal FreeWeight { get; set; }
    }
}