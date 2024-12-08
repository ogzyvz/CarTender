using CarTender.BusinessAccess;
using CarTender.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CarTender.WebProject.Areas.TEN.Controllers
{

    public class TEN_TenantController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new CarTenderDatabase();
            var data = db.GetTEN_Tenant(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetTEN_TenantCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new CarTenderDatabase();
            var data = db.GetTEN_Tenant(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        public ActionResult Detail(Guid id)
        {
            var db = new CarTenderDatabase();
            var data = db.GetTEN_TenantById(id);
            return View(data);
        }


        public ActionResult Insert(bool? IsPOC)
        {

            var data = new TEN_Tenant
            {
                id = Guid.NewGuid(),
                TenantCode = 1100,
                TenantIsPOC = IsPOC ?? false,
                DBIp = "10.100.0.223",
                DBCatalog = "WorkOfTime1100",
                DBPort = 1433,
                DBUser = "WOT...",
                DBPassword = "WOT...",
                DBType = Convert.ToInt32(Infoline.Framework.Database.DatabaseType.Mssql),
                WebDomain = "http://alanadi.workoftime.com",
                ServiceDomain = "http://alanadiservice.workoftime.com",
                TenantStartDate = DateTime.Now,
                TenantEndDate = DateTime.Now.AddYears(1),
                MailHost = "smtp.office365.com",
                MailPort = 587,
                MailSSL = true,
                MailUser = "noreply@workoftime.com",
                MailPassword = "Paz01220"
            };
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(TEN_Tenant item)
        {
            var db = new CarTenderDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var res = new FileUploadSave(Request).SaveAs();

            if (((SysFileReturn[])res.Object).Count() > 0)
            {
                foreach (var list in ((SysFileReturn[])res.Object).ToList())
                {
                    var bytes = System.IO.File.ReadAllBytes(Server.MapPath(list.url));
                    var base64 = Convert.ToBase64String(bytes);

                    if (list.fileGroup == "Logo1")
                    {
                        item.Logo = "data:image/"+ list.extension +";base64," +  base64;
                    }
                    else if(list.fileGroup == "Logo2")
                    {
                        item.LogoOther = "data:image/" + list.extension + ";base64," + base64;
                    }
                    else
                    {
                        item.Favicon = "data:image/x-icon;base64," + base64;
                    }
                }
            }

            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            item.DBType = Convert.ToInt32(Infoline.Framework.Database.DatabaseType.Mssql);
            var dbresult = db.InsertTEN_Tenant(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update(Guid id)
        {
            var db = new CarTenderDatabase();
            var data = db.GetTEN_TenantById(id);
            data.MailSSL = data.MailSSL ?? true;
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(TEN_Tenant item)
        {
            var db = new CarTenderDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var res = new FileUploadSave(Request).SaveAs();

            if (((SysFileReturn[])res.Object).Count() > 0)
            {
                foreach (var list in ((SysFileReturn[])res.Object).ToList())
                {
                    var bytes = System.IO.File.ReadAllBytes(Server.MapPath(list.url));
                    var base64 = Convert.ToBase64String(bytes);

                    if (list.fileGroup == "Logo1")
                    {
                        item.Logo = "data:image/" + list.extension + ";base64," + base64;
                    }
                    else if (list.fileGroup == "Logo2")
                    {
                        item.LogoOther = "data:image/" + list.extension + ";base64," + base64;
                    }
                    else
                    {
                        item.Favicon = "data:image/x-icon;base64," + base64;
                    }
                }
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            item.DBType = Convert.ToInt32(Infoline.Framework.Database.DatabaseType.Mssql);

            var dbresult = db.UpdateTEN_Tenant(item, true);
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
            var db = new CarTenderDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new TEN_Tenant { id = new Guid(a) });

            var dbresult = db.BulkDeleteTEN_Tenant(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}
