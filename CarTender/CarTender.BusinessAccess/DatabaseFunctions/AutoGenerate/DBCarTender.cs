﻿using System;
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
    public partial class CarTenderDatabase
    {
        public string ConnectionString { get; private set; }
        public DatabaseType DatabaseType { get; private set; }

        public CarTenderDatabase()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"] != null)
                this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

#if DEBUG
            ConnectionString = "Data Source=94.73.147.7;Initial Catalog=CarTend;User ID=enucuz;Password=DevUcuz2024!!**";
#endif

        }

        public CarTenderDatabase(string conn)
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
