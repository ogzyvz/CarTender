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

namespace CarTender.WebProject.Areas.HDM.Controllers
{
	public class VWHDM_FaqController : Controller
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
			var data = db.GetVWHDM_Faq(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWHDM_FaqCount(condition.Filter);


			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeManagementDatabase();
			var data = db.GetVWHDM_Faq(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		public ActionResult Detail(VMHDM_FaqModel data)
		{
			return View(data.Load());
		}

		public ActionResult Insert(VMHDM_FaqModel data)
		{
			return View(data.Load());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMHDM_FaqModel item, bool? a)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = item.Save(Guid.Empty);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Başarısız Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);

		}

		public ActionResult Update(/*Guid id*/VMHDM_FaqModel data)
		{
			//    var db = new InfolineHRManagementDatabase();
			//    var data = db.GetVWHDM_FaqById(id);

			return View(data.Load());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMHDM_FaqModel data, bool? a)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var rs = data.Save(Guid.Empty);
			return Json(new ResultStatusUI
			{
				Result = rs.result,
				FeedBack = rs.result ? feedback.Success(rs.message) : feedback.Warning("Güncelleme işlemi başarısız. Mesaj : " + rs.message)
			}, JsonRequestBehavior.AllowGet);

		}

		[HttpPost]
		public JsonResult Delete(string[] id)
		{
			var db = new WorkOfTimeManagementDatabase();
			var feedback = new FeedBack();

			var item = id.Select(a => new HDM_Faq { id = new Guid(a) });

			var dbresult = db.BulkDeleteHDM_Faq(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

	}
}
