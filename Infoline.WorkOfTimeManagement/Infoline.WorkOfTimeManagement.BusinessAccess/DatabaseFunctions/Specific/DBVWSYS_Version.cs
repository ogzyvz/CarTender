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


    partial class WorkOfTimeManagementDatabase
    {

        public VWSYS_Version GetVWSYS_VersionExistByVersionNumber(string versionNumber, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSYS_Version>().Where(a => a.versionNumber == versionNumber).Execute().FirstOrDefault();
            }
        }

    }

}
