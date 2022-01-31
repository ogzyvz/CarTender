using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class SH_UserRole : InfolineTable
    {
        public Guid? userid { get; set;}
        public Guid? roleid { get; set;}
    }
}
