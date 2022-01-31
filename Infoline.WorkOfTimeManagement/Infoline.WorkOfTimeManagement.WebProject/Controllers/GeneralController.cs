using Infoline.WorkOfTimeManagement.BusinessAccess;
using Infoline.Helper;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTimeManagement.WebProject.Controllers
{
    [AllowEveryone]
    public class GeneralController : Controller
    {

        public JsonResult GetEnums([DataSourceRequest]DataSourceRequest request)
        {
            var db = new IntranetManagementDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetSYS_Enums(condition).Select(a => new { Id = a.enumKey, enumDescription = a.enumDescription });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSH_UserStatus()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumSH_UserStatus>().OrderBy(a => a.Value).Distinct().Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSH_UserType()
        {
            var result = EnumsProperties.EnumToArrayValues<EnumSH_UserType>().Distinct().OrderBy(a => a.Value).Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUT_City()
        {
            var db = new IntranetManagementDatabase();
            var result = db.GetUT_CityCustom();

            return Json(result.Select(a => new { Id = a.id, Name = a.NAME, CityNumber = a.CityNumber }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUT_TownByCityNumber(int CityNumber, string filtre)
        {
            var db = new IntranetManagementDatabase();
            var result = db.GetUT_TownCustomByCityNumberName(CityNumber, filtre);
            return Json(result.Select(a => new { Id = a.id, Name = a.NAME }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSH_UserByStatus(EnumSH_UserStatus status)
        {
            var db = new IntranetManagementDatabase();
            var result = db.GetSH_UserByStatus(status).Select(a => new { Id = a.id, Name = a.firstname + " " + a.lastname + " (" + a.loginname + ")" });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSH_Role()
        {
            var db = new IntranetManagementDatabase();
            var result = db.GetSH_Role().Select(a => new { Id = a.id, Name = a.rolname });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSH_Pages()
        {
            var db = new IntranetManagementDatabase();
            var result = db.GetSH_Pages().Select(a => new { Id = a.id, Name = a.Action });
            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}