using System.Collections.Generic;
using System.ComponentModel;

namespace System.Web.Mvc
{
    public class QrCodeBuilder : FactoryBuilderBase<QrCode, QrCodeBuilder>
    {

        public QrCodeBuilder(QrCode component) : base(component)
        {
            this.Component = component;
        }

        public QrCodeBuilder Value(object Value)
        {
            this.Component._Value = Value == null ? "Veri Bulunamadı !" : Value.ToString();
            return this;
        }
        public QrCodeBuilder Name(string Name)
        {
            this.Component._Name = string.IsNullOrEmpty(Name) ? "QR" : Name;
            return this;
        }
        public QrCodeBuilder Logo(string logo)
        {
            this.Component._Logo = logo;
            return this;
        }

        public QrCodeBuilder Size(int size)
        {
            this.Component._Size = size;
            return this;
        }


        public QrCodeBuilder Margin(int margin)
        {
            this.Component._Margin = margin;
            return this;
        }

        public QrCodeBuilder HtmlAttributes(Dictionary<string, object> HtmlAttributes)
        {
            this.Component.HtmlAttributes = HtmlAttributes;
            return this;
        }

        public override void Render()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return this.Component.Render();
        }

    }
}