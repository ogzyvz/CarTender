using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI.Fluent;

namespace Kendo.Mvc.UI
{
    public static class NumericTextBoxCustom
    {
        public static NumericTextBoxBuilder<double> Placeholder(this NumericTextBoxBuilder<double> builder, string placeholder)
        {
            var temlHtml = new Dictionary<string, object>() { { "placeholder", placeholder } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static NumericTextBoxBuilder<double> MinElement(this NumericTextBoxBuilder<double> builder, string DataPickerId)
        {
            var temlHtml = new Dictionary<string, object>() { { "data-numerictextbox", DataPickerId } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);

            return builder;
        }
        public static NumericTextBoxBuilder<double> MaxElement(this NumericTextBoxBuilder<double> builder, string DataPickerId)
        {
            var temlHtml = new Dictionary<string, object>()
            {
                {"data-cascadefrom", DataPickerId},
                {"data-cascadetype", "max"}
            };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static NumericTextBoxBuilder<double> Id(this NumericTextBoxBuilder<double> builder, string Id)
        {
            var temlHtml = new Dictionary<string, object>() { { "id", Id } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
    }
}