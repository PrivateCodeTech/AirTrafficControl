using AirTrafficControl.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirTrafficControl.Controllers
{
    public class CenteresController : Controller
    {
        private Entities db = new Entities();

        // GET: Centeres
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData()
        {
            var data = db.Centres.Select(d => new
            {
                d.Id,
                d.Name,
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save(Centre data)
        {
            Centre g = new Centre();
            g.Name = data.Name;
            db.Centres.Add(g);
            db.SaveChanges();
            return Json(new { Message = "تمت عملية الاضافة بنجاح", Title = "نجاح", Status = "success" });
        }


        public ActionResult Edit(Centre data)
        {
            if (data.Id != 0 && data.Name != null)
            {
                int c = db.Centres.Where(f => f.Id != data.Id && f.Name == data.Name).Count();
                if (c == 0)
                {
                    Centre t = db.Centres.Find(data.Id);

                    t.Name = data.Name;


                    db.Entry(t).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { Message = "تمت عملية التعديل بنجاح", Title = "نجاح", Status = "success" });
                }

            }

            return Json(new { Message = "حدث خطأ اثناء التعديل", Title = "خطأ", Status = "error" });
        }



        public ActionResult DeleteName(Centre data)
        {
            try
            {
                if (data.Id != 0 && data.Name != null)
                {
                    int c = db.Centres.Where(f => f.Id == data.Id && f.Name == data.Name).Count();
                    if (c > 0)
                    {
                        Centre t = db.Centres.Find(data.Id);

                        // t.Name = data.Name;


                        db.Entry(t).State = EntityState.Deleted;
                        db.SaveChanges();
                        return Json(new { Message = "تمت عملية المسح بنجاح", Title = "نجاح", Status = "success" });
                    }

                }

                return Json(new { Message = "حدث خطأ اثناء المسح", Title = "خطأ", Status = "error" });
            }
            catch
            {
                return Json(new { Message = "حدث خطأ اثناء المسح لان الاسم مستخدم في بيانات اخري ", Title = "خطأ", Status = "error" });

            }
        }


    }
}
