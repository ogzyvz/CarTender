using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class SH_Pages : InfolineTable
    {
        public string Bread { get; set;}
        public string Description { get; set;}
        public string Action { get; set;}
        public bool? Status { get; set;}
        public string Area { get; set;}
        public string Controller { get; set;}
        public string Method { get; set;}
        public bool? BreadCrumbStatu { get; set;}
        public string ReturnParametre { get; set;}
        public string MethodParametre { get; set;}
    }
}
