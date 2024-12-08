using System.Collections.Generic;
using System.ComponentModel;

namespace System.Web.Mvc
{
    public class RadioButtonGroupBuilder : FactoryBuilderBase<RadioButtonGroup, RadioButtonGroupBuilder>
    {

        public RadioButtonGroupBuilder(RadioButtonGroup component) : base(component)
        {
            this.Component = component;
        }

        public RadioButtonGroupBuilder Value(Enum Value)
        {
            this.Component._Value = Value == null ? null : Value;
            return this;
        }
        public RadioButtonGroupBuilder Readonly(bool Readonly)
        {
            this.Component._Readonly = Readonly;
            return this;
        }

        public RadioButtonGroupBuilder IconClass(string[] Icons)
        {
            this.Component._IconClass = Icons == null ? new string[0] : Icons;
            return this;
        }

        public RadioButtonGroupBuilder HtmlAttributes(Dictionary<string, object> HtmlAttributes)
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