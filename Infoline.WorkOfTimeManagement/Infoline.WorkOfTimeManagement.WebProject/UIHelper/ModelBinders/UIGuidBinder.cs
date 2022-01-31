using System.Collections.Generic;
using System.Linq;

namespace System.Web.Mvc
{
    public class UIGuidBinder : IModelBinder
    {
        public string GetRequiredString(ControllerContext controllerContext, string key)
        {
            if (controllerContext.RouteData.Values[key] != null)
            {
                return controllerContext.RouteData.Values[key].ToString();
            }
            return controllerContext.HttpContext.Request[key];
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var model = bindingContext.ModelMetadata.Container;


            //  Query String
            if (model == null)
            {
                try
                {

                    if (bindingContext.ModelType.IsAssignableFrom(typeof(Guid)))
                    {
                        return (Guid)GetRequiredString(controllerContext, bindingContext.ModelName).ToGuid();
                    }
                    else if (bindingContext.ModelType.IsAssignableFrom(typeof(Guid?)))
                    {
                        return GetRequiredString(controllerContext, bindingContext.ModelName).ToGuid();
                    }
                    else if (bindingContext.ModelType.IsAssignableFrom(typeof(List<Guid>)))
                    {
                        return GetRequiredString(controllerContext, bindingContext.ModelName).Split(',').Select(a => (Guid)a.ToGuid()).ToList();
                    }
                    else if (bindingContext.ModelType.IsAssignableFrom(typeof(List<Guid?>)))
                    {
                        return GetRequiredString(controllerContext, bindingContext.ModelName).Split(',').Select(a => a.ToGuid()).ToList();
                    }
                    else if (bindingContext.ModelType.IsAssignableFrom(typeof(Guid[])))
                    {
                        return GetRequiredString(controllerContext, bindingContext.ModelName).Split(',').Select(a => (Guid)a.ToGuid()).ToArray();
                    }
                    else if (bindingContext.ModelType.IsAssignableFrom(typeof(Guid?[])))
                    {
                        return GetRequiredString(controllerContext, bindingContext.ModelName).Split(',').Select(a => a.ToGuid()).ToArray();
                    }

                }
                catch (Exception ex) { new FeedBack().Error(ex.Message.ToString()); }

                return null;

            }
            else
            {

                var elem = model.GetType().GetProperties().Where(a => a.Name == bindingContext.ModelName).FirstOrDefault();
                try
                {
                    elem.SetValue(model, request[bindingContext.ModelName].ToGuid());
                }
                catch (Exception ex) { new FeedBack().Error(ex.Message.ToString()); }

            }

            return model;
        }
    }
}