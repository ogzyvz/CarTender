using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class MultiSelectCustom
    {
        public static MultiSelectBuilder MultiSelect(HtmlHelper helper)
        {
            return helper.Kendo().MultiSelect().HtmlAttributes(new Dictionary<string, object>()
            {
                {"style", "width:100%"},
                {"class", "form-control"},
            })
            .Filter(FilterType.Contains)
            .Placeholder("Lütfen Seçimi Yapınız")
            .DataTextField("Name")
            .DataValueField("Id")
            .AutoClose(false)
            .TagMode(TagMode.Single)
            .DataSource(source =>
            {
                source
                    .Custom()
                    .Transport(c => c.Read(b => b.Action("DataSourceDrop", "General", new {area = string.Empty})))
                    .ServerFiltering(true)
                    .ServerPaging(true)
                    .ServerSorting(true)
                    .Page(1)
                    .PageSize(50)
                    .Type("aspnetmvc-ajax");
            })
            .Delay(1000);
            
        }
    }
}