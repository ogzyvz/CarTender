using Infoline;
using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.Web.SmartHandlers;
using CarTender.BusinessAccess;
using CarTender.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace CarTender.Business.Security
{
    [Export(typeof(ISmartHandler))]
    public class SecurityHandler : BaseSmartHandler
    {
        public SecurityHandler()
            : base("security", new string[] { "cmd" })
        {

        }

        public override void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {
            var _start = DateTime.Now;
            var cmd = paramters["cmd"].ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);

            var func = typeof(SecurityHandler).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .FirstOrDefault(a => a.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture) == cmd && a.GetParameters().Length == 1);
            if (func != null)
            {
                func.Invoke(this, new[] { context });
            }
        }

        void Logout(HttpContext context)
        {
            if (CallContext.IsReady)
            {
                CallContext.Current.Logout();
                RenderResponse(context, new ResultStatus { message = "Successfully", result = true, objects = null });
            }
            else
            {
                RenderResponse(context, new ResultStatus { message = "User Is Not Found", result = false, objects = null });
            }
        }

        void Login(HttpContext context)
        {
            var _start = DateTime.Now;
            if (!(CallContext.IsReady))
            {
                if (HttpContext.Current.Request.HttpMethod == "POST")
                {
                    var loginpost = ParseRequest<Login>(context);

                    if (loginpost != null)
                    {
                        var userName = new CryptographyHelper().Decrypt(loginpost.username);
                        var passwordBeforeSplit = new CryptographyHelper().Decrypt(loginpost.password).Split('_');
                        var password = passwordBeforeSplit[0];
                        var deviceid = loginpost.deviceId.HasValue ? loginpost.deviceId.Value : Guid.Empty;
                        var clientTime = Convert.ToDateTime(passwordBeforeSplit[1]);
                        if (clientTime.Day == DateTime.Now.Day)
                        {

                            var result = Application.Current.SecurityService.Login(userName, password, deviceid, loginpost.IPAddress);
                            ResultStatus status;
                            if (result == Infoline.LoginResult.OK)
                            {
                                status = new ResultStatus
                                {
                                    result = true,
                                    objects = new ResultLogin
                                    {
                                        TicketId = CallContext.Current.TicketId
                                    },
                                    message = "TicketId is Lives"
                                };
                            }
                            else
                            {
                                var tResult = String.Empty;

                                if (result == Infoline.LoginResult.InvalidPassword)
                                    tResult = "Şifreniz kullanıcı adınız ile eşleşmemektedir, Lütfen bilgilerinizi kontrol ediniz.";
                                else if (result == Infoline.LoginResult.InvalidUser)
                                    tResult = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                                else if (result == Infoline.LoginResult.InvalidUser)
                                    tResult = "Hesabınız kapatılmıştır. İletişim üzerinden iletişime geçiniz.";
                                else if (result == Infoline.LoginResult.RequiresPasswordChage)
                                    tResult = "Şifre değişikliğine ihtiyaç vardır. Daha sonra tekrar deneyiniz.";
                                else
                                    tResult = result.ToString();

                                status = new ResultStatus { result = false, message = tResult };
                            }
                            RenderResponse(context, status);
                        }
                        else
                        {
                            context.Response.StatusCode = 401;
                            context.Response.End();
                        }
                    }
                    else
                    {
                        RenderResponse(context, new ResultStatus { result = false, objects = null, message = "Invalid username and password" });
                    }
                }
                else
                {
                    RenderResponse(context, new ResultStatus { result = false, objects = null, message = "Method is not POST" });
                }
            }
            else
            {
                RenderResponse(context, new ResultStatus
                {
                    result = true,
                    objects = new ResultLogin
                    {
                        TicketId = CallContext.Current.TicketId
                    },
                    message = "TicketId is Lives"
                });
            }
        }
        void WhoAmI(HttpContext context)
        {
            try
            {
                var ticketid = context.Request.Params["ticketid"];
                if (ticketid != null)
                {
                    var db = new WorkOfTimeManagementDatabase();
                    var user = db.GetSH_UserByTicketid(new Guid(ticketid));
                    RenderResponse(context, user);
                }
                else
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "ticketid is null", objects = null });
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message, objects = null });
            }
        }
    }
}

public class Login
{
    public string username { get; set; }
    public string password { get; set; }
    public Guid? deviceId { get; set; }
    public string IPAddress { get; set; }
}

public class ResultLogin
{
    public Guid TicketId { get; set; }
}

