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
	public class VWHDM_QuestionController : Controller
	{
		public ActionResult Index()
		{
			var db = new WorkOfTimeManagementDatabase();
			ViewBag.toplam = db.GetAllQuestionCount();
			ViewBag.cevaplanmis = db.GetAnsweredQuestion();
			ViewBag.cevaplanmamis = db.GetNotAnsweredQuestion();
			ViewBag.question = db.GetLastQuestion();
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
			var data = db.GetVWHDM_Question(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWHDM_QuestionCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeManagementDatabase();
			var data = db.GetVWHDM_Question(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeManagementDatabase();
			var data = db.GetVWHDM_QuestionById(id);
			return View(data);
		}

		public ActionResult Insert()
		{
			var data = new VWHDM_Question { id = Guid.NewGuid() };
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HDM_Question item)
		{
			var db = new WorkOfTimeManagementDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.created = DateTime.Now;
			item.createdby = Guid.Empty;
			var dbresult = db.InsertHDM_Question(item);
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
			var data = db.GetVWHDM_QuestionById(id);
			return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(HDM_Question item)
		{
			var db = new WorkOfTimeManagementDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;

			var dbresult = db.UpdateHDM_Question(item);
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

			var item = id.Select(a => new HDM_Question { id = new Guid(a) });

			var dbresult = db.BulkDeleteHDM_Question(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}
