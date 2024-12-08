using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class HDM_Faq : InfolineTable
    {
        public string name { get; set;}
        public string content { get; set;}
        public Guid? categoryId { get; set;}
        public string videoUrl { get; set;}
    }
}
