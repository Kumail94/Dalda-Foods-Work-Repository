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
    
    public partial class tblVehicleEntryDetail
    {
        public long TranDtlID { get; set; }
        public Nullable<long> TranID { get; set; }
        public Nullable<long> CompanyID { get; set; }
        public Nullable<long> RFDeviceLocationID { get; set; }
        public string IPAddress { get; set; }
        public Nullable<long> RFWorkCodeID { get; set; }
        public Nullable<int> Activity { get; set; }
        public Nullable<System.DateTime> ActivityDate { get; set; }
        public string ActivityTime { get; set; }
        public Nullable<bool> IsWorkCodeUsed { get; set; }
        public Nullable<int> RouteOrderNo { get; set; }
        public Nullable<double> WeightInKg { get; set; }
        public string QCNo { get; set; }
        public string QCResult { get; set; }
        public string Remarks { get; set; }
    }
}