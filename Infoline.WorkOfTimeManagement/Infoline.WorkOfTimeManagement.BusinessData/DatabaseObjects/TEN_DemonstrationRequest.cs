using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class TEN_DemonstrationRequest : InfolineTable
    {
        /// <summary>
        /// Talepte Bulunan Kişinin Adı
        /// </summary>
        public string Name { get; set;}
        /// <summary>
        /// Talepte Bulunan Kişinin Soyadı
        /// </summary>
        public string Surname { get; set;}
        /// <summary>
        /// Talepte Bulunan Kişinin Mail Adresi
        /// </summary>
        public string EMail { get; set;}
        /// <summary>
        /// Talepte Bulunan Kişinin Erişim Numarası
        /// </summary>
        public string Phone { get; set;}
        /// <summary>
        /// Talepte Bulunan Kişinin Şirket Bilgisi
        /// </summary>
        public string CompanyName { get; set;}
    }
}
