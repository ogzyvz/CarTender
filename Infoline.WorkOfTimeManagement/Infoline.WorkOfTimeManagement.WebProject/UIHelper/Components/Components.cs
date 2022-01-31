namespace System.Web.Mvc
{
    public static class Components
    {
        public static Factory<TModel> Akilli<TModel>(this HtmlHelper<TModel> helper)
        {
            return new Factory<TModel>(helper);
        }
   

        public static Factory Akilli(this HtmlHelper helper)
        {
            return new Factory(helper);
        }

    }
}