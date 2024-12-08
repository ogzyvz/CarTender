using System.Collections.Generic;
using System.ComponentModel;

namespace System.Web.Mvc
{
    public class CardBuilder : FactoryBuilderBase<Card, CardBuilder>
    {

        public CardBuilder(Card component) : base(component)
        {
            this.Component = component;
        }

        public CardBuilder Value(object Value)
        {
            this.Component._Value = Value == null ? "" : Value.ToString();
            return this;
        }

        public CardBuilder Text(string text)
        {
            this.Component._Text = text;
            return this;
        }

        public CardBuilder IconClass(string iconClass)
        {
            this.Component._IconClass = iconClass;
            return this;
        }

        public CardBuilder Color(string color)
        {
            this.Component._Color = color;
            return this;
        }

        public CardBuilder Color(Card.Colors color)
        {
            this.Component._Color = color.ToString();
            return this;
        }

        public CardBuilder Link(string link)
        {
            this.Component._Link = link;
            return this;
        }

        public CardBuilder GridFilter(string Grid, string Filter)
        {
            this.Component._Grid = Grid;
            this.Component._GridFilter = Filter;
            return this;
        }

        public CardBuilder HtmlAttributes(Dictionary<string, object> HtmlAttributes)
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