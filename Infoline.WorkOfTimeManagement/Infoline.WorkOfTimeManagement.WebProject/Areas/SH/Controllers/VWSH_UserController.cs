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
    public class VWSH_UserController : Controller
    {
        public ActionResult Index()
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetSH_VWSH_UserPageReportSummary() ?? new VWSH_UserPageReport();
            return View(data);
        }


        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new IntranetManagementDatabase();
            var data = db.GetVWSH_User(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_UserCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(Guid id)
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetVWSH_UserById(id);
            return View(data);
        }


        public ActionResult Insert()
        {
            var data = new VWSH_User
            {
                id = Guid.NewGuid(),
                idcode = Extensions.GetIdCode()
            };
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_User item)
        {
            var db = new IntranetManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var validate = ((ResultStatus)ValidateTc(item.tckimlikno).Data);
            if (!validate.result)
            {
                return Json(new ResultStatusUI
                {
                    Result = validate.result,
                    FeedBack = feedback.Warning(validate.message)
                }, JsonRequestBehavior.AllowGet);
            }

            validate = ((ResultStatus)ValidateLoginName(item.loginname).Data);
            if (!validate.result)
            {
                return Json(new ResultStatusUI
                {
                    Result = validate.result,
                    FeedBack = feedback.Warning(validate.message)
                }, JsonRequestBehavior.AllowGet);
            }
            validate = ((ResultStatus)ValidateEmail(item.email).Data);
            if (!validate.result)
            {
                return Json(new ResultStatusUI
                {
                    Result = validate.result,
                    FeedBack = feedback.Warning(validate.message)
                }, JsonRequestBehavior.AllowGet);
            }

            if (item.password != Request["rePassword"])
            {
                return Json(new ResultStatusUI
                {
                    Result = validate.result,
                    FeedBack = feedback.Warning("Şifreler Uyuşmuyor")
                }, JsonRequestBehavior.AllowGet);
            }



            item.type = item.type ?? (int)EnumSH_UserType.Kullanıcı;
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            item.status = (bool)item.status;
            item.password = item.password != null ? db.GetMd5Hash(db.GetMd5Hash(item.password)) : item.password;
            var dbresult = db.InsertSH_User(item);


            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Custom("Kullanıcı Kaydetme işlemi başarılı.", (string.IsNullOrEmpty(Request["returnUrl"]) ? Url.Action("Index", "VWSH_User") : Request["returnUrl"]), "Bilgilendirme", "success", 2, false) : feedback.Error("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Guid id)
        {
            var db = new IntranetManagementDatabase();
            var data = db.GetVWSH_UserById(id);
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_User item)
        {
            var db = new IntranetManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var user = db.GetSH_UserById(item.id);

            if (!String.IsNullOrEmpty(item.password) && item.password != Request["rePassword"])
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Şifreler Uyuşmuyor")
                }, JsonRequestBehavior.AllowGet);
            }

            if (user.email != item.email && !((ResultStatus)ValidateEmail(item.email).Data).result)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Girilen Email sistemde zaten mevcut!...")
                }, JsonRequestBehavior.AllowGet);
            }

            item.type = item.type ?? (int)EnumSH_UserType.Kullanıcı;
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            item.password = item.password != null ? db.GetMd5Hash(db.GetMd5Hash(item.password)) : item.password;

            var dbresult = db.UpdateSH_User(item);
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

            var item = id.Select(a => new SH_User { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_User(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public JsonResult ValidateLoginName(string loginname)
        {
            var db = new IntranetManagementDatabase();
            var res = db.GetSH_UserByLoginName(loginname);
            var jsonRes = new ResultStatus
            {
                result = (res == null),
                message = (res == null) ? "" : "Girilen kullanıcı adı sistemde zaten mevcut."
            };

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public JsonResult ValidateEmail(string email)
        {
            var db = new IntranetManagementDatabase();
            var res = db.GetSH_UserInfoByEmail(email);
            var jsonRes = new ResultStatus
            {
                result = (res == null),
                message = (res == null) ? "" : "Girilen Email adresi sistemde zaten mevcut."
            };

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public JsonResult ValidateTc(string tckimlikno)
        {
            var db = new IntranetManagementDatabase();

            if (System.Configuration.ConfigurationManager.AppSettings["MernisKontrol"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["MernisKontrol"] == "false")
                {
                    Json(new ResultStatus
                    {
                        result = true,
                        message = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            var res = db.GetSH_UserByTckimlikNo(tckimlikno);

            var jsonRes = new ResultStatus
            {
                result = (res == null),
                message = (res == null) ? "" : "Girilen  kimlik numarası sistemde zaten mevcut."
            };

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }



    }
}

