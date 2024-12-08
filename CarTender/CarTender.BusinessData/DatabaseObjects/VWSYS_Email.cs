using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWSYS_Email : InfolineTable
    {
        public DateTime? SendingTime { get; set;}
        public string SendingMail { get; set;}
        public string SendingTitle { get; set;}
        public string SendingMessage { get; set;}
        public bool? SendingIsBodyHtml { get; set;}
        public string Result { get; set;}
        public string TableName { get; set;}
        public Guid? DataId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
