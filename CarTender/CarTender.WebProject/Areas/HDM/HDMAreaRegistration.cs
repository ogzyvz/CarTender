using System.Web.Mvc;

namespace CarTender.WebProject.Areas.HDM
{
    public class HDMAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HDM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HDM_default",
                "HDM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}