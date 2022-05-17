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
using Infoline.Framework.Database;

namespace Infoline.WorkOfTimeManagement.WebProject.Areas.SH.Controllers
{
    public class SH_PagesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetSH_Pages(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetSH_PagesCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetSH_PagesById(id);
            return View(data);
        }

        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetSH_PagesById(id);
            ViewBag.Roles = db.GetSH_PagesRoleByActionId(data.id).Where(a => a.status == true).OrderBy(a => a.created).Select(c => c.roleid);

            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_Pages item, string[] RoleList)
        {
            var db = new WorkOfTimeManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();


            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var deleteResult = db.BulkDeleteSH_PagesRole(db.GetSH_PagesRoleByActionId(item.id).ToList());

            if (RoleList != null)
            {
                foreach (var roleid in RoleList)
                {
                    db.InsertSH_PagesRole(new SH_PagesRole
                    {
                        roleid = new Guid(roleid),
                        actionid = item.id,
                        status = true,
                        id = Guid.NewGuid(),
                        created = item.changed,
                        createdby = item.changedby,
                    });
                }
            }


            var dbresult = db.UpdateSH_Pages(item);
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

            var item = id.Select(a => new SH_Pages { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_Pages(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}

