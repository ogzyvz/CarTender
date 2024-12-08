using System.Collections.Generic;

namespace System.Web.Mvc
{
    public interface IHtmlAttributes
    {
        IDictionary<string, object> HtmlAttributes { get; }
    }
}