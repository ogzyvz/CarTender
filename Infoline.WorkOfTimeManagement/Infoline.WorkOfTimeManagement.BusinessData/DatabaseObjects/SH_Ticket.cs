using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class SH_Ticket : InfolineTable
    {
        public Guid userid { get; set;}
        public DateTime? createtime { get; set;}
        public DateTime? endtime { get; set;}
        public string IP { get; set;}
    }
}
