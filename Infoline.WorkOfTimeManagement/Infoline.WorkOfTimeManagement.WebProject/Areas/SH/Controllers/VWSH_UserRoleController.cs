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
    public class VWSH_UserRoleController : Controller
    {
        public ActionResult Index()
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetSH_UserRolePageReportSummary() ?? new VWSH_UserRolePageReport();
            return View(data);
        }


        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new IntranetManagementDatabase();
            var data = db.GetVWSH_UserRole(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_UserRoleCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Detail(Guid id)
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetVWSH_UserRoleById(id);
            return View(data);
        }


        public ActionResult Insert()
        {
            var data = new VWSH_UserRole { id = Guid.NewGuid() };

            if (!string.IsNullOrEmpty(Request.QueryString["roleid"]))
            {
                data.roleid = new Guid(Request.QueryString["roleid"]);
            }

            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_UserRole item, string[] UserIdList)
        {
            var db = new IntranetManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            if (!item.roleid.ToString().IsValidGuid() || UserIdList.Length < 1)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Geçerli bir rol bulunamadı veya Hiç kullanıcı seçilmedi")
                }, JsonRequestBehavior.AllowGet);
            }

            var userRolelist = db.GetSH_UserRoleByRoleId((Guid)item.roleid);

            foreach (var userid in UserIdList)
            {

                var userRole = userRolelist.FirstOrDefault(a => a.userid == new Guid(userid));

                if (userRole == null)
                {
                    var dbres = db.InsertSH_UserRole(new SH_UserRole
                    {
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        userid = new Guid(userid),
                        roleid = item.roleid
                    });

                }
                else
                {

                    userRole.changed = DateTime.Now;
                    userRole.changedby = userStatus.user.id;

                    var dbres = db.UpdateSH_UserRole(userRole);

                }
            }


            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Kullanıcı(lara) başarılı bir şekilde rol atandı.")
            }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Update(Guid id)
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetVWSH_UserRoleById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_UserRole item)
        {
            var db = new IntranetManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateSH_UserRole(item);
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

            var item = id.Select(a => new SH_UserRole { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_UserRole(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}

