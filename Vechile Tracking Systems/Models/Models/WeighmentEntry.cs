//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vechile_Tracking_Systems.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WeighmentEntry
    {
        public int VehicleID { get; set; }
        public string VehicleNo { get; set; }
        public string ConsecNo { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string WeighmentDetails { get; set; }
        public string QrCode { get; set; }
    }
}