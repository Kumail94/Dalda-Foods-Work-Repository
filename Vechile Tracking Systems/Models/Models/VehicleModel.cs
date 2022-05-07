using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vechile_Tracking_Systems.Models
{
    public class VehicleModel
    {
        public string VehicleTitle { get; set; }
        public string VehicleNature { get; set; }
        public string NIC { get; set; }
        public string DriverTitle { get; set; }
        public string TransportTitle { get; set; }
    }

    public class DetailInfo
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
    }

    public class WeightInfo
    {
        public string VehicleNo { get; set; }
        public long rfCardId { get; set; }
    }
}