using System.Web.Mvc;

namespace CarTender.WebProject.Areas.TEN
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