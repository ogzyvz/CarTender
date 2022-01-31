using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class TEN_Tenant : InfolineTable
    {
        public int? DBType { get; set;}
        public string DBIp { get; set;}
        public int? DBPort { get; set;}
        public string DBCatalog { get; set;}
        public string DBUser { get; set;}
        public string DBPassword { get; set;}
        public string WebIp { get; set;}
        public int? WebPort { get; set;}
        public string WebDomain { get; set;}
        public string WebApplicationName { get; set;}
        public string ServiceIp { get; set;}
        public int? ServicePort { get; set;}
        public string ServiceDomain { get; set;}
        public string ServiceApplicationName { get; set;}
        public string TenantName { get; set;}
        public int? TenantCode { get; set;}
        public DateTime? TenantStartDate { get; set;}
        public DateTime? TenantEndDate { get; set;}
        public string TenantConfig { get; set;}
        public bool? TenantIsPOC { get; set;}
        public string MailHost { get; set;}
        public int? MailPort { get; set;}
        public string MailPassword { get; set;}
        public string MailUser { get; set;}
        public bool? MailSSL { get; set;}
        public string Logo { get; set;}
        public string LogoOther { get; set;}
        public string Favicon { get; set;}
    }
}
