using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSH_PagesRolePageReport 
    {
        public int ToplamSayi { get; set;}
        public int AktifSayisi { get; set;}
        public int PasifSayisi { get; set;}
        public DateTime SonKayitTarihi { get; set;}
    }
}
