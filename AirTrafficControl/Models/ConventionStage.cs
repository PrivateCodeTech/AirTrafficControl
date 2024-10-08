//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirTrafficControl.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConventionStage
    {
        public int Id { get; set; }
        public Nullable<int> ConventionId { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public Nullable<System.DateTime> ExitDate { get; set; }
    
        public virtual Convention Convention { get; set; }
        public virtual Stage Stage { get; set; }
    }
}
