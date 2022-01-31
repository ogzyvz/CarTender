namespace System.Web.Mvc
{
    public class InputBuilderBase<TInput, TInputBuilder> : FactoryBuilderBase<TInput, TInputBuilder>, IHideMembers
        where TInput : InputBase
        where TInputBuilder : FactoryBuilderBase<TInput, TInputBuilder>
    {
        public InputBuilderBase(TInput component) : base(component)
        {
        }

    }
}