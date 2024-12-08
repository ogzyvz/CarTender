using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class SH_Ticket : InfolineTable
    {
        public Guid userid { get; set;}
        public DateTime? createtime { get; set;}
        public DateTime? endtime { get; set;}
        public string IP { get; set;}
    }
}
