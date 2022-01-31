using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using System.Linq;

namespace System.Web.Mvc
{
    public static class DatePickerCustom
    {
        public static DatePickerBuilder DatePicker(HtmlHelper helper)
        {

            var kendo = helper
                .Kendo()
                .DatePicker()
                .ParseFormats(new string[] { "dd.MM.yyyy", "dd/MM/yyyy" }).
                HtmlAttributes(new Dictionary<string, object>()
                {
                    {"class", "form-control"},
                }).Format(Extensions.DateFormatShort());
            return kendo;
        }
        public static DateTimePickerBuilder DateTimePicker(HtmlHelper helper)
        {

            var kendo = helper.Kendo()
                .DateTimePicker()
                .ParseFormats(new string[] { "dd.MM.yyyy HH:mm", "dd/MM/yyyy HH:mm" })
                .HtmlAttributes(new Dictionary<string, object>()
                {
                    {"class", "form-control"},
                }).Format(Extensions.DateFormatFull());
            return kendo;
        }
    }
}