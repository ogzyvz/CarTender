using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWHDM_Category : InfolineTable
    {
        public string name { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_title { get; set;}
    }
}
