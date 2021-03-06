//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fc_Estimates.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estimate
    {
        public int estimateId { get; set; }
        public string medicalProcedure { get; set; }
        public Nullable<decimal> minimumCost { get; set; }
        public Nullable<decimal> maximumCost { get; set; }
        public string department { get; set; }
        public string sourceOf { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> dateAdded { get; set; }
        public Nullable<System.DateTime> lastUpdated { get; set; }
        public Nullable<decimal> hospitalEstimate { get; set; }
        public Nullable<decimal> physicianEstimate { get; set; }
        public string cpt { get; set; }
    }
}
