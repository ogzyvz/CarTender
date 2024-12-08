using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWSH_UserRole : InfolineTable
    {
        public Guid? userid { get; set;}
        public Guid? roleid { get; set;}
        public bool? status { get; set;}
        public string User_Title { get; set;}
        public string Role_Title { get; set;}
        public string status_Title { get; set;}
    }
}
