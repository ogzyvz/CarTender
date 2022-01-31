using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.Extension.Service
{
    [Export(typeof(ISmartHandler))]
    public partial class GeneralHandler : BaseSmartHandler
    {
        public GeneralHandler()
            : base("GeneralHandler")
        {

        }

        [HandleFunction("GetTime")]
        public void GetTime(HttpContext context)
        {
            RenderResponse(context, new { Time = DateTime.Now });
        }



    }
}
