using System.Linq;
using GeoAPI.Geometries;
using Infoline.Framework;

namespace System.Web.Mvc
{
    public class UIGeographyBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            HttpRequestBase request = controllerContext.HttpContext.Request;
            var model = bindingContext.ModelMetadata.Container;

            //  Query String
            if (model == null)
            {
                try
                {
                    return GeometryValidator.MakeValid(GeometryValidator.ReorientObject(new NetTopologySuite.IO.WKTReader().Read(request.Form[bindingContext.ModelName])));
                }
                catch (Exception ex) { new FeedBack().Error(ex.Message); }
            }
            else
            {

                var elem = model.GetType().GetProperties().Where(a => a.Name == bindingContext.ModelName).FirstOrDefault();
                try
                {
                    elem.SetValue(model, GeometryValidator.MakeValid(GeometryValidator.ReorientObject(new NetTopologySuite.IO.WKTReader().Read(request[bindingContext.ModelName]))));
                }
                catch (Exception ex) { new FeedBack().Error(ex.Message.ToString()); }

            }

            return model;

        }

    }
}