using GeoAPI.Geometries;
using Infoline.Framework.Database;
using CarTender.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarTender.BusinessAccess
{
    partial class CarTenderDatabase
    {
        public ResultStatus InsertCMP_Company(CMP_Company item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<CMP_Company>(item);
            }
        }
    }

    public partial class CMP_Company : InfolineTable
    {
        public string name { get; set; }
        public int? type { get; set; }
    }
}
