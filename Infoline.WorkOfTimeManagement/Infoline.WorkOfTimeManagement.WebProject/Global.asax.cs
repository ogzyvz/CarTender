using Infoline.WorkOfTimeManagement.BusinessAccess;
using Infoline.WorkOfTimeManagement.BusinessData;
using Infoline.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Infoline;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.WebProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(IGeometry), new UIGeographyBinder());
            ModelBinders.Binders.Add(typeof(double), new UIDoubleBinder());
            ModelBinders.Binders.Add(typeof(double?), new UIDoubleBinder());

            ModelBinders.Binders.Add(typeof(Guid), new UIGuidBinder());
            ModelBinders.Binders.Add(typeof(Guid?), new UIGuidBinder());
            ModelBinders.Binders.Add(typeof(Guid[]), new UIGuidBinder());
            ModelBinders.Binders.Add(typeof(Guid?[]), new UIGuidBinder());
            ModelBinders.Binders.Add(typeof(List<Guid>), new UIGuidBinder());
            ModelBinders.Binders.Add(typeof(List<Guid?>), new UIGuidBinder());

        }

        public bool CheckDebugStart()
        {
            var db = new IntranetManagementDatabase();
            var result = db.Login("infoline", "Se2105");

            if (result.LoginResult == LoginResult.OK)
            {
                HttpContext.Current.Session["ticketid"] = result.ticketid;
                HttpContext.Current.Session["userStatus"] = db.GetUserPageSecurityByticketid(result.ticketid);
                return true;
            }

            return false;

        }

        protected void Session_OnStart(object sender, EventArgs e)
        {
#if DEBUG
            if (CheckDebugStart())
                return;
#endif

        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["ticketid"] != null)
            {
                var db = new IntranetManagementDatabase();
                db.UpdateSH_Ticket(new SH_Ticket { id = new Guid(Session["ticketid"].ToString()), endtime = DateTime.Now });
                Session.Remove("userStatus");
                Session.Remove("ticketid");
                Session.Clear();
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();

            if (error != null)
            {
                var user = HttpContext.Current.Session != null && HttpContext.Current.Session["userStatus"] != null ? (PageSecurity)HttpContext.Current.Session["userStatus"] : new PageSecurity();
                var userid = user != null && user.user != null ? (Guid?)user.user.id : null;


                var httpException = error as HttpException;

                var code = (httpException == null ? 500 : (httpException.ErrorCode)).ToString();
                var mesage = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(httpException == null ? error.Message : httpException.Message, false);
                var trace = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(httpException == null ? error.StackTrace : httpException.StackTrace, false);

                if (HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var c = Infoline.Helper.Json.Serialize(new ResultStatusUI { Result = false, FeedBack = new FeedBack().Error(code + mesage + trace + System.Environment.NewLine + System.Environment.NewLine, error.Message) });
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    Response.Write(c);
                    Response.End();
                }
                else
                {
                    Log.Error(code + mesage + trace + System.Environment.NewLine + System.Environment.NewLine);
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session["ErrorCode"] = code;
                        HttpContext.Current.Session["ErrorMessage"] = mesage;
                        HttpContext.Current.Session["ErrorTrace"] = trace;
                    }

                    if (HttpContext.Current.Request.Url.OriginalString.IndexOf("/Error/InternalServer") < 0)
                    {
                        Response.Redirect("/Error/InternalServer");
                    }
                }

                Server.ClearError();
            }
        }

    }
}
