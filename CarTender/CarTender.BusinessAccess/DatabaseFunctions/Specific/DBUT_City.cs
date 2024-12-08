using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using CarTender.BusinessData;

namespace CarTender.BusinessAccess
{
    partial class WorkOfTimeManagementDatabase
    {


        public UT_City[] GetUT_CityCustom(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_City>().Select(a => new { a.id, a.NAME, a.CityNumber }).OrderBy(a => a.NAME).Execute()
                    .Select(a => new UT_City { id = a.id, NAME = a.NAME, CityNumber = a.CityNumber }).ToArray();
            }
        }
    }

}
