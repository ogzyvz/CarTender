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

    [EnumDefine(typeof(SH_UserRole), "status")]
    public enum EnumSH_UserRoleStatus
    {
        [Description("Aktif")]
        Aktif = 1,
        [Description("Pasif")]
        Pasif = 0
    }

    partial class WorkOfTimeManagementDatabase
    {

        public SH_UserRole[] GetSH_UserRoleByRoleId(Guid roleid)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.roleid == roleid).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public SH_UserRole GetSH_UserRoleByUserIdRoleId(Guid userid, Guid roleid)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.userid == userid && a.roleid == roleid).OrderByDesc(a => a.created).Take(1).Execute().FirstOrDefault();
            }
        }

        public SH_UserRole[] GetSH_UserRoleByUserId(Guid userid)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.userid == userid).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

    }

}
