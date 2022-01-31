using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSYS_TablesColumns 
    {
        public Guid? id { get; set;}
        public string TABLE_NAME { get; set;}
        public string COLUMN_NAME { get; set;}
        public string COLUMN_DESCRIPTION { get; set;}
        public short? COLUMN_ORDER { get; set;}
        public string COLUMN_TYPE { get; set;}
        public int? COLUMN_MAXIMUMLENGTH { get; set;}
        public string COLUMN_ISNULLABLE { get; set;}
        public string COLUMN_DEFAULT { get; set;}
        public int? COLUMN_LENGTH { get; set;}
    }
}
