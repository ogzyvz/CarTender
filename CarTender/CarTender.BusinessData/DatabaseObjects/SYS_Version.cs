﻿using System;
using GeoAPI.Geometries;

namespace CarTender.BusinessData
{
    public partial class SYS_Version : InfolineTable
    {
        public string versionNumber { get; set;}
        public string versionChange { get; set;}
        public bool? isActive { get; set;}
    }
}