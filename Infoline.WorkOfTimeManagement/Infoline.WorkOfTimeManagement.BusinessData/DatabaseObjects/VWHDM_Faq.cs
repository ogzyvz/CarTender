using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWHDM_Faq : InfolineTable
    {
        public string name { get; set;}
        public string content { get; set;}
        public Guid? categoryId { get; set;}
        public string videoUrl { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_title { get; set;}
        public string categoryId_Title { get; set;}
    }
}
