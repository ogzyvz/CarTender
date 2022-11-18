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
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;

namespace Infoline.WorkOfTimeManagement.WebProject.Areas.TEN.Controllers
{
	public class VWTEN_DemonstrationRequestController : Controller
	{
		public ActionResult Index()
		{
		    return View();
		}
        public ActionResult DemoRequests()
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
		    var db = new WorkOfTimeManagementDatabase();
		    var data = db.GetVWTEN_DemonstrationRequest(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWTEN_DemonstrationRequestCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeManagementDatabase();
		    var data = db.GetVWTEN_DemonstrationRequest(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}


		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeManagementDatabase();
		    var data = db.GetVWTEN_DemonstrationRequestById(id);
		    return View(data);
		}

        [AllowEveryone]
		public ActionResult Insert()
		{
		    var data = new VWTEN_DemonstrationRequest { id = Guid.NewGuid() };
		    return View(data);
		}

        [AllowEveryone]
        private string RandomPassword()
        {
            var random = new Random();
            string randomValue = "12345647890";
            string result = "";
            for (var i = 0; i < 10; i++)
            {
                result += randomValue[random.Next(randomValue.Length)];
            }
            return result;
        }


        [AllowEveryone, HttpPost, PageDescription("Demo Talebi Ekleme Methodu", PageSecurityLevel.Dusuk)]
        public JsonResult Insert(TEN_DemonstrationRequest item)
		{
            var feedback = new FeedBack();

            var response = Request["g-recaptcha-response"];
            string secretKey = "6LfZ8IUbAAAAALzYo9O1EnbY__jI-x9U_sjliIw8";


            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            if (!status)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen güvenlik doğrulamasını yapınız.")
                }, JsonRequestBehavior.AllowGet);
            }


            var baseDb = new WorkOfTimeManagementDatabase(); 
            var ConnectionString = "Data Source=.;Initial Catalog=WorkOfTime1199;User ID=sa;Password=c2SSsCM9Y3HZVU";

#if DEBUG
            ConnectionString = "Data Source=10.100.0.222;Initial Catalog=WorkOfTime1199;User ID=sa;Password=c2SSsCM9Y3HZVU";
#endif
            var db = new WorkOfTimeManagementDatabase(ConnectionString);
            var userMail = baseDb.GetTEN_DemonstrationRequestByMail(item.EMail);
			if (userMail!=null)
			{
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı e-mail adresi ile daha önce kayıt gerçekleştirilmiş.")
                }, JsonRequestBehavior.AllowGet);
            }

            var mail = WebConfigurationManager.AppSettings["demoMail"];
            var password = WebConfigurationManager.AppSettings["demoPassword"];
            
            var res = baseDb.InsertTEN_DemonstrationRequest(item);

            if (res.result == false)
            {
              
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kayıt işlemi gerçekleşirken hata oluştu.")
                }, JsonRequestBehavior.AllowGet);
            }


            string url = "http://demo.workoftime.com";
            var text = "<h3>Merhaba {0},</h3>";
            text += "<p><u>WorkOfTime </u> için demo talebi oluşturdunuz.</p>";
            text += "Demo panelimize giriş yapmak için lütfen <a href='{1}/Account/SignIn'>Buraya tıklayınız! </a>";
            text += "<p> Kullanıcı Adınız : {2}</ p >";
            text += "<p> Şifreniz : {3}</ p >";
            text += "<p> NOT : Hesabınız 15 gün aktif kalacaktır.</ p >";
            text += "<p>Demo panelimizde bizim tanımladığımız demo veriler bulunmaktadır. İstediğiniz gibi veri girişleri yapabilirsiniz. WorkOfTime’ı kullanırken karşılaşabileceğiniz sorunlarla ilgili mail veya telefon aracılığıyla bizden destek alabilirsiniz. Sizden gelecek tüm iyi ve kötü eleştirilere açık olduğumuzu belirtir iyi kullanımlar dileriz. </ p >";
            text += "<p> Saygılarımızla. </p>";
            text += "<p> INFOLINE Bilgi Teknolojileri. </p>";
            var mesaj = string.Format(text, item.Name + " " + item.Surname, url, mail, password);
            new Email().Template("Template1", "demo.jpg", "Demo Talebi Hakkında", mesaj)
              .Send(item.EMail, string.Format("{0} | {1}", "INFOLINE WorkOfTime", "Demo Talebi Hakkında.."), true);

            var ourText = "<h3>Merhaba,</h3>";
            ourText += "<p><u>WorkOfTime</u> için demo talebi oluşturuldu.</p>";
            ourText += "<p> Adı Soyadı : {0} {1}</ p >";
            ourText += "<p> Telefon Numarası : {2}</ p >";
            ourText += "<p> E-mail Adresi : {3}</ p >";
            ourText += "<p> Şirket Adı : {4}</ p >";
            ourText += "<p> Bilgilerinize. </p>";

            var mesajOur = string.Format(ourText, item.Name, item.Surname, item.Phone, item.EMail, item.CompanyName);

            string[] cc = WebConfigurationManager.AppSettings["personelMailList"].Split(',');

            new Email().Template("Template1", "demo.jpg", "Demo Talebi Hakkında", mesajOur)
             .Send("senol.elik@infoline-tr.com", string.Format("{0} | {1}", "WorkOfTime", "Demo Talebi Hakkında.."), true, null, null, cc);


            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Kaydınız başarı ile alınmıştır.En kısa sürede tarafınıza dönüş yapılacaktır.",true)
            }, JsonRequestBehavior.AllowGet);

        }


		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeManagementDatabase();
		    var data = db.GetVWTEN_DemonstrationRequestById(id);
		    return View(data);
		}


		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(TEN_DemonstrationRequest item)
		{
		    var db = new WorkOfTimeManagementDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateTEN_DemonstrationRequest(item);
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
		
		    var item = id.Select(a => new TEN_DemonstrationRequest { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteTEN_DemonstrationRequest(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}


    
    }
}
