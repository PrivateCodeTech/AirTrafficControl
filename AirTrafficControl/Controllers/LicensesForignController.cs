using AirTrafficControl.Models;
using AirTrafficControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirTrafficControl.Controllers
{
    public class LicensesForignController : Controller
    {
        private Entities db = new Entities();
        // GET: LicensesForign
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData()
        {
            try
            {
                List<LicensesForignVM> data = new List<LicensesForignVM>();

                if(db.Licenses.Any(x => x.IsPayed == false & x.LicensesTypeId == 2))
                {
                    foreach(var p in db.Licenses.Where(x => x.IsPayed == false & x.LicensesTypeId == 2))
                    {
                        LicensesForignVM obj = new LicensesForignVM();

                        obj.Id = p.Id;
                        obj.LicensesTypeId = p.LicensesTypeId;
                        obj.LicensesTypeName = p.LicensesType.Name;
                        obj.CenterId = p.CenterId;
                        obj.CenterName = p.Centre.Name;
                        obj.CompanyId = p.CompanyId;
                        obj.CompanyName = p.Company.CommercialName;
                        obj.IssueDate = p.IssueDate;
                        obj.ExpiryDate = p.ExpiryDate;
                        obj.IssueDateStr = p.IssueDate.Value.Day + "/" + p.IssueDate.Value.Month + "/" + p.IssueDate.Value.Year;
                        obj.ExpiryDateStr = p.ExpiryDate.Value.ToShortDateString();
                        obj.Statement = p.Statement;
                        obj.Year = p.Year;
                        obj.IsPayed = p.IsPayed;
                        obj.PaymentStatus = p.IsPayed == true ? "مدفوعة" : "غير مدفوعة";

                        data.Add(obj);
                    }
                }

                data = data.OrderByDescending(x => x.IssueDate).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);                
            }
            catch(Exception e)
            {
                return Json("e", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(LicensesForignVM model)
        {
            try
            {
                if(model == null)
                {
                    return Json(new { Status = "error", Title = "خطأ", Message = "عفوا يوجد خطأ في البيانات" }, JsonRequestBehavior.AllowGet);
                }

                if(!db.Licenses.Any(x=>x.CompanyId == model.CompanyId & x.Year == model.IssueDate.Value.Year))
                {
                    License Obj = new License();

                    Obj.LicensesTypeId = 2;
                    Obj.CompanyId = model.CompanyId;
                    Obj.CenterId = model.CenterId;
                    Obj.IssueDate = model.IssueDate;
                    Obj.ExpiryDate = model.ExpiryDate;
                    Obj.Statement = model.Statement;
                    Obj.Year = Convert.ToDateTime(model.IssueDate).Year;
                    Obj.IsPayed = false;

                    db.Licenses.Add(Obj);
                    db.SaveChanges();

                    return Json(new { Status = "success", Title = "نجاح", Message = "تمت عملية الحفظ بي نجاح" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "warning", Title = "خطأ", Message = "تم استخراج ترخيص من قبل في هذه السنة" }, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception e)
            {
                return Json(new { Status = "error", Title = "خطأ", Message = "عفوا يوجد خطأ في عملية الحفظ" }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public ActionResult Edit(LicensesForignVM model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return Json(new { Status = "error", Title = "خطأ", Message = "عفوا يوجد خطأ في البيانات" }, JsonRequestBehavior.AllowGet);
                }

                if (db.Licenses.Any(x => x.Id == model.Id))
                {
                    License Obj = db.Licenses.Find(model.Id);

                    Obj.LicensesTypeId = 2;
                    Obj.CompanyId = model.CompanyId;
                    Obj.CenterId = model.CenterId;
                    Obj.IssueDate = model.IssueDate;
                    Obj.ExpiryDate = model.ExpiryDate;
                    Obj.Statement = model.Statement;
                    Obj.Year = Convert.ToDateTime(model.IssueDate).Year;
                    Obj.IsPayed = Obj.IsPayed;

                    db.Entry(Obj).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Status = "success", Title = "نجاح", Message = "تمت عملية التعديل بي نجاح" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "warning", Title = "خطأ", Message = "تم استخراج ترخيص من قبل في هذه السنة" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "error", Title = "خطأ", Message = "عفوا يوجد خطأ في عملية الحفظ" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}