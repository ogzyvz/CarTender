using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{


    public abstract class FactoryBuilderBase<TViewComponent, TBuilder> : IHtmlString, IHideMembers
        where TViewComponent : FactoryBase
        where TBuilder : FactoryBuilderBase<TViewComponent, TBuilder>
    {

        protected internal TViewComponent Component { get; set; }

        public FactoryBuilderBase(TViewComponent component)
        {
            this.Component = component;
        }

        public virtual void Render()
        {
        }

        public virtual string ToHtmlString()
        {
            return "this.Component.Id";
        }

        public static implicit operator TViewComponent(FactoryBuilderBase<TViewComponent, TBuilder> builder) { return null; }

    }
}