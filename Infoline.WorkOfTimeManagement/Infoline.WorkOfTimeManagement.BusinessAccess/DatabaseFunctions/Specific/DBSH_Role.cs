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


        public bool GetSH_RoleControl(string rolName, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                var data = db.Table<SH_Role>().Where(x => x.rolname == rolName).Execute().FirstOrDefault();
                return data == null ? true : false;
            }
        }

        public bool GetSH_RoleUpdateControl(Guid id, string rolName, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                var data = db.Table<SH_Role>().Where(x => x.rolname == rolName && x.id != id).Execute().FirstOrDefault();
                return data == null ? true : false;
            }
        }
    }

}
