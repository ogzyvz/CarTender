using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class UT_Town : InfolineTable
    {
        public int? ID_1 { get; set;}
        public int? ID_2 { get; set;}
        public string NAME { get; set;}
        public string HASC { get; set;}
        public string VARNAME { get; set;}
        public IGeometry  Polygon { get; set;}
        public int? CityNumber { get; set;}
    }
}
