using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class SYS_Files : InfolineTable
    {
        public string DataTable { get; set;}
        public Guid? DataId { get; set;}
        public string FilePath { get; set;}
        /// <summary>
        /// Input ismi verilecek bu alana göre ayrıma varılacak
        /// </summary>
        public string FileGroup { get; set;}
        public string FileExtension { get; set;}
    }
}
