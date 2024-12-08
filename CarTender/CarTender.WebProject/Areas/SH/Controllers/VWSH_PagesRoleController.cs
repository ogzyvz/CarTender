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
    public class VWSH_PagesRoleController : Controller
    {

        public ActionResult Index()
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetSH_DBVWSH_PagesRolePageReportSummary() ?? new VWSH_PagesRolePageReport();
            return View(data);
        }


        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetVWSH_PagesRole(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_PagesRoleCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetVWSH_PagesRoleById(id);
            return View(data);
        }


        public ActionResult Insert()
        {
            var data = new VWSH_PagesRole { id = Guid.NewGuid() };

            if (!string.IsNullOrEmpty(Request.QueryString["roleid"]))
            {
                data.roleid = new Guid(Request.QueryString["roleid"]);
            }

            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_PagesRole item, string[] ActionIdList)
        {
            var db = new WorkOfTimeManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            if (!item.roleid.ToString().IsValidGuid() || ActionIdList.Length < 1)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Geçerli bir rol bulunamadı veya Hiç sayfa seçilmedi")
                }, JsonRequestBehavior.AllowGet);
            }

            var pageRolelist = db.GetSH_PagesRoleByRoleId((Guid)item.roleid);

            foreach (var pageId in ActionIdList)
            {

                var pageRole = pageRolelist.FirstOrDefault(a => a.actionid == new Guid(pageId));

                if (pageRole == null)
                {
                    var dbres = db.InsertSH_PagesRole(new SH_PagesRole
                    {
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        actionid = new Guid(pageId),
                        status = item.status,
                        roleid = item.roleid
                    });
                }
                else
                {
                    pageRole.changed = DateTime.Now;
                    pageRole.changedby = userStatus.user.id;
                    pageRole.status = item.status;
                    var dbres = db.UpdateSH_PagesRole(pageRole);
                }
            }


            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Sayfa(lara) başarılı bir şekilde rol atandı.")
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var data = db.GetVWSH_PagesRoleById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_PagesRole item)
        {
            var db = new WorkOfTimeManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateSH_PagesRole(item);
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

            var item = id.Select(a => new SH_PagesRole { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_PagesRole(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}

