using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirTrafficControl.Models
{
    public class ViewModels
    {
        public int Id { get; set; }
        public string LicensesType { get; set; }
        public int LicensesTypeId { get; set; }
        public int LicenseId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Stamp { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string PaymentReceiptPath { get; set; }
      
    }
}