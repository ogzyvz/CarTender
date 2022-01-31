namespace System.Web.Mvc
{
    public class MapInputBuilderBase<TMapInput, TMapInputBuilder> : InputBuilderBase<TMapInput, TMapInputBuilder>, IHideMembers
        where TMapInput : InputBase
        where TMapInputBuilder : FactoryBuilderBase<TMapInput, TMapInputBuilder>
    {
        public MapInputBuilderBase(TMapInput component) : base(component)
        {
            base.Component = component;
        }

        public TMapInputBuilder Template(string template)
        {
            return null;
        }

        public TMapInputBuilder TemplateId(string templateId)
        {
            return null;
        }

        public TMapInputBuilder Value(string value)
        {
            return null;
        }

    }
}