using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWHDM_Question : InfolineTable
    {
        public string fullName { get; set;}
        public string email { get; set;}
        public string content { get; set;}
        public Guid? faqId { get; set;}
        public Guid? userId { get; set;}
        public string companyName { get; set;}
        public string phone { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_title { get; set;}
        public string faqId_Title { get; set;}
        public string userId_Title { get; set;}
    }
}
