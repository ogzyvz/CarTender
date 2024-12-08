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
    partial class CarTenderDatabase
    {
        public SH_Pages[] GetSH_PagesByAction(string Action, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_Pages>().Where(a => a.Action == Action).OrderBy(a => a.created).Execute().ToArray();
            }
        }

        public Guid? GetTableToId(string TableName)
        {
            using (var db = GetDB())
            {
                return db.ExecuteScaler<Guid?>("SELECT TOP 1 id FROM " + TableName + "");
            }
        }

    }

}
