using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWSYS_TablesIndexes 
    {
        public string TABLE_NAME { get; set;}
        public string INDEX_NAME { get; set;}
        public string INDEX_TYPE { get; set;}
        public string COLUMN_NAME { get; set;}
    }
}
