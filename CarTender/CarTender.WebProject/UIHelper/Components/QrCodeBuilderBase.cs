namespace System.Web.Mvc
{
    public class QrCodeBuilderBase<TQrCode, TQrCodeBuilder> : InputBuilderBase<TQrCode, TQrCodeBuilder>, IHideMembers
       where TQrCode : InputBase
       where TQrCodeBuilder : FactoryBuilderBase<TQrCode, TQrCodeBuilder>
    {
        public QrCodeBuilderBase(TQrCode component) : base(component)
        {
            base.Component = component;
        }

        public TQrCodeBuilder Template(string template)
        {
            return null;
        }

        public TQrCodeBuilder TemplateId(string templateId)
        {
            return null;
        }

        public TQrCodeBuilder Value(string value)
        {
            return null;
        }

    }
}