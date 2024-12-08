using CarTender.WebProject;
using System;
using System.Linq;
using System.Reflection;

namespace Infoline.Web.Utility
{
    class LanguageSearch
    {

        public ProjectPages[] GetAllPages()
        {
            var projectName = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
            Assembly asm = Assembly.GetAssembly(typeof(MvcApplication));

            var model = asm.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(d => d.ReturnType.Name == "ActionResult")
                .Select(n => new ProjectPages()
                {
                    Controller = n.DeclaringType != null ? n.DeclaringType.Name.Replace("Controller", "") : null,
                    Action = n.Name,
                    Area = n.DeclaringType.Namespace.ToString().Replace(projectName + ".", "").Replace("Areas.", "").Replace(".Controllers", "").Replace("Controllers", ""),
                    Attributes = String.Join(",", n.GetCustomAttributes(true).Select(a => a.GetType().Name.Replace("Attribute", "")))
                })
                .ToArray();
            return model;
        }



    }

    public class ProjectPages
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string Attributes { get; set; }
    }
}
