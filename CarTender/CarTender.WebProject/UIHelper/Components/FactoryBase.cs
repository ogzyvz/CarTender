using System.IO;
using System.Web.UI;

namespace System.Web.Mvc
{
    public abstract class FactoryBase : IFactory, IScriptable
    {

        protected FactoryBase(ViewContext viewContext, ViewDataDictionary viewData = null)
        {
            this.ViewData = viewData;
            this.ViewContext = viewContext;
        }

        public ModelMetadata ModelMetadata { get; set; }

        public ViewContext ViewContext { get; set; }

        public ViewDataDictionary ViewData { get; set; }

        public void WriteInitializationScript(TextWriter writer)
        {
            throw new NotImplementedException();
        }

        protected virtual void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
}