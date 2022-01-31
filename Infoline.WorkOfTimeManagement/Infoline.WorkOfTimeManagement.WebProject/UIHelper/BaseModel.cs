using GeoAPI.Geometries;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public enum GridSelectorType
    {
        [Description("radio")]
        Radio = 0,
        [Description("checkbox")]
        Checkbox = 1
    }

    public static class BaseModel
    {
        private static string GetAttributesString(IDictionary<string, object> attributes)
        {
            var res = String.Empty;

            if (attributes == null)
                return res;

            foreach (var attr in attributes)
            {
                res += " " + attr.Key.ToString().ToLower() + "=\"";

                res += attr.Value.GetType().IsAssignableFrom(typeof(bool)) ? attr.Key.ToString().ToLower() : attr.Value;

                res += "\"";
            }

            return res;

        }

        private static IDictionary<string, object> ReplaceAttributes(ref IDictionary<string, object> Keys, Validations.ValidationUI param)
        {

            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var nm = pr.Name.ToString().Replace("_", "-");
                var val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool))
                    ? nm
                    : pr.GetValue(param);

                if (Keys[nm] != null)
                {
                    Keys[nm] = val;
                }
                else
                {
                    Keys.Add(nm, val);
                }

            }

            return Keys;

        }

        public static string AppendAttribute(this string item, string Key, object Value)
        {

            var pat = Key + "=\"(.*?)\"";

            if (Regex.Match(item, pat).Success)
            {
                item = Regex.Replace(item, "" + Key + "=\"(.*?)\"", Key += "=\"" + Value + "\"", RegexOptions.IgnoreCase);
            }
            else
            {
                if (item.StartsWith("<input"))
                {
                    item = item.Insert(6, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<div"))
                {
                    item = item.Insert(4, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<textarea"))
                {
                    item = item.Insert(9, " " + Key + "=\"" + Value + "\"");
                }
                else if (item.StartsWith("<select"))
                {
                    item = item.Insert(7, " " + Key + "=\"" + Value + "\"");
                }

            }

            return item;

        }

        public static string AppendAttributes(this string item, Validations.ValidationUI validate)
        {
            if (validate == null)
                return item;

            var attr = validate.GetType().GetProperties().Where(a => a.GetValue(validate) != null)
                .ToDictionary(
                k => k.Name.ToString().Replace("_", "-").ToLower(),
                v => v.GetValue(validate).GetType().IsAssignableFrom(typeof(bool)) ? v.Name.ToString().Replace("_", "-").ToLower() : v.GetValue(validate));

            return item.AppendAttributes(attr);

        }

        public static string AppendAttributes(this string item, IDictionary<string, object> attributes)
        {

            if (attributes == null)
                return item;

            foreach (var attr in attributes)
            {
                item = item.AppendAttribute(attr.Key.ToString().ToLower(), attr.Value);
            };

            return item;

        }

        public static string AppendAttributesValidation(this string item, Validations.ValidationUI param)
        {

            if (param == null)
                return item;


            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var key = pr.Name.ToString().Replace("_", "-");
                var val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool))
                    ? key
                    : pr.GetValue(param);

                item = item.AppendAttribute(key, val);

            }

            return item;

        }

        public static string AsAttribute(this string item, string attribute)
        {
            return " " + attribute + "=\"" + item + "\"";
        }

        public static string AsAttribute(this bool item, string attribute)
        {
            return " " + attribute + "=\"" + item.ToString().ToLower() + "\"";
        }

        //  Validations To Kendo

        public static DatePickerBuilder Validate(this DatePickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.DatePicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static DropDownListBuilder Validate(this DropDownListBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.DropDownList)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static MvcHtmlString Validate(this MvcHtmlString item, Validations.ValidationUI param)
        {

            var hstr = item.ToHtmlString();

            foreach (var pr in param.GetType().GetProperties().Where(a => a.GetValue(param) != null).ToList())
            {

                var nm = pr.Name.ToString().Replace("_", "-");
                var pat = nm + "=\"(.*?)\"";

                var _val = pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool)) && (bool)pr.GetValue(param) == true
                    ? nm
                    : pr.GetValue(param);

                if (pr.GetValue(param).GetType().IsAssignableFrom(typeof(bool)) && (bool)pr.GetValue(param) == false)
                    continue;

                hstr = AppendAttribute(hstr, nm, _val);

            }

            return new MvcHtmlString(hstr);

        }

        //public static MvcHtmlString DynamicValidation(this MvcHtmlString item, string elems)
        //{

        //    var hstr = item.ToHtmlString();

        //    hstr = Regex.Replace(hstr, "required=\"(.*?)\"", String.Empty).Replace("  ", " ");

        //    elems += ",#" + Regex.Match(hstr, "id=\"(.*?)\"", RegexOptions.IgnoreCase).Value.Replace("id=\"", "").Replace("\"", "");
        //    var ifQuery = "";
        //    foreach (var elem in elems.Split(','))
        //        ifQuery += "+ $('" + elem.Trim() + "').val()";
        //    ifQuery = ifQuery.Substring(2);

        //    hstr += "\n" + "<script type=\"text-javascript\">" + "\n"
        //        + "$(function () {" + "\n"
        //        + "$(document).on('change', '" + elems + "', function(){" + "\n"
        //        + "var run = (" + ifQuery + ") != null && (" + ifQuery + ") != '';" + "\n"
        //        + "if(run == true) { "
        //        + "$('" + elems + "').attr('required','required');"
        //        + " } else { "
        //        + "$('" + elems + "').removeAttr('required');"
        //        + " }"
        //        + "}" + "\n"
        //        + "$('" + elems + "')"
        //        + "})" + "\n"
        //        + "</script>";

        //    return new MvcHtmlString(hstr);

        //}

        public static AutoCompleteBuilder Validate(this AutoCompleteBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.AutoComplete)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static CheckBoxBuilder Validate(this CheckBoxBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.CheckBox)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static ColorPickerBuilder Validate(this ColorPickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.ColorPicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static ComboBoxBuilder Validate(this ComboBoxBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.ComboBox)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<long> Validate(this NumericTextBoxBuilder<long> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<long>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<int> Validate(this NumericTextBoxBuilder<int> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<int>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<short> Validate(this NumericTextBoxBuilder<short> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<short>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static NumericTextBoxBuilder<double> Validate(this NumericTextBoxBuilder<double> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.NumericTextBox<double>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static DateTimePickerBuilder Validate(this DateTimePickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.DateTimePicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static EditorBuilder Validate(this EditorBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.Editor)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static MaskedTextBoxBuilder Validate(this MaskedTextBoxBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.MaskedTextBox)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static MultiSelectBuilder Validate(this MultiSelectBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.MultiSelect)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static RadioButtonBuilder Validate(this RadioButtonBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.RadioButton)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static RangeSliderBuilder<double> Validate(this RangeSliderBuilder<double> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.RangeSlider<double>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static RecurrenceEditorBuilder Validate(this RecurrenceEditorBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.RecurrenceEditor)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static SliderBuilder<double> Validate(this SliderBuilder<double> item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.Slider<double>)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static TimePickerBuilder Validate(this TimePickerBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.TimePicker)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static TimezoneEditorBuilder Validate(this TimezoneEditorBuilder item, Validations.ValidationUI param)
        {
            var hAttr = ((Kendo.Mvc.UI.TimezoneEditor)item).HtmlAttributes;
            ReplaceAttributes(ref hAttr, param);
            return item;
        }

        public static GridBoundColumnBuilder<T> GridSelector<T>(this GridBoundColumnBuilder<T> item, GridSelectorType type) where T : class
        {

            item.Column.Title = String.Empty;
            item.Column.ClientTemplate = "<input type=\"" + type.ToDescription() + "\" data-event=\"GridSelector\" />";
            item.Column.Width = "45px";
            item.Column.Sortable = false;
            item.Column.Filterable = false;

            return item;
        }

        public static GridBoundColumnBuilder<T> DataColumn<T>(this GridBoundColumnBuilder<T> item, Expression<Func<T, object>> expression) where T : class
        {

            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            item.Column.ClientTemplate = item.Column.ClientTemplate.AppendAttribute("data-" + body.Member.Name, "#=data." + body.Member.Name + "#");

            return item;
        }

    }

    public static class Extensions
    {

        public static string DateFormatShort(bool? kendoGrid = false)
        {
            var vl = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern.Replace("d", "dd").Replace("ddd", "dd"); ;

            if (kendoGrid == true)
            {
                return "{0:" + vl + "}";
            }

            return vl;
        }

        public static string DateFormatFull(bool? kendoGrid = false)
        {
            var vl = DateFormatShort() + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern;

            if (kendoGrid == true)
            {
                return "{0:" + vl + "}";
            }

            return vl;
        }

        public static string GetIdCode()
        {
            DateTime d = DateTime.Now;
            return d.Year.ToString() + d.Month.ToString() + d.Day.ToString() + d.Hour.ToString() + d.Minute.ToString() + d.Second.ToString() + d.Millisecond.ToString();
        }

        public static bool IsValidGuid(this string str)
        {
            Guid guid;

            if (String.IsNullOrEmpty(str)) { return false; }

            return Guid.TryParse(str, out guid);
        }

        public static T RemoveGeographies<T>(this T item)
        {

            if (item == null)
                return item;

            foreach (var propertyInfo in item.GetType().GetProperties().Where(a => a.PropertyType.IsAssignableFrom(typeof(IGeometry))).ToList())
            {
                propertyInfo.SetValue(item, null);
            }

            return item;

        }

        public static T[] RemoveGeographies<T>(this T[] items) where T : new()
        {

            if (items == null)
                return items;

            foreach (var item in items)
            {
                foreach (var propertyInfo in item.GetType().GetProperties().Where(a => a.PropertyType.IsAssignableFrom(typeof(IGeometry))).ToList())
                {
                    propertyInfo.SetValue(item, null);
                }
            }

            return items;

        }

        public static List<T> ToObjectList<T>(this IEnumerable<Guid> guidList) where T : new()
        {

            var res = new List<T>();

            foreach (var item in guidList)
            {

                var resItem = new T();
                var props = resItem.GetType();

                try
                {
                    var propId = props.GetProperties().Where(a => a.Name == "id" || a.Name == "Id").FirstOrDefault();
                    propId.SetValue(resItem, item);
                    res.Add(resItem);
                }
                catch (Exception) { }

            }

            return res;

        }

        public static CultureInfo Culture(string lang = "tr-TR")
        {
            return CultureInfo.GetCultureInfo(lang);
        }

        public static string ToDescription(this Enum item)
        {
            MemberInfo[] memberInfos = item.GetType().GetMembers(BindingFlags.Public | BindingFlags.Static);

            var res = memberInfos.Where(a => a.Name == item.ToString()).FirstOrDefault();

            if (res != null)
            {
                return res.GetCustomAttributesData().Select(a => a.ConstructorArguments).FirstOrDefault().FirstOrDefault().Value.ToString();
            }
            else
            {
                return item.ToString();
            }

        }

        public static Guid? ToGuid(this string item)
        {
            Guid TryParse;

            if (Guid.TryParse(item, out TryParse))
            {
                return TryParse;
            }

            return null;

        }

        public static int? ToInt32(this string item)
        {

            Int32 tryParse;

            if (Int32.TryParse(item, NumberStyles.Any, new CultureInfo("tr-TR"), out tryParse))
            {
                return Convert.ToInt32(item);
            }

            return 0;

        }

        public static double ToDouble(this int item)
        {
            double TryParse;

            if (double.TryParse(item.ToString().Replace(".", ","), NumberStyles.Any, new CultureInfo("tr-TR"), out TryParse))
            {
                return Convert.ToDouble(item);
            }

            return default(double);

        }

        public static double? ToDouble(this string item)
        {
            double TryParse;

            if (double.TryParse(item.Replace(".", ","), NumberStyles.Any, new CultureInfo("tr-TR"), out TryParse))
            {
                return TryParse;
            }

            return null;

        }

        public static Int32 ToInt32(this double item)
        {
            Int32 TryParse;

            if (Int32.TryParse(item.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out TryParse) == true)
            {
                return TryParse;
            }

            return default(Int32);

        }

        public static Int32 ToInt32(this float item)
        {
            Int32 TryParse;

            if (Int32.TryParse(item.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out TryParse) == true)
            {
                return TryParse;
            }

            return default(Int32);

        }

    }

}