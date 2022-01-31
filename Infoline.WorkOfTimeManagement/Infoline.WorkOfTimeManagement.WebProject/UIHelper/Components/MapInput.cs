using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;

namespace System.Web.Mvc
{
    public class MapInput : FactoryBase
    {

        public enum CoordinateSystem
        {
            LatLon = 0,
            LonLat = 1
        }

        public enum DrawMode
        {
            Point = 0,
            LineString = 1,
            Polygon = 2
        }

        public IDictionary<string, object> HtmlAttributes { get; set; }
        public string _HelpText { get; set; }
        public bool _Opened { get; set; }
        public bool _Navigation { get; set; }
        public string _Name { get; set; }
        public string _Id { get; set; }
        public bool _ShowRemoveButton { get; set; }
        //public string _TargetInput { get; set; }
        public bool _ReadOnly { get; set; }
        public string _Type { get; set; }
        public string _Value { get; set; }
        public bool _Searchable { get; set; }
        public bool _OnlyMap { get; set; }
        public string _MapImage { get; set; }
        public string IconClass { get; set; }
        public Validations.ValidationUI _Validation { get; set; }
        public string _SlideUpElement { get { return this._Id + "SlideUp"; } }
        private string _ContainerName { get { return this._Id + "Container"; } }
        public string _MapToggleElement { get { return this._Id + "MapToggle"; } }
        public int _Height { get; set; }
        public int _ZoomLevel { get; set; }
        public DrawMode _DrawMode { get; set; }
        public bool _DebugMode { get; set; }

        public string Render()
        {

            var sb = new StringBuilder();
            object outId;
            if (this.HtmlAttributes.TryGetValue("id", out outId))
                this._Id = outId.ToString();

            /*HTMLElement Yaratıldı*/
            sb.AppendLine();
            sb.AppendLine("<div"
                + this._ContainerName.AsAttribute("id")
                + "AkilliMapInput".AsAttribute("data-toggle") + " >");
            sb.AppendLine("<div" + "input-group mapInput".AsAttribute("class") + (this._OnlyMap == true ? "display:none;".AsAttribute("style") : "") + " >");
            sb.AppendLine(("<input"
                + this._Id.AsAttribute("id")
                + this._Name.AsAttribute("name")
                + this._ReadOnly.AsAttribute("readonly")
                + this._Value.AsAttribute("value")
                + "form-control".AsAttribute("class") + " />").AppendAttributes(this.HtmlAttributes).AppendAttributes(this._Validation));
            sb.AppendLine("<span" + "input-group-addon".AsAttribute("class") + " ><i" + this.IconClass.AsAttribute("class") + "></i></span>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div" + this._MapToggleElement.AsAttribute("id") + ("display:none;height: " + this._Height + "px; position: relative;").AsAttribute("style") + "></div>");
            sb.AppendLine("</div>");
            sb.AppendLine();
            /*HTMLElement Yaratıldı*/


            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("  $(function () {");
            sb.AppendLine("    haritalar['" + this._Id + "'] = new AkilliHarita('" + this._MapToggleElement + "', { uiAltlikMini: true, debugmode:" + this._DebugMode.ToString().ToLower() + ", zoom: " + this._ZoomLevel + ",uiSearch: " + this._Searchable.ToString().ToLower() + "});");

            if (!String.IsNullOrEmpty(this._HelpText))
            {
                sb.AppendLine("haritalar['" + this._Id + "'].overlay.setContent('helpTooltip', '" + this._HelpText + "');");
            }

            if (this._ReadOnly == true)
            {
                sb.AppendLine("    haritalar['" + this._Id + "'].layer.addVector('Çizim Layerı', 'DrawLayer');");
            }
            else
            {
                sb.AppendLine("    haritalar['" + this._Id + "'].drawing.create('" + this._DrawMode.ToString() + "',null,false, false, function (e) { $('#" + this._Id + "').val(e.features.item(0) ? e.features.item(0).getSQL() : '').trigger('change'); if(e.event=='Draw'){ $('#" + _MapToggleElement + "').slideUp();}});");
            }

            if (this._Navigation == true)
            {
                if (String.IsNullOrEmpty(this._Value))
                {
                    sb.AppendLine("    GetLocation(function (res) {");
                    sb.AppendLine("      haritalar['" + this._Id + "'].feature.remove('DrawFeature');");
                    sb.AppendLine("      $('#" + this._Id + "').val(res);");
                    sb.AppendLine("      haritalar['" + this._Id + "'].feature.add('DrawLayer', 'DrawFeature', res);");
                    sb.AppendLine("      haritalar['" + this._Id + "'].feature.panTo('DrawFeature');");
                    sb.AppendLine("      $('#" + this._Id + "').trigger('change');");
                    sb.AppendLine("    });");
                }
            }

            var imgInfo = GetImageProps(this._MapImage);
            if (imgInfo.IsImage == true)
            {
                sb.AppendLine("    haritalar['" + this._Id + "'].style.add('" + this._Id + "_Style" + "','#ffffff','#000000',2,'" + this._MapImage + "','" + this._MapImage + "',[" + imgInfo.Anchor[0] + ", " + imgInfo.Anchor[1] + "],1);");
                sb.AppendLine("    haritalar['" + this._Id + "'].layer.get('DrawLayer')['DrawLayer'].setStyle(" + "haritalar['" + this._Id + "'].style.get('" + this._Id + "_Style" + "')['" + this._Id + "_Style" + "']" + ");");
            }

            if (!String.IsNullOrEmpty(this._Value))
            {
                sb.AppendLine("    haritalar['" + this._Id + "'].feature.add('DrawLayer', 'DrawFeature', '" + this._Value + "' )['DrawFeature'];");
                sb.AppendLine("     $('#" + this._Id + "').val('" + this._Value + "')");
            }


            sb.AppendLine("      $('#" + this._ContainerName + "')");
            sb.AppendLine("          .on('click', '.mapInput', function (e) {");
            sb.AppendLine("                $('#" + this._MapToggleElement + "').slideToggle(function () {");
            sb.AppendLine("                  haritalar['" + this._Id + "'].map.updateSize();");
            sb.AppendLine("                  var f = haritalar['" + this._Id + "'].layer.get()['DrawLayer'].getSource().getFeatures()[0];");
            sb.AppendLine("                  if (typeof (f) != 'undefined') {");
            sb.AppendLine("                      haritalar['" + this._Id + "'].map.getView().fit(f.getGeometry().getExtent(), haritalar['" + this._Id + "'].map.getSize());");
            sb.AppendLine("                      haritalar['" + this._Id + "'].map.getView().setZoom(" + this._ZoomLevel + ");");
            sb.AppendLine("                  }");
            sb.AppendLine("                });");
            sb.AppendLine("          });");

            sb.AppendLine("$('#" + this._ContainerName + " .mapInput').trigger(" + (this._Opened ? "'click'" : "'change'") + ")");

            sb.AppendLine("  });");



            sb.AppendLine("</script>");

            return sb.ToString();

        }

        private ImageInfo GetImageProps(string url)
        {

            var res = new ImageInfo();

            try
            {

                url = HttpContext.Current.Server.MapPath(url);

                if (File.Exists(url))
                    res.FileExist = true;
                else
                    res.FileExist = false;

                if (res.FileExist == true)
                {

                    res.FileExtension = Path.GetExtension(url).ToLower();
                    res.Registiry = Registry.ClassesRoot.OpenSubKey(res.FileExtension);

                    if (res.Registiry != null && res.Registiry.GetValue("Content Type") != null)
                        res.IsImage = res.Registiry.GetValue("Content Type").ToString().StartsWith("image/");

                    if (res.IsImage == true)
                    {
                        res.Width = Image.FromFile(url).Width;
                        res.Height = Image.FromFile(url).Height;
                        res.Anchor = new double?[] { Math.Round((double)res.Width / 2), Math.Round((double)res.Height * 1) };
                    }

                }
            }
            catch (Exception ex)
            {
                res.IsImage = false;
            }

            return res;

        }

        public MapInput(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            //  this.ViewData = ViewData;
            //  this.ViewContext = ViewContext;

            this._Value = "";
            this._Height = 250;
            this._Type = "input";
            this._ReadOnly = true;
            this._DebugMode = false;
            this._Searchable = false;
            this._Navigation = true;
            this._ShowRemoveButton = false;
            this._DrawMode = DrawMode.Point;
            //this._CoordinateSystem = CoordinateSystem.LonLat;
            this.IconClass = "icon-location-4";
            this._ZoomLevel = 16;

            this.HtmlAttributes = new Dictionary<string, object>();

            //  this.InputHtmlAttributes = new Dictionary<string, object>();
            //  this.InputGroupHtmlAttributes = new Dictionary<string, object>();
            //  this.ToggleElementHtmlAttributes = new Dictionary<string, object>();

            //  this.InputHtmlAttributes["readonly"] = "readonly";
            //  this.InputGroupHtmlAttributes["data-opened"] = "false";
            //  this.ToggleElementHtmlAttributes["style"] = "display: none;";


        }

        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

        private class ImageInfo
        {
            public bool? FileExist { get; set; }
            public bool? IsImage { get; set; }
            public double? Width { get; set; }
            public double? Height { get; set; }
            public double?[] Anchor { get; set; }
            public string FileExtension { get; set; }
            public RegistryKey Registiry { get; set; }
        }

    }
}