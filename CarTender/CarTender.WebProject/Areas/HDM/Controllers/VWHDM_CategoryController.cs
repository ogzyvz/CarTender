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

namespace CarTender.WebProject.Areas.HDM.Controllers
{
	public class VWHDM_CategoryController : Controller
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
			var db = new WorkOfTimeManagementDatabase();
			var data = db.GetVWHDM_Category(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWHDM_CategoryCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeManagementDatabase();
			var data = db.GetVWHDM_Category(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		public ActionResult Detail(VMHDM_CategoryModel data)
		{
			return View(data.Load());
		}

		public ActionResult Insert(VMHDM_CategoryModel data)
		{
			return View(data.Load());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMHDM_CategoryModel model, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = model.Save(Guid.Empty);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaydetme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Update(VMHDM_CategoryModel data)
		{
			return View(data.Load());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMHDM_CategoryModel data, bool? isPost)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = data.Save(Guid.Empty);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Kaza ve olay eğitimi güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Delete(string[] id) //VMHDM_CategoryModel data yazdıgımda idyi neden alamıyorum
		{
			//var db = new InfolineHRManagementDatabase();
			//var data = db.DeleteHDM_Category(Guid.Parse(id));
			//return null;
			//return Json(new ResultStatusUI(new VMHDM_CategoryModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
			var db = new WorkOfTimeManagementDatabase();
			var feedback = new FeedBack();
			var item = id.Select(a => new HDM_Category
			{
				id = new Guid(a)
			});
			var dbresult = db.BulkDeleteHDM_Category(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız")
			};
			return Json(result);

		}
	}
}
