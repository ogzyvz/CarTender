﻿using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class VWSH_UserPageReport 
    {
        public int ToplamKullaniciSayisi { get; set;}
        public int OnayliKullaniciSayisi { get; set;}
        public int OnaysizKullaniciSayisi { get; set;}
        public DateTime SonKayitTarihi { get; set;}
    }
}