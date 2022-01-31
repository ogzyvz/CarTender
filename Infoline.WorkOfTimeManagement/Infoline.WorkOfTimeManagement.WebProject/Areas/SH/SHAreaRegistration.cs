using System.Web.Mvc;

namespace Infoline.WorkOfTimeManagement.WebProject.Areas.SH
{
    public class SHAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SH";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SH_default",
                "SH/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
