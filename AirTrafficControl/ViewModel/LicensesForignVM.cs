using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirTrafficControl.ViewModel
{
    public class LicensesForignVM
    {
        public int Id { get; set; }
        public int? LicensesTypeId { get; set; }
        public int? CompanyId { get; set; }
        public int? CenterId { get; set; }
        public string Statement { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssueDateStr { get; set; }
        public string ExpiryDateStr { get; set; }
        public int? Year { get; set; }
        public bool? IsPayed { get; set; }
        public string LicensesTypeName { get; set; }
        public string CompanyName { get; set; }
        public string CenterName { get; set; }
        public string PaymentStatus { get; set; }

    }
}