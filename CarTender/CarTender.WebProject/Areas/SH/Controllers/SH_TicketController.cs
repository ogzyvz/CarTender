using CarTender.BusinessData;
using CarTender.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CarTender.WebProject.Areas.SH.Controllers
{
    public class SH_TicketController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetVWSH_Ticket().RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_TicketCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetVWSH_TicketById(id);
            return View(data);
        }


        public ActionResult Insert()
        {
            var data = new SH_Ticket { id = Guid.NewGuid() };
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_Ticket item)
        {
            var db = new WorkOfTimeManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = db.InsertSH_Ticket(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetSH_TicketById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_Ticket item)
        {
            var db = new WorkOfTimeManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();



            var dbresult = db.UpdateSH_Ticket(item);
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
            var db = new WorkOfTimeManagementDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new SH_Ticket { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_Ticket(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}

