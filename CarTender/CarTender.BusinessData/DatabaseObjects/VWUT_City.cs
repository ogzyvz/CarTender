using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWUT_City : InfolineTable
    {
        public int? ID_1 { get; set;}
        public string NAME { get; set;}
        public string HASC { get; set;}
        public string VARNAME { get; set;}
        public IGeometry  Polygon { get; set;}
        public int? CityNumber { get; set;}
        public int? RegionNumber { get; set;}
    }
}
