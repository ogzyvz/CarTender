using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWTEN_DemonstrationRequest : InfolineTable
    {
        public string Name { get; set;}
        public string Surname { get; set;}
        public string EMail { get; set;}
        public string Phone { get; set;}
        public string CompanyName { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
