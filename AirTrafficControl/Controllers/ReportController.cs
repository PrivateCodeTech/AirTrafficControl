using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Threading;
using AirTrafficControl.Models;
using NumberToWord;
using AirTrafficControl.Function;

namespace AirTrafficControl.Controllers
{
    public class ReportController : Controller
    {
        Entities db = new Entities();
        List<CurrencyInfo> currencies = new List<CurrencyInfo>();

        public ReportController()
        {
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Sudan));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Dollar));
            //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.UAE));
            //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
            //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Tunisia));
            //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Gold));
        }

        public ActionResult Print()
        {
            return View();
        }

        // GET: Report
        public ActionResult PrintData(int id)
        {
          
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/Receipt/Receipt.rpt")));
            IQueryable<PaymentReceipt> Receipt;
            Receipt = db.PaymentReceipts.Where(x => x.Id == id);
            PaymentReceipt R = db.PaymentReceipts.Where(x => x.Id == id).FirstOrDefault();
            double TotalAmount = Convert.ToDouble(Math.Round(Convert.ToDouble(R.TotalAmount), 2));
            ToWord word = new ToWord(Convert.ToDecimal(TotalAmount), currencies[Convert.ToInt32(1)]);
            string text = "(" + word.ConvertToArabic() + ")";

            var ReceiptList = new List<PrintModel>();

            foreach (var item in Receipt)
            {
                PrintModel F = new PrintModel();
                F.Id = item.Id;
                F.Name = item?.License.Company.CommercialName;
                F.Price = Convert.ToDecimal(item?.Price);
                F.Type = item.License.LicensesType.Name;
                F.Word = text;
                F.Stamp = Convert.ToDecimal(item?.Stamp);
                F.Tax = Convert.ToDecimal(item?.Tax);
                F.TotalAmount = Convert.ToDecimal(item?.TotalAmount);
                ReceiptList.Add(F);
            }

            rd.SetDataSource(ReceiptList.Select(p => new
            {

                Price = p.Price,
                Tax = p.Tax,
                Stamp = p.Stamp,
                TotalAmount = p.TotalAmount,
                Name = p.Name,
                Type = p.Type,
                Word = p.Word,
            }).ToList());



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream Stream = rd.ExportToStream
                //WordForWindows
                (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);//application/doc
            return File(Stream, "application/pdf", "Receipt.pdf");
        }



        public ActionResult GetLicense(int? CompanyId, int? LicenseTypeId, int? Year, int? Id)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/License/LicenseRep.rpt")));
            IQueryable<License> License;
            License = db.Licenses;


            if (Id != null)
            {

                rd.SetDataSource(License.Select(p => new
                {
                    Id = p.Id,
                    LicensesTypeId = p.LicensesTypeId ?? 0,
                    LicensesType = p.LicensesType.Name ?? "",
                    CompanyId = p.Company.Id,
                    CompanyName = p.Company.CommercialName ?? "",
                    CommercialName = p.Company.CommercialName ?? "",
                    CommercialNo = p.Company.CommercialNo ?? 0,
                    Phone = p.Company.Phone ?? "",
                    Email = p.Company.Email ?? "",
                    EmployerName = p.Company.EmployerName ?? "",
                    CenterName = p.Centre.Name ?? "",
                    Statement = p.Statement ?? "",
                    //IssueDate = p.IssueDate.ToString(),
                    //ExpiryDate = p.ExpiryDate.ToString(),
                    Year = p.Year ?? 0,
                    IsPayed = p.IsPayed ?? false,
                    //

                }).Where(c => c.Id == Id).ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream Stream1 = rd.ExportToStream
                    ///WordForWindows
                    (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream1.Seek(0, SeekOrigin.Begin);//application/doc
                return File(Stream1, "application/pdf");
            }

            else
            {

                rd.SetDataSource(License.Select(p => new
                {
                    Id = p.Id,
                    LicensesTypeId = p.LicensesTypeId ?? 0,
                    LicensesType = p.LicensesType.Name ?? "",
                    CompanyId = p.Company.Id,
                    CompanyName = p.Company.CommercialName ?? "",
                    CommercialName = p.Company.CommercialName ?? "",
                    CommercialNo = p.Company.CommercialNo ?? 0,
                    Phone = p.Company.Phone ?? "",
                    Email = p.Company.Email ?? "",
                    EmployerName = p.Company.EmployerName ?? "",
                    CenterName = p.Centre.Name ?? "",
                    Statement = p.Statement ?? "",
                    IssueDate = p.IssueDate.ToString(),
                    ExpiryDate = p.ExpiryDate.ToString(),
                    Year = p.Year ?? 0,
                    IsPayed = p.IsPayed ?? false,
                    //

                }).Where(c => c.LicensesTypeId == LicenseTypeId && c.CompanyId == CompanyId && c.IsPayed == true && c.Year == Year).ToList());

            }


            rd.SetDataSource(License.Select(p => new
            {
                Id = p.Id,
                LicensesTypeId = p.LicensesTypeId ?? 0,
                LicensesType = p.LicensesType.Name ?? "",
                CompanyId = p.Company.Id,
                CompanyName = p.Company.CommercialName ?? "",
                CommercialName = p.Company.CommercialName ?? "",
                CommercialNo = p.Company.CommercialNo ?? 0,
                Phone = p.Company.Phone ?? "",
                Email = p.Company.Email ?? "",
                EmployerName = p.Company.EmployerName ?? "",
                CenterName = p.Centre.Name ?? "",
                Statement = p.Statement ?? "",
                IssueDate = p.IssueDate.ToString(),
                ExpiryDate = p.ExpiryDate.ToString(),
                Year = p.Year ?? 0,
                IsPayed = p.IsPayed ?? false,
                //

            }).Where(c => c.LicensesTypeId == LicenseTypeId && c.CompanyId == CompanyId && c.IsPayed == true && c.Year == Year).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream Stream = rd.ExportToStream
                    ///WordForWindows
                    (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Stream.Seek(0, SeekOrigin.Begin);//application/doc
            return File(Stream, "application/pdf");



        }












        public class PrintModel
        {
            public int Id { get; set; }
            public decimal Price { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Word { get; set; }
            public decimal Tax { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal Stamp { get; set; }



        }


    }
}