using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using Infoline.Helper;

namespace System.Web.Mvc
{
    public class RadioButtonGroup : FactoryBase
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
        public object _Value { get; set; }
        public object _CheckValue { get; set; }
        public string _Name { get; set; }
        public string _Default { get; set; }
        public bool _Readonly { get; set; }
        public string[] _IconClass { get; set; }

        public string Render()
        {

            var sayi = 0;
            var sb = new StringBuilder();

            sb.AppendLine("<div class=\"radio akilliRadioGrup clearfix\">");

            foreach (var item in Enum.GetValues(_Value.GetType()))
            {

                sb.AppendLine("<input type=\"radio\" name=\"" + this._Name + "\" " + (this._Readonly ? "disabled" : "") + " id=\"" + this._Name + "_" + (int)item +
                              "\" " + (Convert.ToInt16(_CheckValue ?? _Value) == (int)item ? "checked" : "") + " value =\"" +
                              (Int32)item + "\">");


                var tabindex =  "tabindex=\"0\"";
                sb.AppendLine("<label " + tabindex + " class=\"radio-label\" for=\"" + this._Name + "_" + (int)item + "\">");

                if (_IconClass.Length > 0 && _IconClass.Length - 1 >= sayi)
                    sb.AppendLine("<i class=\"" + _IconClass[sayi] + "\"></i> ");

                sb.AppendLine(EnumsProperties.GetDescriptionFromEnumValue((Enum)item) + "</label>");
                sayi++;
            }

            sb.AppendLine("</div>");
            return sb.ToString();

        }

        public RadioButtonGroup(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            this.HtmlAttributes = new Dictionary<string, object>();
            this._Name = "";
            this._IconClass = new string[0];
        }

        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
}