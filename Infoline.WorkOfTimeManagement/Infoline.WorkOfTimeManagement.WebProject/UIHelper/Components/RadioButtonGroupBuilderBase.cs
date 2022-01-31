namespace System.Web.Mvc
{
    public class RadioButtonGroupBuilderBase<TRadioButtonGroup, TRadioButtonGroupBuilder> : InputBuilderBase<TRadioButtonGroup, TRadioButtonGroupBuilder>, IHideMembers
       where TRadioButtonGroup : InputBase
       where TRadioButtonGroupBuilder : FactoryBuilderBase<TRadioButtonGroup, TRadioButtonGroupBuilder>
    {
        public RadioButtonGroupBuilderBase(TRadioButtonGroup component) : base(component)
        {
            base.Component = component;
        }

        public TRadioButtonGroupBuilder Template(string template)
        {
            return null;
        }

        public TRadioButtonGroupBuilder TemplateId(string templateId)
        {
            return null;
        }

        public TRadioButtonGroupBuilder Value(Enum value)
        {
            return null;
        }

    }
}