using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSH_PagesRole : InfolineTable
    {
        public Guid? actionid { get; set;}
        public Guid? roleid { get; set;}
        public bool? status { get; set;}
        public string Action_Title { get; set;}
        public string Role_Title { get; set;}
        public string status_Title { get; set;}
    }
}
