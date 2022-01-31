namespace System.Web.Mvc
{
    public class CardBuilderBase<TCard, TCardBuilder> : InputBuilderBase<TCard, TCardBuilder>, IHideMembers
        where TCard : InputBase
        where TCardBuilder : FactoryBuilderBase<TCard, TCardBuilder>
    {
        public CardBuilderBase(TCard component) : base(component)
        {
            base.Component = component;
        }

        public TCardBuilder Template(string template)
        {
            return null;
        }

        public TCardBuilder TemplateId(string templateId)
        {
            return null;
        }

        public TCardBuilder Value(string value)
        {
            return null;
        }

    }
}