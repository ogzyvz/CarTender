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

    [EnumDefine(typeof(SH_User), "type")]
    public enum EnumSH_UserType
    {
        [Description("Yönetici")]
        Yonetici = 0,
        [Description("Operatör")]
        Operator = 10,
        [Description("Kullanıcı")]
        Kullanıcı = 20
    }

    [EnumDefine(typeof(SH_User), "status")]
    public enum EnumSH_UserStatus
    {
        [Description("Onaylanmış")]
        Onaylanmis = 1,
        [Description("Onay Bekliyor")]
        OnayBekliyor = 0
    }


    partial class WorkOfTimeManagementDatabase
    {

        public SH_User[] GetSH_UserByStatus(EnumSH_UserStatus status, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_User>().Where(a => a.status == Convert.ToBoolean((int)status)).OrderBy(a => a.firstname).Execute().ToArray();
            }
        }

        public SH_User[] GetSH_UserAllAdmins(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_User>().Where(x => x.type == (Int32)EnumSH_UserType.Yonetici).OrderBy(a => a.firstname).Execute().ToArray();
            }
        }



    }

}
