using Infoline.Web.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarTender.WebProject.Controllers
{
    public class ErrorController : Controller
    {

        [AllowEveryone]
        public ActionResult UnkownPage()
        {
            return View();
        }

        [AllowEveryone]
        public ActionResult InternalServer()
        {
            return View();
        }

    }
}
