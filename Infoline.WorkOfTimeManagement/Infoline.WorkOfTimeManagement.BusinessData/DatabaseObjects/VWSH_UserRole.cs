using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSH_UserRole : InfolineTable
    {
        public bool status { get; set;}
        public string User_Title { get; set;}
        public string Role_Title { get; set;}
        public string status_Title { get; set;}
        public Guid? userid { get; set;}
        public Guid? roleid { get; set;}
    }
}
