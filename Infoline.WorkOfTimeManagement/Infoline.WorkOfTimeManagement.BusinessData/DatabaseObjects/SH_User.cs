using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class SH_User : InfolineTable
    {
        public bool status { get; set;}
        public Guid? city { get; set;}
        public Guid? town { get; set;}
        public string idcode { get; set;}
        public string tckimlikno { get; set;}
        public int? type { get; set;}
        public string loginname { get; set;}
        public string firstname { get; set;}
        public string lastname { get; set;}
        public DateTime? birthday { get; set;}
        public string password { get; set;}
        public string title { get; set;}
        public string email { get; set;}
        public string phone { get; set;}
        public string cellphone { get; set;}
        public string address { get; set;}
    }
}
