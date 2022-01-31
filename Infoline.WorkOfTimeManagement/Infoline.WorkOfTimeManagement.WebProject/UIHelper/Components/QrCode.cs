using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using QRCoder;

namespace System.Web.Mvc
{
    public class QrCode : FactoryBase
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
        public string _Value { get; set; }
        public string _Logo { get; set; }
        public int _Size { get; set; }
        public int _Margin { get; set; }
        public string _Name { get; set; }

        public string Render()
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(this._Value, QRCodeGenerator.ECCLevel.L))
                {
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        var bitmap = qrCode.GetGraphic(20, Color.Black, Color.White, GetIconBitmap(this._Logo));
                        var memoryStream = new MemoryStream();

                        bitmap.Save(memoryStream, Drawing.Imaging.ImageFormat.Jpeg);
                        var div = new TagBuilder("div");
                        div.Attributes.Add("id", this._Name.ToString());
                        div.Attributes.Add("data-role", "qrcode");

                   
                        var img = new TagBuilder("img");
                        img.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(memoryStream.ToArray())));
                        img.Attributes.Add("height", this._Size.ToString());
                        img.Attributes.Add("width", this._Size.ToString());
                        img.Attributes.Add("name", this._Name.ToString());

                        div.InnerHtml += img.ToString();

                        return div.ToString();
                    }
                }
            }
        }

        private static Bitmap GetIconBitmap(string logo)
        {
            Bitmap img = null;
            if (!string.IsNullOrEmpty(logo))
            {
                try
                {
                    img = new Bitmap(HttpContext.Current.Request.MapPath(logo));
                }
                catch (Exception)
                {
                }
            }
            return img;
        }

        public QrCode(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            //  this.ViewData = ViewData;
            //  this.ViewContext = ViewContext;
            this._Value = "";
            this._Name = "";
            this._Logo = null;
            this._Size = 250;
            this._Margin = 0;
            this.HtmlAttributes = new Dictionary<string, object>();
        }

        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
}