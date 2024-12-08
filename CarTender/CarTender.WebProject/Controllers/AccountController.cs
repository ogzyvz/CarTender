using BotDetect.Web.Mvc;
using Infoline.Framework.Database;
using CarTender.BusinessAccess;
using CarTender.BusinessData;
using Infoline.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infoline.Framework.Helper;

namespace CarTender.WebProject.Controllers
{
    public class AccountController : Controller
    {
         
        public ActionResult Index()
        {
            return View();
        }

        [AllowEveryone]
        public ActionResult SignIn()
        {

            var user = new SH_User();
            var ticketid = Session["ticketid"];
            var userStatus = Session["userStatus"];
            var db = new WorkOfTimeManagementDatabase();

            if (ticketid != null && userStatus != null)
            {
                return RedirectToAction("Index", "Account");
            }


            if (Request.Cookies["logins"] != null && !string.IsNullOrEmpty(Request.Cookies["logins"].Value) && !string.IsNullOrEmpty(Request.Cookies["logins"]["Id"]))
            {

                user = db.GetSHUserById(new Guid(Request.Cookies["logins"]["Id"]));
                if (user != null)
                {

                    ViewBag.Check = "checked";
                }
                else
                    user = new SH_User();

            }

            ViewBag.Request = Request.QueryString["returnUrl"];
            return View(user);

        }


        [AllowEveryone]
        [HttpPost, CaptchaValidation("CaptchaCode", "UserControlCaptcha", "Hatalı Giriş")]
        public JsonResult SignIn(SH_User sh_user)
        {

            var Feedback = new FeedBack();

            if (!ModelState.IsValid)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = Feedback.Warning("Geçersiz güvenlik kodu")
                }, JsonRequestBehavior.AllowGet);
            }

            var db = new WorkOfTimeManagementDatabase();
            var ticketId = Session["ticketid"];
            var userStatus = Session["userStatus"];
            var username = sh_user.loginname.ToString().Trim();
            var password = sh_user.password.ToString().Trim();
            var login = db.Login(username, password);
            var check = Request["rememberMe"];


            if (login.LoginResult != Infoline.LoginResult.OK || login.ticketid == null)
            {
                var state = login.LoginResult.ToString();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = Feedback.Warning(state == "InvalidUser" ? "Geçersiz kullanıcı" : (state == "InvalidPassword" ? "Geçersiz şifre" : "Kullanıcı aktif değil"))
                }, JsonRequestBehavior.AllowGet);
            }

            Session["ticketid"] = login.ticketid;
            Session["userStatus"] = db.GetUserPageSecurityByticketid(login.ticketid);

            if (check != "false")
            {
                Response.Cookies["logins"]["Id"] = ((PageSecurity)Session["userStatus"]).user.id.ToString();
            }
            else
            {
                Response.Cookies["logins"].Value = string.Empty;
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = Feedback.Custom("Giriş işlemi başarılı.", (string.IsNullOrEmpty(Request["returnUrl"]) ? Url.Action("Index", "Account", new { area = String.Empty }) : Request["returnUrl"]), "Bilgilendirme", "success", 2, false)
            }, JsonRequestBehavior.AllowGet);
        }


        [AllowEveryone]
        public ActionResult SignOut()
        {
            if (Session["ticketid"] != null)
            {
                var db = new WorkOfTimeManagementDatabase();
                db.UpdateSH_Ticket(new SH_Ticket { id = new Guid(Session["ticketid"].ToString()), endtime = DateTime.Now });
                Session.Remove("ticketid");
                Session.Remove("userStatus");
                Session.Clear();
            }

            return RedirectToAction("Index", "Home", new { area = String.Empty });
        }


        [AllowEveryone]
        public ActionResult SignUp()
        {
            return View();
        }

        [AllowEveryone, HttpPost, ValidateAntiForgeryToken, CaptchaValidation("CaptchaCode2", "UserControlCaptcha2", "Hatalı Giriş")]
        public JsonResult SignUp(SH_User item)
        {
            var db = new WorkOfTimeManagementDatabase();
            var feedback = new FeedBack();
            MvcCaptcha.ResetCaptcha("UserControlCaptcha2");

            if (!ModelState.IsValid)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Geçersiz Güvenlik Kodu !")
                }, JsonRequestBehavior.AllowGet);
            }

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
            item.createdby = item.id;
            item.status = (bool)item.status;
            item.password = db.GetMd5Hash(db.GetMd5Hash(item.password));
            var dbresult = db.InsertSH_User(item);


            if (dbresult.result)
            {
                var mesajIcerigi = "Tebrikler! Sisteme kaydınız başarı ile gerçekleştirildi. Yönetici Onayınızdan sonra sisteme giriş yapabilirsiniz..";
                var messageText = EMail.MailTextTemp(item.firstname + " " + item.lastname, mesajIcerigi);
                EMail.Send(item.email, string.Format("{0} | {1}", "CarTender.WebProject", "Üyelik Bilgisi"), messageText, true);

                var adminler = db.GetSH_UserAllAdmins();
                foreach (var yonetici in adminler)
                {
                    var mesajIcerikYonetici = string.Format("Sisteme yeni kaydolan kullanıcılar var. Lütfen Onaylayınız. Kullanıcı Adı : {0} , E-Mail : {1}", item.loginname, item.email);
                    var messageTextYonetici = EMail.MailTextTemp(yonetici.firstname + " " + yonetici.lastname, mesajIcerikYonetici);
                    EMail.Send(yonetici.email, string.Format("{0} | {1}", "CarTender.WebProject", "Üyelik Bilgisi"), messageTextYonetici, true);
                }

            }


            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Custom("Kaydolma İşlemi Başarılı", Url.Action("SignIn", "Account", new { area = String.Empty }), "Bilgilendirme", "success", 5, false) : feedback.Custom("Kaydolma İşlemi Başarılı", null, "Bilgilendirme", "error", 5, false)

            }, JsonRequestBehavior.AllowGet);
        }

        [AllowEveryone]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowEveryone]
        [HttpPost]
        public JsonResult ForgotPassword(FormCollection form)
        {
            var loginName = form["FRPloginName"];
            var email = form["FRPemail"];


            var db = new WorkOfTimeManagementDatabase();
            var feedback = new FeedBack();

            var userm = db.GetSH_UserByLoginName(loginName);

            if (userm == null)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Böyle bir kullanıcı adı sistemde kayıtlı değil.") }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (userm.email.ToLower() != email.ToLower())
                {
                    return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Girdiğiniz Bilgiler Uyuşmuyor.") }, JsonRequestBehavior.AllowGet);
                }

                var password = RandomPassword();
                var md5Password = db.GetMd5Hash(db.GetMd5Hash(password));
                var res = db.UpdateSH_User(new SH_User
                {
                    id = userm.id,
                    password = md5Password,
                    status = userm.status
                });

                if (!res.result)
                {
                    //feedback.Error(res.message);
                    return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("HATA") }, JsonRequestBehavior.AllowGet);
                }

                if (userm != null)
                {
                    var mesajIcerigi = "Tebrikler! Geçici şifreniz başarı ile oluşturuldu. Kullanıcı Adınız : " + userm.loginname + "<br /> Yeni Şifreniz : " + password;
                    var messageText = EMail.MailTextTemp(userm.firstname + " " + userm.lastname, mesajIcerigi);
                    EMail.Send(userm.email, string.Format("{0} | {1}", "CarTender.WebProject", "Geçici Şifre Talebi"), messageText, true);
                }
            }

            var result = new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Custom("Geçici şifreniz mail adresinize gönderilmiştir.", Url.Action("SignIn", "Account", new { area = String.Empty }), "Bilgilendirme", "success", 5, false)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [AllowEveryone]
        public JsonResult ValidateLoginName(string loginname)
        {
            var db = new WorkOfTimeManagementDatabase();
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
            var db = new WorkOfTimeManagementDatabase();
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
            var db = new WorkOfTimeManagementDatabase();

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

        [AllowEveryone]
        public ActionResult Profile()
        {
            var db = new WorkOfTimeManagementDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            var kullanici = db.GetSHUserById(userStatus.user.id);
            CryptographyHelper decrypt = new CryptographyHelper();
            //kullanici.tckimlikno = decrypt.Decrypt(kullanici.tckimlikno);

            if (kullanici == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(kullanici);
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

        [AllowEveryone]
        public ActionResult Help()
        {
            return View();
        }
      
        public string Execute()
        {
            var searchPages = new SearchPages().Run();
            var searchEnums = new SearchEnums().Run();
            var roles = new SearchPages().CreateAndUpdateDeveloperRoles();
            return "işlem başarılı";
        }



    }
}