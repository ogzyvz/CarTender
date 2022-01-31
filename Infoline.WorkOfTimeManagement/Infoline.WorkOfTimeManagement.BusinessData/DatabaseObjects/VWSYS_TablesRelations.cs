using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTimeManagement.BusinessData
{
    public partial class VWSYS_TablesRelations 
    {
        public string PK_TABLE { get; set;}
        public string FK_TABLE { get; set;}
        public int ORDINAL_POSITION { get; set;}
        public string FK_SCHEMA { get; set;}
        public string FK_COLUMN { get; set;}
        public string PK_SCHEMA { get; set;}
        public string PK_COLUMN { get; set;}
        public string CONSTRAINT_NAME { get; set;}
        public string PRIMARYKEY { get; set;}
    }
}
