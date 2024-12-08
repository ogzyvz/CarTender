using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class SH_PagesRole : InfolineTable
    {
        public Guid? actionid { get; set;}
        public Guid? roleid { get; set;}
        public bool? status { get; set;}
    }
}
