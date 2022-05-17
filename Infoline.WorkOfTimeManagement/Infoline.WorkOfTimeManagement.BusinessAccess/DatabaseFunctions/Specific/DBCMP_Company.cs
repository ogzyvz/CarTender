using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.WorkOfTimeManagement.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{
    partial class WorkOfTimeManagementDatabase
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
