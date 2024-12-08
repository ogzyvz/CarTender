using Kendo.Mvc.UI.Fluent;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Web.Mvc
{
    public class Factory : IHideMembers
    {

        public HtmlHelper HtmlHelper { get; set; }

        public IJSInitializer Initializer { get; set; }

        public Factory(HtmlHelper htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
        }

    }

    public class Factory<TModel> : IHideMembers
    {

        public HtmlHelper<TModel> HtmlHelper { get; set; }

        public IJSInitializer Initializer { get; set; }

        public Factory(HtmlHelper<TModel> htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
        }

        public virtual MapInputBuilder MapInput()
        {
            var comp = new MapInput(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData);
            comp._ReadOnly = false;
            return new MapInputBuilder(comp);
        }

        public virtual MapInputBuilder MapInputFor(Expression<Func<TModel, object>> expression)
        {

            var memberInfo = ParseExpression(expression);

            var comp = new MapInputBuilder(new MapInput(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData));

            comp.Component._Navigation = false;
            comp.Component._Name = memberInfo.Key;
            comp.Component._Id = memberInfo.Key;
            comp.Component._Value = memberInfo.Value != null ? memberInfo.Value.ToString() : String.Empty;
            comp.Component._Navigation = String.IsNullOrEmpty(comp.Component._Value);
            comp.Component._ReadOnly = false;

            return comp;
        }

        public virtual CardBuilder Card()
        {
            return new CardBuilder(new Card(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData));
        }
        public virtual QrCodeBuilder QrCode()
        {
            return new QrCodeBuilder(new QrCode(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData));
        }
        public virtual RadioButtonGroupBuilder RadioButtonGroupFor(Expression<Func<TModel, object>> expression)
        {
            var memberInfo = ParseExpression(expression);
            var comp = new RadioButtonGroupBuilder(new RadioButtonGroup(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData));
            comp.Component._Name = memberInfo.Key;
            comp.Component._CheckValue = memberInfo.Value;
            return comp;
        }

        public virtual FileUploadBuilder FileUpload()
        {
            return new FileUploadBuilder(new FileUploads(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData));
        }

        public virtual CardBuilder CardFor(Expression<Func<TModel, object>> expression)
        {

            var memberInfo = ParseExpression(expression);

            var comp = new CardBuilder(new Card(this.HtmlHelper.ViewContext, this.HtmlHelper.ViewData));

            comp.Component._Text = memberInfo.Key;
            comp.Component._Value = memberInfo.Value != null ? memberInfo.Value.ToString() : String.Empty;

            return comp;

        }

        public virtual GridBuilder<T> Grid<T>(string name) where T : class
        {
            return AkilliGridBuilder.AkilliGrid<T>(this.HtmlHelper, name);
        }

        public virtual DropDownListBuilder DropDownListFor(Expression<Func<TModel, object>> expression)
        {
            var drp = DropDownListCustom.DropDownList(this.HtmlHelper);
            var memberInfo = ParseExpression(expression);
            drp.Name(memberInfo.Key);
            if (memberInfo.Value != null)
            {
                drp.Value(memberInfo.Value.ToString());
            }
            return drp;
        }
        public virtual DropDownListBuilder DropDownList(string Name)
        {
            var drp = DropDownListCustom.DropDownList(this.HtmlHelper);
            drp.Name(Name);
            return drp;
        }


        public virtual NumericTextBoxBuilder<double> NumericTextBoxFor(Expression<Func<TModel, object>> expression)
        {
            var drp = NumericTextBoxCustom.NumericTextBox(this.HtmlHelper);
            var memberInfo = ParseExpression(expression);
            drp.Name(memberInfo.Key);
            if (memberInfo.Value != null)
            {
                drp.Value(Double.Parse(memberInfo.Value.ToString()));
            }
            return drp;
        }
        public virtual NumericTextBoxBuilder<double> NumericTextBox(string Name)
        {
            var drp = NumericTextBoxCustom.NumericTextBox(this.HtmlHelper);
            drp.Name(Name);
            return drp;
        }

        public virtual MultiSelectBuilder MultiSelectFor(Expression<Func<TModel, object>> expression)
        {
            var multiSelect = MultiSelectCustom.MultiSelect(this.HtmlHelper);
            var memberInfo = ParseExpression(expression);
            multiSelect.Name(memberInfo.Key);
            if (memberInfo.Value != null)
            {
                if (memberInfo.Value is IEnumerable)
                {
                    multiSelect.Value(memberInfo.Value as IEnumerable);
                }
                else
                {
                    var list = new List<object>();
                    list.Add(memberInfo.Value);
                    multiSelect.Value(list as IEnumerable);
                }
            }

            return multiSelect;
        }
        public virtual MultiSelectBuilder MultiSelect(string Name)
        {
            var drp = MultiSelectCustom.MultiSelect(this.HtmlHelper);
            drp.Name(Name);
            return drp;
        }
        public virtual DatePickerBuilder DatePickerFor(Expression<Func<TModel, object>> expression)
        {
            var datePicker = DatePickerCustom.DatePicker(this.HtmlHelper);
            var memberInfo = ParseExpression(expression);
            datePicker.Name(memberInfo.Key);

            if (memberInfo.Value != null && memberInfo.Value.Equals(default(DateTime)) == false)
                datePicker.Value((DateTime)memberInfo.Value);

            return datePicker;
        }
        public virtual DatePickerBuilder DatePicker(string Name)
        {
            var control = DatePickerCustom.DatePicker(this.HtmlHelper);
            control.Name(Name);
            return control;
        }


        public virtual DateTimePickerBuilder DateTimePickerFor(Expression<Func<TModel, object>> expression)
        {
            var datePicker = DatePickerCustom.DateTimePicker(this.HtmlHelper);
            var memberInfo = ParseExpression(expression);

            datePicker.Name(memberInfo.Key);
            datePicker.Format(Extensions.DateFormatFull());
            datePicker.Culture(Extensions.Culture().Name);

            if (memberInfo.Value != null && memberInfo.Value.Equals(default(DateTime)) == false)
                datePicker.ToComponent().Value = (DateTime)memberInfo.Value;

            return datePicker;
        }
        public virtual DateTimePickerBuilder DateTimePicker(string Name)
        {
            var control = DatePickerCustom.DateTimePicker(this.HtmlHelper);
            control.Name(Name);
            return control;
        }



        private KeyValue ParseExpression(Expression<Func<TModel, object>> expression)
        {

            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            var obj = ((System.Web.Mvc.WebViewPage)HtmlHelper.ViewDataContainer).Model;

            var path = GetPath(new List<string>(), body);
            path.Reverse();

            var value = GetValue(obj, path);

            //var value = ((System.Web.Mvc.WebViewPage)HtmlHelper.ViewDataContainer).Model.GetType().GetProperty(body.Member.Name)
            //            .GetValue(((System.Web.Mvc.WebViewPage)HtmlHelper.ViewDataContainer).Model);

            return new KeyValue { Key = body.Member.Name, Value = value };

        }

        public List<string> GetPath(List<string> lst, MemberExpression exp)
        {
            if (exp.Expression is ParameterExpression)
            {
                lst.Add(exp.Member.Name.ToString());
                return lst;
            }
            else if (exp.Expression is MemberExpression)
            {
                var memberExp = exp.Expression as MemberExpression;
                lst.Add(exp.Member.Name.ToString());
                return GetPath(lst, memberExp);
            }
            return lst;
        }

        public object GetValue(object obj, List<string> path)
        {
            if (path.Count == 0)
                return null;

            for (var i = 0; i < path.Count; i++)
            {
                if (obj == null)
                    return null;
                obj = obj.GetType().GetProperty(path[i]).GetValue(obj);
            }
            return obj;
        }

        private class KeyValue
        {
            public string Key { get; set; }
            public object Value { get; set; }
        }

    }

}