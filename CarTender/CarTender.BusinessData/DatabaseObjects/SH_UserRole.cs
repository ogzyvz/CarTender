﻿using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class SH_UserRole : InfolineTable
    {
        public Guid? userid { get; set;}
        public Guid? roleid { get; set;}
        public bool? status { get; set;}
    }
}