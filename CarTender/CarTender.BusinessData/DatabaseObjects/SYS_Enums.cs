using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class SYS_Enums : InfolineTable
    {
        public string Name { get; set;}
        public string tableName { get; set;}
        public string fieldName { get; set;}
        public string enumKey { get; set;}
        public string enumValue { get; set;}
        public string enumDescription { get; set;}
    }
}
