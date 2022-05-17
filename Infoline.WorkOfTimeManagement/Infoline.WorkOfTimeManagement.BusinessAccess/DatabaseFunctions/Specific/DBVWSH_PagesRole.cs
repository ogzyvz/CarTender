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
using System.ComponentModel;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{


    [EnumDefine(typeof(SH_PagesRole), "Status")]
    public enum EnumSH_PagesRoleStatus
    {
        [Description("Aktif")]
        Aktif = 1,
        [Description("Pasif")]
        Pasif = 0
    }


    partial class WorkOfTimeManagementDatabase
    {



        public SH_PagesRole[] GetSH_PagesRoleByActionId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_PagesRole>().Where(a => a.actionid == id).OrderBy(a => a.created).Execute().ToArray();
            }
        }

        public SH_PagesRole[] GetSH_PagesRoleByRoleId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_PagesRole>().Where(a => a.roleid == id).OrderBy(a => a.created).Execute().ToArray();
            }
        }



    }

}
