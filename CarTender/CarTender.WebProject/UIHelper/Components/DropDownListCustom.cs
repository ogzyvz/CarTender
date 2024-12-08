using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class DropDownListCustom
    {

        public static DropDownListBuilder DropDownList(HtmlHelper helper)
        {
            var kendo = helper.Kendo().DropDownList().HtmlAttributes(new Dictionary<string, object>()
                    {
                        {"style", "width:100%"},
                        {"class", "form-control"},
                    })
                    .Filter(FilterType.Contains)
                    .OptionLabel("Lütfen Seçimi Yapınız")
                    .DataTextField("Name")
                    .DataValueField("Id")
                    .DataSource(source =>
                    {
                        source
                            .Custom()
                            .Transport(c => c.Read(b => b.Action("DataSource", "General", new { area = string.Empty })))
                            .ServerFiltering(true)
                            .ServerPaging(true)
                            .ServerSorting(true)
                            .Page(1)
                            .PageSize(50)
                            .Type("aspnetmvc-ajax");
                    })
                    .Delay(1000);

            return kendo;
        }

        public static DropDownListBuilder KenoDropDownList(HtmlHelper helper)
        {
            return helper.Kendo().DropDownList().HtmlAttributes(new Dictionary<string, object>()
            {
                {"style", "width:100%"},
                {"class", "form-control"},
            })
            .Filter(FilterType.Contains)
            .OptionLabel("Lütfen Seçimi Yapınız")
            .DataTextField("Name")
            .DataValueField("Id");
        }
    }
}