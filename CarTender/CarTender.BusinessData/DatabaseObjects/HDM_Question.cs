using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class HDM_Question : InfolineTable
    {
        public string fullName { get; set;}
        public string email { get; set;}
        public string content { get; set;}
        public Guid? faqId { get; set;}
        public Guid? userId { get; set;}
        public string companyName { get; set;}
        public string phone { get; set;}
    }
}
