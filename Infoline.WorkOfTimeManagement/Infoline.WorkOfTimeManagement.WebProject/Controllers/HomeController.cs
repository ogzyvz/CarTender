using Infoline.WorkOfTimeManagement.BusinessAccess;
using Infoline.WorkOfTimeManagement.BusinessData;
using Infoline.Web.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTimeManagement.WebProject.Controllers
{
    public class HomeController : Controller
    {

        [AllowEveryone]
        public ActionResult Index()
        {
            return View();
        }

        [AllowEveryone]
        public ActionResult Features()
        {
            return View();
        }
        [AllowEveryone]
        public ActionResult ScreenShots()
        {
            return View();
        }
        [AllowEveryone]
        public ActionResult AskToUs()
        {
            return View();
        }
        [AllowEveryone]
        public ActionResult Demo()
        {
            return View();
        }
        public ActionResult References()
        {
            return View();
        }

        public ActionResult LanguageInsert()
        {

            var db = new WorkOfTimeManagementDatabase();
            var model = new Dictionary<string, string>();
            var data = new LanguageSearch().GetAllPages()
                .Where(a => System.IO.File.Exists(Server.MapPath(String.Concat((string.IsNullOrEmpty(a.Area) ? "~" : "~/Areas/" + a.Area), "/Views/", a.Controller, "/", a.Action, ".cshtml"))))
                .Where(a => a.Attributes.IndexOf("HttpPost") == -1)
                .OrderBy(a => a.Area)
                .ToArray();

            Guid? id = null;
            foreach (var item in data)
            {
                try
                {
                    id = db.GetTableToId(item.Controller);
                }
                catch (Exception ex)
                {

                }

                model.Add("-" + item.Area + "-" + item.Controller + "-" + item.Action + ".js", (item.Area == "" ? "" : "/") + item.Area + "/" + item.Controller + "/" + item.Action + ((id.HasValue && (item.Action != "Index" || item.Action != "Insert")) ? "?id=" + id : ""));

            }

            return View(model);

        }

        [AllowEveryone, HttpPost]
        public JsonResult Save(string json, string fileName)
        {

            var filepath = Server.MapPath("~/Content/Custom/js/");

            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory(filepath);
            }

            var filedirectory = filepath + fileName;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filedirectory))
            {
                file.Write(json);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [AllowEveryone]
        public JsonResult ContactMailSend(ContactClass data)
        {

            var response = Request["g-recaptcha-response"];
            string secretKey = "6LcivcYUAAAAAF5xycwy0CQmsWsPjk03CfejG7W0";

#if DEBUG
            secretKey = "6LfT   vMYUAAAAAItF5EGxTDCDG8KsZklLmLWx_4Tx";
#endif
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            if (!status)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Lütfen güvenlik doğrulamasını yapınız.")
                }, JsonRequestBehavior.AllowGet);
            }


            var control = IsValid(data.EmailAddress);
            if (!control)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Email adresi yanlış")
                }, JsonRequestBehavior.AllowGet);
            }

            var mesaj = "<p><strong>Merhaba " + data.FullName + " tarafından mesaj gönderilmiştir.</strong></p>" +
                            "<p>Email : " + data.EmailAddress + "</p>" +
                            "<p>Kurum : " + data.Foundation + "</p>" +
                            "<p>Mesaj : " + data.Description + "</p>";

            var mesajUser = "<p><strong>Merhaba ,</strong></p>" +
                            "<p>WorkOfTime üzerinden göndermiş olduğunuz mesajınızı aldık.Konuyla ilgili en kısa sürede iletişime geçeceğiz.</p>" +
                            "<p>İyi Günler Dileriz.</p>";


            new Email().Template("Template1", "bos.png", "WorkOfTime İletişim Mesajı", mesaj)
            .Send("info@workoftime.com", string.Format("{0} | {1}", "WorkOfTime", "WorkOfTime İletişim Mesajı"), true);


            new Email().Template("Template1", "bos.png", "WorkOfTime İletişim Mesajınız Hakkında", mesajUser)
          .Send(data.EmailAddress, string.Format("{0} | {1}", "WorkOfTime", "WorkOfTime İletişim Mesajı"), true);

            return Json(new ResultStatusUI {
                Result = true,
                FeedBack = new FeedBack().Success("Mesaj gönderme işleminiz başarılı.")
            },JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public ActionResult Video()
        {
            return View();
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [AllowEveryone]
        [PageDescription("Kullanıcı Sözleşmesi", PageSecurityLevel.Dusuk)]
        public ActionResult UserAgreement()
        {
            return View();
        }

        [AllowEveryone]
        [PageDescription("Kişisel Verilerin Korunması ve İşlenmesi Politikası", PageSecurityLevel.Dusuk)]
        public ActionResult ProtectionOfPersonel()
        {
            return View();
        }


    }

    public class ContactClass
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Foundation { get; set; }
        public string Description { get; set; }
    }


}
