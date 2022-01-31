using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSH_Ticket 
    {
        public Guid userid { get; set;}
        public Guid id { get; set;}
        public DateTime? created { get; set;}
        public DateTime? createtime { get; set;}
        public DateTime? endtime { get; set;}
        public string IP { get; set;}
        public string user_Title { get; set;}
    }
}
