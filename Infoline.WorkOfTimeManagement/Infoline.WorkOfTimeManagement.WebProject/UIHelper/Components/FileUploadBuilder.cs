using Infoline.WorkOfTimeManagement.BusinessData;
using Infoline.WorkOfTimeManagement.WebIntranet.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace System.Web.Mvc
{
    public class FileUploadBuilder : FactoryBuilderBase<FileUploads, FileUploadBuilder>
    {
        public FileUploadBuilder(FileUploads component) : base(component)
        {
            this.Component = component;
        }

        public FileUploadBuilder DataKey(Func<FileBase, bool> predicate)
        {
            this.Component._DataKey = new LocalFileProvider(this.Component._DataTable).
                _dataTableFileGroup
                .Where(x => x.Key == this.Component._DataTable)
                .Select(x => x.Value.Where(predicate).FirstOrDefault())
                .Select(x => x.fileGroup).FirstOrDefault();
            return this;
        }

        public FileUploadBuilder DataId(Guid dataId)
        {
            this.Component._DataId = dataId;
            return this;
        }

        public FileUploadBuilder PreviewMode(bool param = false)
        {
            this.Component._PreviewMode = param;
            return this;
        }

        public FileUploadBuilder Validate(bool param = false)
        {
            this.Component._Validate = param;
            return this;
        }

        public FileUploadBuilder AutoSend(bool param = true)
        {
            this.Component._AutoSend = param;
            return this;
        }

        public FileUploadBuilder DataTable(string dataTable)
        {
            this.Component._DataTable = dataTable;
            return this;
        }

        public FileUploadBuilder Value(VWSYS_Files[] files)
        {
            this.Component._Values = files;
            return this;
        }

        public FileUploadBuilder HtmlAttributes(Dictionary<string, object> HtmlAttributes)
        {
            this.Component.HtmlAttributes = HtmlAttributes;
            return this;
        }

        public override void Render()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return this.Component.Render();
        }
    }
}