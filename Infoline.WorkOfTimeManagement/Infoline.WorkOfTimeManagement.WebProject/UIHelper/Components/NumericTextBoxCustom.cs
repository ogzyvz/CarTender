using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace System.Web.Mvc
{
    public static class NumericTextBoxCustom
    {
        public static NumericTextBoxBuilder<double> NumericTextBox(HtmlHelper helper)
        {
            return helper.Kendo().NumericTextBox().
                HtmlAttributes(new Dictionary<string, object>()
                {
                    {"style", "width:100%"},
                })
                .Format("n3")
                .Step(1)
                .Spinners(false)
                .Culture(Extensions.Culture().Name);
        }


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