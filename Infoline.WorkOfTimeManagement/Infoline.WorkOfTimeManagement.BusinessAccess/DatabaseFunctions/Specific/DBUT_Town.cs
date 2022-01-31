using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTimeManagement.BusinessData;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{
    partial class IntranetManagementDatabase
    {

        public UT_Town[] GetUT_TownCustomByCityNumber(int CityNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Town>().Where(a => a.CityNumber == CityNumber).Select(a => new { a.id, a.NAME }).Execute().OrderBy(a => a.NAME)
                   .Select(a => new UT_Town { id = a.id, NAME = a.NAME }).ToArray();

            }
        }

        public UT_Town[] GetUT_TownCustomByCityNumberName(int CityNumber, string filter, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Town>().Where(a => a.CityNumber == CityNumber && a.NAME.Contains(filter)).Select(a => new { a.id, a.NAME }).Execute()
                    .Select(a => new UT_Town { id = a.id, NAME = a.NAME }).OrderBy(a => a.NAME).ToArray();
            }
        }


    }
}
