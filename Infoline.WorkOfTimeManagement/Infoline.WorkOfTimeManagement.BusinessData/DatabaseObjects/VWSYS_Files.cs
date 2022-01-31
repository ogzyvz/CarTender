using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSYS_Files : InfolineTable
    {
        public string DataTable { get; set;}
        public Guid? DataId { get; set;}
        public string FilePath { get; set;}
        public string FileGroup { get; set;}
        public string FileExtension { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
