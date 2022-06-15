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
        //Demo Talebinde captcha ayarı
        //Demo Talep json result
        //Soru kısmı 
        //screen shot görüntülerin arka planı

        [AllowEveryone]
        public ActionResult Index()
        {

            return View();
        }

        [AllowEveryone]
        public ActionResult Feautures()
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
            var db = new WorkOfTimeManagementDatabase();
            var categories = db.GetVWHDM_Category().Reverse().ToArray();
            var faqs = db.GetVWHDM_Faq();
            var quest = new VWHDM_Question { id = Guid.NewGuid() };

            //var a = CategoriSırala();

            var data = new CategoryListModel
            {
                categories = categories,
                faqs = faqs,
            };
            ViewBag.faqId = new SelectList(faqs, "id", "name");
            

            return View(data);

        }
        
        [AllowEveryone,HttpPost]
        public ActionResult AskToUs(VWHDM_Question data)
        {
            var db = new WorkOfTimeManagementDatabase();
            data.created = DateTime.Now;

            if (data.faqId_Title == null)
            {
                data.faqId_Title = "Soru Seçilmemiştir";
            }

            if (data.faqId != null)
            {
                var faqName = db.GetHDM_FaqById((Guid)data.faqId).name;
                data.faqId_Title = faqName;
            }

            var entitiy = new HDM_Question()
            {
                content = data.content,
                companyName = data.companyName,
                email = data.email,
                faqId = data.faqId,
                fullName = data.fullName,
                phone = data.phone,
                userId = data.userId,
                id = data.id
            };


            var response = Request["g-recaptcha-response"];
            string secretKey = "6LfZ8IUbAAAAALzYo9O1EnbY__jI-x9U_sjliIw8";



            //  Local Secret Key => 6LfZ8IUbAAAAALzYo9O1EnbY__jI-x9U_sjliIw8    

            // Corporate Secret Key => 6LcCq2wgAAAAAMjJOXBHU2H3BUgBm8MIOXX_LI5F

            // Canlı Secret Key => 


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

            if (string.IsNullOrEmpty(data.fullName) && string.IsNullOrEmpty(data.companyName) && string.IsNullOrEmpty(data.content))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Lütfen Boş alan Bırakmayınız")
                }, JsonRequestBehavior.AllowGet);
            }
            
            if (!IsValid(data.email))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Email adresi yanlış")
                }, JsonRequestBehavior.AllowGet);
            }

            if (!IsValidPhone(data.phone.ToString()))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Telefon numaranız yanlış (0555 555 55 55)")
                }, JsonRequestBehavior.AllowGet);
            }


            db.InsertHDM_Question(entitiy);

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = new FeedBack().Success("Soru talebiniz başarıyla gerçekleşti.", false, Url.Action("AskToUs", "Home", string.Empty))
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public ActionResult Demo()
        {
            return View();
        }
        [AllowEveryone]
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


        [AllowEveryone,HttpPost, ValidateAntiForgeryToken]
        public JsonResult DemoMailSend(ContactClass data)
        {

            var response = Request["g-recaptcha-response"];
            string secretKey = "6LfZ8IUbAAAAALzYo9O1EnbY__jI-x9U_sjliIw8";


            // Site Anahtarı => 6LfZ8IUbAAAAALAtrjJt_e4Q93nzLEDHUh6o1rYS

            //  Secret Key => 6LfZ8IUbAAAAALzYo9O1EnbY__jI-x9U_sjliIw8    

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


            if (string.IsNullOrEmpty(data.username) && string.IsNullOrEmpty(data.company) && string.IsNullOrEmpty(data.message))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Lütfen Boş alan Bırakmayınız")
                }, JsonRequestBehavior.AllowGet);
            }
            if (!IsValid(data.email))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Email adresi yanlış")
                }, JsonRequestBehavior.AllowGet);
            }
            if (!IsValidPhone(data.phone))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Telefon numaranız yanlış (0555 555 55 55)")
                }, JsonRequestBehavior.AllowGet);
            }

            var mesaj = "<p><strong>Merhaba " + data.username + " tarafından mesaj gönderilmiştir.</strong></p>" +
                            "<p>Email : " + data.email + "</p>" +
                            "<p>Kurum : " + data.company + "</p>" +
                            "<p>Mesaj : " + data.message + "</p>";

            var mesajUser = "<p><strong>Merhaba ,</strong></p>" +
                            "<p>WorkOfTime üzerinden göndermiş olduğunuz mesajınızı aldık.Konuyla ilgili en kısa sürede iletişime geçeceğiz.</p>" +
                            "<p>İyi Günler Dileriz.</p>";


            new Email().Template("Template1", "bos.png", "WorkOfTime İletişim Mesajı", mesaj)
            .Send("info@workoftime.com", string.Format("{0} | {1}", "WorkOfTime", "WorkOfTime İletişim Mesajı"), true);


            new Email().Template("Template1", "bos.png", "WorkOfTime İletişim Mesajınız Hakkında", mesajUser)
          .Send(data.email, string.Format("{0} | {1}", "WorkOfTime", "WorkOfTime İletişim Mesajı"), true);

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = new FeedBack().Success("Demo talebiniz başarıyla gerçekleşti.", false, Url.Action("Demo", "Home", string.Empty))
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public ActionResult Video()
        {
            return View();
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
        
        [AllowEveryone]
        public ActionResult PrivacyPolicy()
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

        public bool IsValidPhone(string phone)
        {
            if (phone == null)
            {
                return false;
            }
            try
            {
                phone = phone.Replace(" ", "");
                phone = phone.Replace("(", "");
                phone = phone.Replace(")", "");

                var result = Convert.ToUInt64(phone);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class ContactClass
    {
        public string username { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string phone { get; set; }
        public string message { get; set; }
    }

    public class CategoryListModel
    {
        public VWHDM_Category[] categories { get; set; }
        public VWHDM_Faq[] faqs { get; set; }
    }
}
