using GeoAPI.Geometries;
using Infoline.WorkOfTimeManagement.BusinessData;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{
    public static class BusinessExtensions
    {

        public static T B_EntityDataCopyForMaterial<T, T2>(this T entity, T2 copyData, bool infolineBase = true)
        {

            var model = copyData.GetType().GetProperties();
            if (!infolineBase)
            {
                model = model.Where(prop => !new InfolineTable().GetType().GetProperties().Select(x => x.Name).Contains(prop.Name)).ToArray();
            }

            foreach (var prop in model)
            {
                var value = prop.GetValue(copyData, null);
                if (value == null) continue;

                var isThere = entity.GetType().GetProperties().Where(x => x.Name == prop.Name).FirstOrDefault();
                if (isThere != null)
                {
                    if (prop.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType.GenericTypeArguments[0], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        if (prop.PropertyType == typeof(IGeometry))
                        {
                            isThere.SetValue(entity, value);
                        }
                        else
                        {
                            isThere.SetValue(entity, Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture));
                        }
                    }
                }

            }
            return entity;
        }



        public static string B_GetIdCode()
        {
            DateTime d = DateTime.Now;
            return d.Year.ToString() + d.Month.ToString() + d.Day.ToString() + d.Hour.ToString() + d.Minute.ToString() + d.Second.ToString() + d.Millisecond.ToString();
        }

        public static string B_GetIdCode(string firstValue = null)
        {
            DateTime d = DateTime.Now;
            return firstValue.ToUpper() + d.Year.ToString() + d.Month.ToString() + d.Day.ToString() + d.Hour.ToString() + d.Minute.ToString() + d.Second.ToString() + d.Millisecond.ToString();
        }

        public static string B_GetCode(int length = 8)
        {
            return Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, length);
        }

        public static Guid? B_ToGuid(this string item)
        {
            Guid TryParse;

            if (!String.IsNullOrEmpty(item) && Guid.TryParse(item.Trim(), out TryParse))
            {
                return TryParse;
            }

            return null;

        }

        public static int? B_ToInt32(this string item)
        {

            Int32 tryParse;

            if (!String.IsNullOrEmpty(item) && Int32.TryParse(item.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out tryParse))
            {
                return Convert.ToInt32(item);
            }

            return 0;

        }

        public static T B_RemoveGeographies<T>(this T item)
        {

            if (item == null)
                return item;

            foreach (var propertyInfo in item.GetType().GetProperties().Where(a => a.PropertyType.IsAssignableFrom(typeof(IGeometry))).ToList())
            {
                propertyInfo.SetValue(item, null);
            }

            return item;

        }

        public static T[] B_RemoveGeographies<T>(this T[] items) where T : new()
        {

            if (items == null)
                return items;

            foreach (var item in items)
            {
                foreach (var propertyInfo in item.GetType().GetProperties().Where(a => a.PropertyType.IsAssignableFrom(typeof(IGeometry))).ToList())
                {
                    propertyInfo.SetValue(item, null);
                }
            }

            return items;

        }
        public static string GetStringFromUrl(string url)
        {

            var wrGETURL = WebRequest.Create(url);
            wrGETURL.Method = "GET";
            wrGETURL.Timeout = 35000;
            wrGETURL.ContentType = "APPLICATION/JSON";
            Stream objStream;
            string text;

            try
            {
                objStream = wrGETURL.GetResponse().GetResponseStream();

                using (StreamReader objReader = new StreamReader(objStream))
                {
                    text = objReader.ReadToEnd();
                }

                return text;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }


}
