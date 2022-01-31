using System.Web.Mvc;

namespace Infoline.WorkOfTimeManagement.WebProject.Areas.TEN
{
    public class TENAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TEN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TEN_default",
                "TEN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}