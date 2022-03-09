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
    public partial class IntranetManagementDatabase
    {
        public string ConnectionString { get; private set; }
        public DatabaseType DatabaseType { get; private set; }

        public IntranetManagementDatabase()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"] != null)
                this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

#if DEBUG
            ConnectionString = "Data Source=46.221.52.221;Initial Catalog=IntranetManagement;User ID=developer;Password=InfDev!!**38493--";
#endif

        }

        public IntranetManagementDatabase(string conn)
        {
            this.ConnectionString = conn;
        }

        public InfolineDatabase GetDB(DbTransaction tran = null)
        {
            return new InfolineDatabase(ConnectionString, DatabaseType, tran);
        }

        public DbTransaction BeginTransaction()
        {
            return new InfolineDatabase(ConnectionString, DatabaseType).BeginTransection();
        }
    }

}
