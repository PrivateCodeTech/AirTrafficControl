using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirTrafficControl.Models;
using System.Net;
using System.Data.Entity;
using System.Data;

namespace AirTrafficControl.Controllers
{
    public class EconomicLicenseController : Controller
    {
        Entities db = new Entities();
        // GET: Ticket Sales License (Foreign Companies)

        private int EconomicLicenseId;
        private int TSLicenseId;

        public EconomicLicenseController()
        {
            EconomicLicenseId = 1;
            TSLicenseId = 2;
        }


        #region Economic License
        public ActionResult EconomicLicense()
        {
            return View();
        }

     

        public ActionResult GetAllItem()
        {

            IQueryable<License> License = db.Licenses;


            var ItemList = new List<LicenseViewModel>();
            foreach (var item in License)
            {
             
                LicenseViewModel r = new LicenseViewModel();
                r.Id = item.Id;
                r.LicensesType = item?.LicensesType.Name;
                r.Company = item?.Company.CommercialName;

                r.Center = item?.Centre.Name;
                r.Statement = item?.Statement;
                r.IssueDate = CheckDate(item?.IssueDate.ToString());
                r.ExpiryDate = CheckDate(item?.ExpiryDate.ToString());
                r.IsPayed = Convert.ToBoolean(item?.IsPayed);
                r.PayedName = CheckIsPayed(r.IsPayed);
                ItemList.Add(r);
            }

            return Json(new { data = ItemList.ToList() }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetLicensesType(string q)
        {
            var data = db.LicensesTypes.Select(p => new
            {
                id = p.Id,
                text = p.Name,

            }).Where(x => x.text.Contains(q));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompany(string q)
        {
            var data = db.Companies.Select(p => new
            {
                id = p.Id,
                text = p.CommercialName,

            }).Where(x => x.text.Contains(q));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCenter(string q)
        {
            var data = db.Centres.Select(p => new
            {
                id = p.Id,
                text = p.Name,

            }).Where(x => x.text.Contains(q));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(License data)
        {
            data.LicensesTypeId = EconomicLicenseId;
            LicenseVaildModel model = new LicenseVaildModel();
            model = CheckCreateLicensesVaild(data);
            if (model.IsValid == false) { return Json(new { Message = model.Message, Title = "فشل", Status = "error" }); }



            if (data.IssueDate.HasValue) { int year; year = Convert.ToDateTime(data.IssueDate).Year; data.Year = year; }

            db.Licenses.Add(data);
            db.SaveChanges();
            return Json(new { Message = "تمت عملية الإضافة  بنجاح", Title = "نجاح", Status = "success" });
        }





        public ActionResult GetDataForUpdate(int id)
        {
            License License = db.Licenses.Find(id);

            LicenseModel model = new LicenseModel();
            model.Id = License.Id;
            model.LicensesTypeId = Convert.ToInt32(License.LicensesTypeId);
            model.LicensesType = License.LicensesType.Name;

            model.CompanyId = Convert.ToInt32(License.CompanyId);
            model.Company =  License.Company.CommercialName;

            model.CenterId = Convert.ToInt32(License.CenterId);
            model.Center = License.Centre.Name;

            model.CenterId = Convert.ToInt32(License.CenterId);
            model.Statement = License.Statement;
            model.Year = Convert.ToInt32(License.Year);

            model.IssueDate = Convert.ToDateTime(License.IssueDate).ToString("yyyy-MM-dd");
            model.ExpiryDate = Convert.ToDateTime(License.ExpiryDate).ToString("yyyy-MM-dd");

            return Json(model, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License License = db.Licenses.Find(id);
            if (License == null)
            {
                return HttpNotFound();
            }

        
            //check is Has child
            if (License.IsPayed == true)
            {
                return Json(new { success = true, message = "Used", MessageText = "لايمكن حذف هذا العنصر", Title = "تعذر الحذف" }, JsonRequestBehavior.AllowGet);
            }

            var LicenseDelete = db.Licenses.Find(id);
            db.Licenses.Remove(LicenseDelete);
            db.SaveChanges();
            return Json(new { success = true, message = "Delete", MessageText = "تم الحذف الترخيص", Title = " الحذف" }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Edit(License data)
        {

            if (data.Id >= 0)
            {
                data.LicensesTypeId = EconomicLicenseId;
                LicenseVaildModel model = new LicenseVaildModel();
                model = CheckUpdateLicensesVaild(data);
                if (model.IsValid == false) { return Json(new { Message = model.Message, Title = "فشل", Status = "error" }); }


                if (data.IssueDate.HasValue) { int year; year = Convert.ToDateTime(data.IssueDate).Year; data.Year = year; }

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Message = "تمت عملية التعديل بنجاح", Title = "نجاح", Status = "success" });
            }
            else
            {

                return Json(new { Message = "حدث خطأ اثناء التعديل", Title = "خطأ", Status = "error" });

            }

        }

        public LicenseVaildModel CheckCreateLicensesVaild(License data)
        {
            LicenseVaildModel model = new LicenseVaildModel();
            model.IsValid = true; model.Message = "نجاح";

            if (data.IssueDate.HasValue && data.ExpiryDate.HasValue)
            {
                if (data.IssueDate > data.ExpiryDate) { model.IsValid = false; model.Message = "لايمكن تاريخ الاصدار اصغر من تاريخ الانتهاء "; }
                if (db.Licenses.Any(x => x.CompanyId == data.CompanyId && x.ExpiryDate >= data.ExpiryDate && x.LicensesTypeId == data.LicensesTypeId)) { model.IsValid = false; model.Message = "يوجد ترخيص لهذة الشركة ساري المفعول"; }
                if (db.Licenses.Any(x => x.CompanyId == data.CompanyId && x.IssueDate >= data.IssueDate && x.LicensesTypeId ==data.LicensesTypeId)) { model.IsValid = false; model.Message = "يوجد ترخيص لهذة الشركة ساري المفعول"; }
              
            }
            return model;
        }
        public LicenseVaildModel CheckUpdateLicensesVaild(License data)
        {
            LicenseVaildModel model = new LicenseVaildModel();
            model.IsValid = true; model.Message = "نجاح";

            if (data.IssueDate.HasValue && data.ExpiryDate.HasValue)
            {
                if (data.IssueDate > data.ExpiryDate) { model.IsValid = false; model.Message = "لايمكن تاريخ الاصدار اصغر من تاريخ الانتهاء "; }
                if (db.Licenses.Any(x => x.CompanyId == data.CompanyId && x.ExpiryDate >= data.ExpiryDate && x.Id != data.Id && x.LicensesTypeId == data.LicensesTypeId)) { model.IsValid = false; model.Message = "يوجد ترخيص لهذة الشركة ساري المفعول"; }
                if (db.Licenses.Any(x => x.CompanyId == data.CompanyId && x.IssueDate >= data.IssueDate && x.Id != data.Id && x.LicensesTypeId == data.LicensesTypeId)) { model.IsValid = false; model.Message = "يوجد ترخيص لهذة الشركة ساري المفعول"; }

            }
            return model;
        }

















        #endregion

        #region Taket Sales Liences(TSL)
        public ActionResult TSLForForeign()
        {
            return View();
        }

        public ActionResult GetAllItemTSL()
        {

            IQueryable<License> License = db.Licenses.Where(x=>x.LicensesTypeId == TSLicenseId);


            var ItemList = new List<LicenseViewModel>();
            foreach (var item in License)
            {

                LicenseViewModel r = new LicenseViewModel();
                r.Id = item.Id;
                r.LicensesType = item?.LicensesType.Name;
                r.Company = item?.Company.CommercialName;

                r.Center = item?.Centre.Name;
                r.Statement = item?.Statement;
                r.IssueDate = CheckDate(item?.IssueDate.ToString());
                r.ExpiryDate = CheckDate(item?.ExpiryDate.ToString());
                r.IsPayed = Convert.ToBoolean(item?.IsPayed);
                r.PayedName = CheckIsPayed(r.IsPayed);
                ItemList.Add(r);
            }

            return Json(new { data = ItemList.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateTSL(License data)
        {
            // Add TSLID
            data.LicensesTypeId = TSLicenseId;
            LicenseVaildModel model = new LicenseVaildModel();
            model = CheckCreateLicensesVaild(data);
            if (model.IsValid == false) { return Json(new { Message = model.Message, Title = "فشل", Status = "error" }); }



            if (data.IssueDate.HasValue) { int year; year = Convert.ToDateTime(data.IssueDate).Year; data.Year = year; }

            db.Licenses.Add(data);
            db.SaveChanges();
            return Json(new { Message = "تمت عملية الإضافة  بنجاح", Title = "نجاح", Status = "success" });
        }
        public ActionResult EditTSL(License data)
        {

            if (data.Id >= 0)
            {
                data.LicensesTypeId = TSLicenseId;
                LicenseVaildModel model = new LicenseVaildModel();
                model = CheckUpdateLicensesVaild(data);
                if (model.IsValid == false) { return Json(new { Message = model.Message, Title = "فشل", Status = "error" }); }


                if (data.IssueDate.HasValue) { int year; year = Convert.ToDateTime(data.IssueDate).Year; data.Year = year; }

                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Message = "تمت عملية التعديل بنجاح", Title = "نجاح", Status = "success" });
            }
            else
            {

                return Json(new { Message = "حدث خطأ اثناء التعديل", Title = "خطأ", Status = "error" });

            }

        }




        #endregion


        #region View Models Using in The Structure


        public class LicenseViewModel
        {
            public int Id { get; set; }
            public string LicensesType { get; set; }
            public string Company { get; set; }
            public string Center { get; set; }
            public string Statement { get; set; }
            public string IssueDate { get; set; }
            public string ExpiryDate { get; set; }
            public bool IsPayed { get; set; }
            public string PayedName { get; set; }


        }

        public class LicenseModel
        {
            public int Id { get; set; }
            public int LicensesTypeId { get; set; }
            public string LicensesType { get; set; }
            public int CompanyId { get; set; }
            public string Company { get; set; }

            public int CenterId { get; set; }
            public string Center { get; set; }

            public string Statement { get; set; }
            public string IssueDate { get; set; }
            public string ExpiryDate { get; set; }
            public bool IsPayed { get; set; }
            public string PayedName { get; set; }
            public int Year { get; set; }
            

        }

        public class LicenseVaildModel
        {
            public bool IsValid { get; set; }

            public string Message { get; set; }



        }

        public string CheckDate(string d)
        {
            string value = "";
            if (d == null || d == "" || string.IsNullOrEmpty(d))
            {
                value = null;
                return value;
            }
            else
            {
                value = Convert.ToDateTime(d).ToString("dd/MM/yyyy");
                return value;
            }
        }
        public string CheckIsPayed(bool d)
        {
            if (d == true) { return "مدفوع"; } else { return "غير مدفوع"; }
        }



        #endregion



    }
}