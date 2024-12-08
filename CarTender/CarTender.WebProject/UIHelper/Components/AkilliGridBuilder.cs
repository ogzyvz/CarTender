using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    public static class AkilliGridBuilder
    {

        public static GridBuilder<T> AkilliGrid<T>(HtmlHelper helper, string name)
            where T : class
        {

            return helper.Kendo().Grid<T>()
                .Selectable(x => x.Mode(GridSelectionMode.Multiple))
                .Name(name)
                .Navigatable()
                .Filterable(filterable =>
                      filterable.Operators(
                          operators =>
                      operators.ForString(str =>
                      str.Clear().Contains("İçeriyor")
                      .StartsWith("İle Başlar")
                      .IsEqualTo("Eşit")
                      .IsNotEqualTo("Eşit Değil")
                      .EndsWith("İle Biter")
                      .DoesNotContain("İçermiyor"))))
                .Sortable()
                .Resizable(a => a.Columns(true))
                .Scrollable(x => x.Height(380))
                .Pageable(x => x.PageSizes(new[] { 5, 10, 25, 50, 100, 250 }).Refresh(true).Messages(m =>
                {
                    m.Refresh("Yenile")
                     .AllPages("Tümünü getir")
                     .Last("Son sayfaya git")
                     .First("İlk sayfaya git")
                     .Previous("Bir önceki sayfaya git")
                     .Next("Bir sonraki sayfaya git")
                     .ItemsPerPage("Sayfa başına ürün");
                }))
                .ToolBar(x =>
                {

                    //  x.Custom().Text("Ekle").HtmlAttributes(new Dictionary<string, object>() { { "data-task", "Insert" }, { "data-modal", "true" }, { "data-target", Url.Action("Insert", "INV_InvoiceMaterialStock", new { area = "INV", Materialid = Model.MaterialId }) } }).Url("#");
                    //  x.Custom().Text("Düzenle").HtmlAttributes(new Dictionary<string, object>() { { "data-task", "Update" }, { "data-modal", "true" }, { "data-target", Url.Action("Update", "INV_InvoiceMaterialStock", new { area = "INV" }) } }).Url("#");
                    //  x.Custom().Text("Detay").HtmlAttributes(new Dictionary<string, object>() { { "data-task", "Detail" }, { "data-modal", "true" }, { "data-target", Url.Action("Detail", "INV_InvoiceMaterialStock", new { area = "INV" }) } }).Url("#");
                    //  x.Custom().Text("Sil").HtmlAttributes(new Dictionary<string, object>() { { "data-task", "Delete" }, { "data-modal", "true" }, { "data-target", Url.Action("Delete", "INV_InvoiceMaterialStock", new { area = "INV" }) } }).Url("#");
                   // x.Pdf().Text("<div data-toggle='tooltip' title='PDF e Aktar'><i class='icon-file-pdf'></i><span class='hidden-md hidden-sm hidden-xs'> PDF'e Aktar</span></div>").HtmlAttributes(new Dictionary<string, object>() { { "type", "button" }, { "class", "hide" } });
                    x.Custom().Text("<div data-toggle='tooltip' title='Sayfayı Excel e Aktar'><i class='icon-file-excel'></i><span class='hidden-md hidden-sm hidden-xs'> Sayfayı Excel'e Aktar</span></div>").HtmlAttributes(new Dictionary<string, object>() { { "type", "button" }, { "data-task", "excel" }, { "data-all", "false" }, { "class", "pull-right" } }).Url("#");
                    x.Custom().Text("<div data-toggle='tooltip' title='Tümünü Excel e Aktar'><i class='icon-file-excel'></i><span class='hidden-md hidden-sm hidden-xs'> Tümünü Excel'e Aktar</span></div>").HtmlAttributes(new Dictionary<string, object>() { { "type", "button" }, { "data-task", "excel" }, { "data-all", "true" }, { "class", "pull-right" } }).Url("#");
                    x.Custom().Text("<div data-toggle='tooltip' title='PDF e Aktar'><i class='icon-file-pdf'></i><span class='hidden-md hidden-sm hidden-xs'> PDF e Aktar</span></div>").HtmlAttributes(new Dictionary<string, object>() { { "type", "button" }, { "data-task", "pdf" }, { "data-all", "false" }, { "class", "pull-right" } }).Url("#");
                    x.Excel().HtmlAttributes(new Dictionary<string, object>() { { "class", "hide" } });
                    x.Pdf().HtmlAttributes(new Dictionary<string, object>() { { "class", "hide" } });
                })
                .Excel(x => x.FileName((helper.ViewData["Title"] + "-" + DateTime.Now) + ".xlsx"))
                .Pdf(x => x.FileName((helper.ViewData["Title"] + "-" + DateTime.Now) + ".pdf").ProxyURL("http://demos.telerik.com/kendo-ui/service/export"))
                .Events(x =>
                {
                    x.ExcelExport("Kendo_ExcelExport");
                    x.DataBound("Kendo_GridLoad");
                    x.Change("Kendo_GridChange");
                })
                .Mobile(MobileMode.Auto);

        }

    }
}
