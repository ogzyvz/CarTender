using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSYS_Tables 
    {
        public string TABLE_CATALOG { get; set;}
        public string TABLE_SCHEMA { get; set;}
        public string TABLE_NAME { get; set;}
        public string TABLE_TYPE { get; set;}
        public string TABLE_DESCRIPTION { get; set;}
        public int? TABLE_ROWCOUNT { get; set;}
        public double? TABLE_TOTALSPACEMB { get; set;}
        public double? TABLE_USEDSPACEMB { get; set;}
        public double? TABLE_UNUSEDSPACEMB { get; set;}
    }
}
