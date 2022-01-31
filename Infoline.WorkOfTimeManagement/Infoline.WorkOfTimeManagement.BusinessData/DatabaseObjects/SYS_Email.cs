using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    /// <summary>
    /// Sistem tarafından gönderilen email loglarının tutulduğu tablodur.
    /// </summary>
    public partial class SYS_Email : InfolineTable
    {
        /// <summary>
        /// Gönderilme zamanı
        /// </summary>
        public DateTime? SendingTime { get; set;}
        /// <summary>
        /// Gönderilen mail adresleri
        /// </summary>
        public string SendingMail { get; set;}
        /// <summary>
        /// Mail başlığı
        /// </summary>
        public string SendingTitle { get; set;}
        /// <summary>
        /// Gönderilen mail içeriği
        /// </summary>
        public string SendingMessage { get; set;}
        /// <summary>
        /// Gönderilen mail html gövdesi
        /// </summary>
        public bool? SendingIsBodyHtml { get; set;}
        /// <summary>
        /// Mail gönderim sonucu
        /// </summary>
        public string Result { get; set;}
        /// <summary>
        /// Tablo Adı
        /// </summary>
        public string TableName { get; set;}
        /// <summary>
        /// Data Id
        /// </summary>
        public Guid? DataId { get; set;}
    }
}
