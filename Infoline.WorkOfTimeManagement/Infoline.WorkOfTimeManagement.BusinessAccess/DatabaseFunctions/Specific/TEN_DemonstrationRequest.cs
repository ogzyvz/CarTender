using Infoline.Framework.Database;
using Infoline.WorkOfTimeManagement.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{

    public class SysFileReturn
    {
        public Guid id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string fileGroup { get; set; }
        public string extension { get; set; }
    }

    partial class WorkOfTimeManagementDatabase
    {
        //public ResultStatus InsertSA_DemoDemandDemo(Guid createdby,DbTransaction tran = null)
        //{
        //    using (var db = GetDB(tran))

        //    {
        //        return db.ExecuteNonQuery("Insert Into [dbo].Intranet1999.SA_DemoDemand (createdby, DemandStatus) "+
        //            "values(" + createdby + " , " + 0 + ")");
        //    }
        //}

        public TEN_DemonstrationRequest GetTEN_DemonstrationRequestByMail(string mail)
        {
            using (var db = GetDB())
            {
                return db.Table<TEN_DemonstrationRequest>().Where(a => a.EMail == mail).Execute().FirstOrDefault();
            }
        }

    }

}
