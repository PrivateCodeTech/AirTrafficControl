using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirTrafficControl.Models;
using System.IO;

namespace AirTrafficControl.Controllers
{
    public class PaymentReceiptsController : Controller
    {
        private Entities db = new Entities();

        // GET: PaymentReceipts
        public ActionResult IndexForLicense()
        {
            
            return View();
        }

        // GET: PaymentReceipts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData()
        {
            var data = db.PaymentReceipts.Select(p => new
            {
                Id = p.Id,
                LicenseId = p.LicensesId,
                licensesType = p.License.LicensesType.Name,
                Price = p.Price,
                Tax = p.Tax,
                Stamp = p.Stamp,
                TotalAmount = p.TotalAmount
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDataForLicense()
        {
            var data = db.Licenses.Where(x=> x.IsPayed != true).Select(p => new
            {
                Id = p.Id,
                LicensesType = p.LicensesType.Name,
                Company = p.Company.CommercialName,
                Center = p.Centre.Name,
                Statement = p.Statement,
                ExpiryDate = p.ExpiryDate.Value.Day + "/" + p.ExpiryDate.Value.Month + "/" + p.ExpiryDate.Value.Year,
                Year = p.Year,
                Status = p.IsPayed == true ? "مدفوعه" : "غير مدفوعة",
                IssueDate = p.ExpiryDate.Value.Day + "/" + p.ExpiryDate.Value.Month + "/" + p.ExpiryDate.Value.Year
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult Create(ViewModels data)
        {
            decimal? TotalPayment;   
            string fname;
            try
            {
                PaymentReceipt e = new PaymentReceipt();

                e.LicensesId = data.LicenseId;
                e.Price = data.Price;
                e.Tax = data.Tax;
                e.Stamp = data.Stamp;

                var tax = data.Price * (data.Tax / 100);
                TotalPayment = (data.Price) - tax - (data.Stamp);
                e.TotalAmount = TotalPayment;
                
                db.PaymentReceipts.Add(e);
                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            string uploadPath = Server.MapPath("~/Files/");
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }

                            fname = "PaymentReceipt_No_" + e.Id + "_PaymentReceiptNo" + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(uploadPath, fname);
                            file.SaveAs(filePath);
                            e.PaymentReceiptPath = fname;
                        }
                    }
                }

                var LicenseObj = db.Licenses.Find(data.LicenseId);

                LicenseObj.IsPayed = true;

                db.Entry(LicenseObj).State = EntityState.Modified;
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();
                
                return Json(new { Message = "تمت عملية الإضافة بنجاح", Title = "نجاح", Status = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "فشل في حفظ البيانات. " + ex.Message, Title = "خطأ", Status = "error" });
            }
        }

        [HttpPost]
        public ActionResult Edit(ViewModels data)
        {
            decimal? TotalPayment;
            string fname;
            try
            {
                PaymentReceipt e = db.PaymentReceipts.Find(data.Id);
                if (e == null)
                {
                    return Json(new { Message = "لايوجد توريد", Title = "خطأ", Status = "error" });
                }
                
                e.Price = data.Price;
                e.Tax = data.Tax;
                e.Stamp = data.Stamp;

                var tax = data.Price * (data.Tax / 100);
                TotalPayment = (data.Price) - tax - (data.Stamp);
                e.TotalAmount = TotalPayment;

                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            string uploadPath = Server.MapPath("~/Files/");
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }

                            fname = "PaymentReceipt_No_" + e.Id + "_PaymentReceiptNo" + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(uploadPath, fname);
                            file.SaveAs(filePath);
                            e.PaymentReceiptPath = fname;
                        }
                    }
                }

                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Message = "تمت عملية الإضافة بنجاح", Title = "نجاح", Status = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "فشل في حفظ البيانات. " + ex.Message, Title = "خطأ", Status = "error" });
            }
        }
        
        [HttpGet]
        public int Delete(int Id)
        {
            try
            {
                PaymentReceipt e = db.PaymentReceipts.Find(Id);
                if (e == null)
                {
                    return -1;
                }

                var LicenseObj = db.Licenses.Find(e.LicensesId);
                LicenseObj.IsPayed = false;

                db.Entry(LicenseObj).State = EntityState.Modified;

                db.PaymentReceipts.Remove(e);
                db.SaveChanges();

                //return Json(new { Message = "تمت عملية الإضافة بنجاح", Title = "نجاح", Status = "success" });
                return 1;
            }
            catch (Exception ex)
            {
                //return Json(new { Message = "فشل في حفظ البيانات. " + ex.Message, Title = "خطأ", Status = "error" });
                return -1;
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
