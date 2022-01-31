using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace System.Web.Mvc
{
    public class Card : FactoryBase
    {

        public enum Colors
        {
            red = 0,
            green = 1,
            blue = 2,
            yellow = 3,
            navy = 4,
        }

        public IDictionary<string, object> HtmlAttributes { get; set; }
        public string _Color { get; set; }
        public string _IconClass { get; set; }
        public string _Value { get; set; }
        public string _Text { get; set; }
        public string _Link { get; set; }
        public string _Grid { get; set; }
        public string _GridFilter { get; set; }
        public string _Name { get; set; }

        public string Render()
        {

            var sb = new StringBuilder();

            sb.AppendLine("<div id=\"" + this._Name + "\" class=\"widget style1 " + this._Color.ToString() + "-bg\">");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-xs-2 hidden-md wHeader\">");
            sb.AppendLine("<i class=\"" + this._IconClass + " fa-4x\"></i>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"col-xs-10 text-right wInformation\">");
            sb.AppendLine("<span>" + this._Text + "</span>");
            sb.AppendLine("<h2><span>" + this._Value + "</span><br /></h2>");
            //sb.AppendLine("<span>" + this._Format("{0:dd.MM.yyyy}") + "</span>");
            sb.AppendLine("</div></div></div>");

            if (!String.IsNullOrEmpty(this._Grid))
            {

                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine("    $('#" + this._Name + "').on('click',function(e){ ");
                sb.AppendLine("        e.preventDefault();");
                sb.AppendLine("        $('#" + this._Grid + "').data('kendoGrid').dataSource.filter(" + this._GridFilter + ")");
                sb.AppendLine("        return false;");
                sb.AppendLine("    });");
                sb.AppendLine("</script>");

            }

            return sb.ToString();

        }

        public Card(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            //  this.ViewData = ViewData;
            //  this.ViewContext = ViewContext;

            this._Text = "";
            this._Value = "";
            this._Color = Colors.red.ToString();
            this._Link = "#";
            this._IconClass = "fa fa-comments";
            this._Name = "Card_" + Guid.NewGuid().ToString().Substring(0, 8);

            this.HtmlAttributes = new Dictionary<string, object>();

        }

        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
}