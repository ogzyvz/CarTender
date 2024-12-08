using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWSH_RolePageReport 
    {
        public int ToplamSayi { get; set;}
        public string EnFazlaRolSayisi { get; set;}
        public string EnAzRolSayisi { get; set;}
        public DateTime SonKayitTarihi { get; set;}
    }
}
