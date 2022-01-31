using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI.Fluent;

namespace Kendo.Mvc.UI
{

    public static class DatePickerBuilderExtension
    {
        public static DatePickerBuilder MinDateTime(this DatePickerBuilder builder, DateTime minDateTime)
        {
            return builder.Min(new DateTime(minDateTime.Year, minDateTime.Month, minDateTime.Day, minDateTime.Hour, 00, 00));
        }
        public static DatePickerBuilder MaxDateTime(this DatePickerBuilder builder, DateTime maxDateTime)
        {
            return builder.Max(new DateTime(maxDateTime.Year, maxDateTime.Month, maxDateTime.Day, maxDateTime.Hour, 00, 00));
        }
        public static DatePickerBuilder Placeholder(this DatePickerBuilder builder, string placeholder)
        {
            var temlHtml = new Dictionary<string, object>() { { "placeholder", placeholder } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DatePickerBuilder MinDateElement(this DatePickerBuilder builder, string DataPickerId)
        {
            var temlHtml = new Dictionary<string, object>() {
                { "data-cascadefrom", DataPickerId },
                { "data-cascadetype", "min" }
            };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DatePickerBuilder MaxDateElement(this DatePickerBuilder builder, string DataPickerId)
        {
            var temlHtml = new Dictionary<string, object>() {
              { "data-cascadefrom", DataPickerId },
              { "data-cascadetype", "max" } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DatePickerBuilder Id(this DatePickerBuilder builder, string Id)
        {
            var temlHtml = new Dictionary<string, object>() { { "id", Id } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
    }

    public static class DateTimePickerBuilderExtension
    {
        public static DateTimePickerBuilder MinDateTime(this DateTimePickerBuilder builder, DateTime minDateTime)
        {
            var minute = (int)Math.Round((decimal)(minDateTime.Minute / 10)) * 10;
            return builder.Min(new DateTime(minDateTime.Year, minDateTime.Month, minDateTime.Day, minDateTime.Hour, minute, minDateTime.Second));
        }
        public static DateTimePickerBuilder MaxDateTime(this DateTimePickerBuilder builder, DateTime maxDateTime)
        {
            var minute = (int)Math.Round((decimal)(maxDateTime.Minute / 10)) * 10;
            return builder.Max(new DateTime(maxDateTime.Year, maxDateTime.Month, maxDateTime.Day, maxDateTime.Hour, minute, maxDateTime.Second));
        }
        public static DateTimePickerBuilder Placeholder(this DateTimePickerBuilder builder, string placeholder)
        {
            var temlHtml = new Dictionary<string, object>() { { "placeholder", placeholder } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DateTimePickerBuilder MinDateElement(this DateTimePickerBuilder builder, string DataPickerId)
        {
            var temlHtml = new Dictionary<string, object>() {
                { "data-cascadefrom", DataPickerId },
                { "data-cascadetype", "min" }
            };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DateTimePickerBuilder MaxDateElement(this DateTimePickerBuilder builder, string DataPickerId)
        {
            var temlHtml = new Dictionary<string, object>() {
                { "data-cascadefrom", DataPickerId },
                { "data-cascadetype", "max" }
            };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DateTimePickerBuilder Id(this DateTimePickerBuilder builder, string Id)
        {
            var temlHtml = new Dictionary<string, object>() { { "id", Id } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
    }
}