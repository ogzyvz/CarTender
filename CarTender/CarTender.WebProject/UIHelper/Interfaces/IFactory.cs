namespace System.Web.Mvc
{
    public interface IFactory
    {
        ModelMetadata ModelMetadata { get; }
        ViewContext ViewContext { get; }
        ViewDataDictionary ViewData { get; }
    }
}