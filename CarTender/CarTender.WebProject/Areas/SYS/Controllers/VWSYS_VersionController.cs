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
using Infoline.Framework.Database;

namespace CarTender.WebProject.Areas.SYS.Controllers
{
	public class VWSYS_VersionController : Controller
	{
		public ActionResult Index()
		{
		    return View();
		}


		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new CarTenderDatabase();
		    var data = db.GetVWSYS_Version(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWSYS_VersionCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new CarTenderDatabase();
		    var data = db.GetVWSYS_Version(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		public ActionResult Detail(Guid id)
		{
		    var db = new CarTenderDatabase();
		    var data = db.GetVWSYS_VersionById(id);
		    return View(data);
		}


		public ActionResult Insert()
		{
		    var data = new VWSYS_Version { id = Guid.NewGuid() };
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(SYS_Version item)
		{
		    var db = new CarTenderDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			if (string.IsNullOrEmpty(item.versionNumber) || string.IsNullOrEmpty(item.versionChange))
            {
				return Json(new ResultStatusUI
				{
					Result = false,
					Object = item.id,
					FeedBack = feedback.Warning("Zorunlu Alanları Doldurunuz.")
				}, JsonRequestBehavior.AllowGet);

            }
			var existVersionNumber = db.GetVWSYS_VersionExistByVersionNumber(item.versionNumber);
			if (existVersionNumber != null && existVersionNumber.id != item.id && existVersionNumber.versionNumber != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					Object = item.id,
					FeedBack = feedback.Warning(item.versionNumber + " versiyon numarası zaten sistemde kayıtlı, aynı versiyon numarasına ait kayıt oluşturamazsınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;	
		    var dbresult = db.InsertSYS_Version(item);
			if(item.isActive == true)
            {
				var oldversions = db.GetSYS_Version().Where(a => a.id != item.id).ToList();
				if (oldversions.Count > 0)
				{
					foreach (var oldversion in oldversions)
					{
						oldversion.isActive = false;
					}

					foreach (var items in oldversions.ToList())
					{
						db.UpdateSYS_Version(items);
						oldversions.Remove(items);
					}
				}
			}

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
		    var data = db.GetVWSYS_VersionById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(SYS_Version item)
		{
		    var db = new CarTenderDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			if (string.IsNullOrEmpty(item.versionNumber) || string.IsNullOrEmpty(item.versionChange))
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					Object = item.id,
					FeedBack = feedback.Warning("Zorunlu Alanları Doldurunuz.")
				}, JsonRequestBehavior.AllowGet);

			}
			var existVersionNumber = db.GetVWSYS_VersionExistByVersionNumber(item.versionNumber);
			if (existVersionNumber != null && existVersionNumber.id != item.id && existVersionNumber.versionNumber != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					Object = item.id,
					FeedBack = feedback.Warning(item.versionNumber + " versiyon numarası zaten sistemde kayıtlı, aynı versiyon numarasına ait kayıt oluşturamazsınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;		
		    var dbresult = db.UpdateSYS_Version(item);

			if (item.isActive == true)
			{
				var oldversions = db.GetSYS_Version().Where(a => a.id != item.id).ToList();
				if (oldversions.Count > 0)
				{
					foreach (var oldversion in oldversions)
					{
						oldversion.isActive = false;
					}

					foreach (var items in oldversions.ToList())
					{
						db.UpdateSYS_Version(items);
						oldversions.Remove(items);
					}
				}
			}
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
		
		    var item = id.Select(a => new SYS_Version { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteSYS_Version(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
