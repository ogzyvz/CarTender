using Infoline.WorkOfTimeManagement.BusinessData;
using Infoline.WorkOfTimeManagement.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTimeManagement.WebProject.Areas.SH.Controllers
{
    public class SH_RoleController : Controller
    {

        public ActionResult Index()
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetSH_VWSH_RolePageReportSummary() ?? new VWSH_RolePageReport();
            return View(data);
        }


        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new IntranetManagementDatabase();
            var data = db.GetSH_Role(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetSH_RoleCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(Guid id)
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetSH_RoleById(id);
            return View(data);
        }


        public ActionResult Insert()
        {
            var data = new SH_Role
            {
                id = Guid.NewGuid(),
                idcode = Extensions.GetIdCode()
            };
            return View(data);
        }

        [AllowEveryone]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_Role item)
        {
            var db = new IntranetManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var control = db.GetSH_RoleControl(item.rolname);
            if (control == false)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı rol adı zaten bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertSH_Role(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update(Guid id)
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetSH_RoleById(id);
            return View(data);
        }

        [AllowEveryone]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_Role item)
        {
            var db = new IntranetManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var control = db.GetSH_RoleUpdateControl(item.id, item.rolname);
            if (control == false)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı rol adı zaten bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateSH_Role(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new IntranetManagementDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new SH_Role { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_Role(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}

