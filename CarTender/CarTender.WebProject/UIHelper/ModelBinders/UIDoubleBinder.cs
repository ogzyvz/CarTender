using System.Linq;

namespace System.Web.Mvc
{
    public class UIDoubleBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var model = bindingContext.ModelMetadata.Container;

            //  Query String
            if (model == null)
            {
                try
                {
                    return request[bindingContext.ModelName].ToDouble();
                }
                catch (Exception ex) { new FeedBack().Error(ex.Message.ToString()); }
                return null;
            }
            else
            {

                var elem = model.GetType().GetProperties().Where(a => a.Name == bindingContext.ModelName).FirstOrDefault();
                try
                {
                    elem.SetValue(model, request[bindingContext.ModelName].ToDouble());
                }
                catch (Exception ex) { new FeedBack().Error(ex.Message.ToString()); }

            }

            return model;
        }
    }
}