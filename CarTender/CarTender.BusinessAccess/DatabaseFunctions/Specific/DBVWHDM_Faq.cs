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
using System.ComponentModel;

namespace CarTender.BusinessAccess
{
    partial class CarTenderDatabase
    {
        public VWHDM_Faq[] GetVWHDM_FaqByCategoryId(Guid categoryId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_Faq>().Where(a => a.categoryId == categoryId).OrderBy(a => a.created).Execute().ToArray();
            }
        }
        public int GetVWHDM_FaqCountAll()
        {
            using (var db = GetDB(/*tran*/))
            {
                return db.Table<VWHDM_Faq>().Count();
            }
        }
    }
}
