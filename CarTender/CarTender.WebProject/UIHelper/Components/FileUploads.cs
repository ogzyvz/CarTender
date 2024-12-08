using CarTender.BusinessAccess;
using CarTender.BusinessData;
using CarTender.WebIntranet.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace System.Web.Mvc
{
    public class FileUploads : FactoryBase
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
        private int fileUploadCount;
        public bool _PreviewMode { get; set; } = false;
        public bool _Validate { get; set; } = false;
        public string _DataKey { get; set; }
        public Guid? _DataId { get; set; }
        public string _DataTable { get; set; }
        public VWSYS_Files[] _Values { get; set; }
        public bool _AutoSend { get; set; } = false;
        public VWSYS_Files[] SYS_Files()
        {
            var locaFileProvider = new LocalFileProvider(this._DataTable);
            return locaFileProvider.GETSYS_FilesByDataIdAndFileGroup(this._DataId.Value, this._DataKey);
        }
        private string _FileExtension()
        {
            var locaFileProvider = new LocalFileProvider(this._DataTable);
            var c = locaFileProvider._dataTableFileGroup.Where(x => x.Key == this._DataTable).Select(x => x.Value).FirstOrDefault();
            if (c != null)
            {
                var r = c.Where(x => x.fileGroup == this._DataKey).FirstOrDefault();
                if (r != null)
                {
                    fileUploadCount = r.count;
                    return r.fileExtensions;
                }
            }
            return String.Concat((object)string.Empty);
        }
        public string Render()
        {
            var sb = new StringBuilder();
            var fext = this._FileExtension().Replace(" *", "").Replace("*", "");
            var files = _Values ?? this.SYS_Files();
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var hasPreview = true;
            var hasDelete = this._PreviewMode == false;
            var hasInsert = this._PreviewMode == false;

            sb.AppendLine($"<div class=\"fileupload-container\" data-required=\"{_Validate}\" data-extension=\"{fext}\" data-count=\"{fileUploadCount}\" data-insert=\"{hasInsert}\"  data-autosend=\"{this._AutoSend}\" data-id=\"{this._DataId}\" data-table=\"{this._DataTable}\" data-group=\"{this._DataKey}\" data-user=\"{userStatus.user.firstname + " " + userStatus.user.lastname }\">");
            if (hasInsert)
            {
                sb.AppendLine($"<div class=\"drop-container\" style=\"display:none;\"> SÜRÜKLE BIRAK ILE DOSYA YÜKLEME YAPABILIRSINIZ.</div>");
                sb.AppendLine($"<input type=\"text\" class=\"form-control hide\" {(_Validate ? "required" : "")} {(files.Count() > 0 ? "value='dosya var'" : "value=''")} />");
                sb.AppendLine($"<button class=\"btn btn-info btn-block\" data-toggle=\"tooltip\" title=\"Yükleme Limitiniz {fileUploadCount} Adettir.Kabul edilen uzantılar ({fext})\"  data-task=\"upload\" type=\"button\"><i class=\"fa fa-upload\"></i> {this._DataKey} </button>");
            }

            sb.AppendLine($"<ul class=\"fileupload-content\">");
            if (!hasPreview)
            {
                sb.AppendLine($"<li class=\"file-info\"><strong>{this._DataKey}</strong> dosya(lar)ını görüntüleme yetkiniz bulunmamaktadır.</li>");
            }
            else
            {
                var display = files.Count() > 0 ? "none" : "block";
                sb.AppendLine($"<li class=\"file-info col-md-12\" style=\"display:{display}\" ><strong>{this._DataKey}</strong> dosya(lar)ı bulunmamaktadır.{(hasInsert ? "<br/> Dosya yüklemek için yükleme butonunu kullanabilir veya sürükle bırak ile yükleme yapabilirsiz." : "")}</li>");

                foreach (var item in files.OrderBy(a => a.created))
                {
                    try
                    {
                        var fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(item.FilePath));
                        var ext = fileInfo.Extension.Split('.').LastOrDefault().ToLowerInvariant();

                        sb.AppendLine($"<li class=\"file-item col-md-12\" data-url=\"{item.FilePath}\" data-id=\"{item.id}\">");
                        sb.AppendLine($"<div class=\"clearfix file-container\">");
                        sb.AppendLine($"<div class=\"file-image\"><img src=\"/Content/SYS_Files/img/new_icons/{ext}.png\"></div>");
                        sb.AppendLine($"<div class=\"file-desc\">");
                        sb.AppendLine($"<div class=\"col-md-9\" title=\"{fileInfo.Name}\">{fileInfo.Name}</div>");
                        sb.AppendLine($"<div class=\"col-md-3\">");
                        sb.AppendLine($"<div class=\"file-created\">{string.Format("{0:dd.MM.yyyy HH:MM}", item.created)}</div>");
                        sb.AppendLine($"<div class=\"file-owner\">{item.createdby_Title}</div>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"<div class=\"file-button\">");
                        if (hasDelete)
                        {
                            sb.AppendLine($"<button type=\"button\" title=\"Dosya sil\" data-task=\"remove\"  class=\"btn btn-sm btn-danger\"><i class=\"fa fa-remove\"></i></button>");
                        }
                        sb.AppendLine($"<button type=\"button\" title=\"Dosya linkini kopyala\" data-task=\"copy\" class=\"btn btn-sm btn-info\"><i class=\"fa fa-copy\"></i></button>");
                        sb.AppendLine($"<button type=\"button\" title=\"Dosya önizleme\" data-task=\"download\" class=\"btn btn-sm btn-success\"><i class=\"icon-zoom-in\"></i></button>");
                        sb.AppendLine($"<button type=\"button\" title=\"Dosya paylaşım\" data-task=\"mail\" class=\"btn btn-sm btn-warning\"><i class=\"icon-mail-3\"></i></button>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</li>");
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }

            sb.AppendLine($"</ul>");
            sb.AppendLine($"</div>");
            return sb.ToString();
        }
        public FileUploads(ViewContext viewContext, ViewDataDictionary viewData = null) : base(viewContext, viewData)
        {
            this.ViewData = ViewData;
            this.ViewContext = ViewContext;
            this._DataId = default(Guid?);
            this._DataTable = "";
            this.HtmlAttributes = new Dictionary<string, object>();

        }
        protected override void WriteHtml(HtmlTextWriter writer)
        {

        }

    }
    public class FileUploadSave
    {
        public HttpRequestBase Request { get; set; }
        public Guid? DataId { get; set; }
        public string DataTable { get; set; }
        public FileUploadSave(HttpRequestBase request, Guid? DataId = null, string DataTable = null)
        {
            Request = request;
            this.DataId = DataId;
            this.DataTable = DataTable;
        }

        public ResultStatusUI SaveAs()
        {
            var res = new List<SysFileReturn>();
            try
            {

                var errorList = new List<string>();
                var files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var key = files.AllKeys[i];
                    var items = key.Split('|');
                    var table = DataTable ?? items[0];
                    var group = items[1];
                    var dataid = DataId ?? new Guid(items[2]);
                    var fileProvider = new LocalFileProvider(table);
                    res.Add(((SysFileReturn)fileProvider.Import(dataid, group, file).Object));
                }
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message.ToString());
                return new ResultStatusUI() { Result = false, FeedBack = new FeedBack().Warning("İşlem ") };
            }
            return new ResultStatusUI { Result = true, FeedBack = new FeedBack().Success("Başarılı"), Object = res.ToArray() };
        }
    }
}