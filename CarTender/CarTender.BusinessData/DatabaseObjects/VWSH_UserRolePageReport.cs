using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWSH_UserRolePageReport 
    {
        public int ToplamSayi { get; set;}
        public int AktifSayisi { get; set;}
        public int PasifSayisi { get; set;}
        public DateTime SonKayitTarihi { get; set;}
    }
}
