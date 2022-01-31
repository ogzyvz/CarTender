using Infoline.Framework.Database;
using Infoline.WorkOfTimeManagement.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{

    public class InfolineDatabase : IDisposable
    {
        string _connectionString;
        Database _database;

        public InfolineDatabase(string connectionString, DatabaseType dbType, DbTransaction tran = null)
        {
            _connectionString = connectionString;
            _database = new Database(connectionString, dbType, tran);
        }


        public ResultStatus ExecuteNonQuery(string txt, params object[] parameters)
        {
            return _database.ExecuteNonQuery(txt, parameters);
        }
        public T ExecuteScaler<T>(string txt, params object[] parameters)
        {
            return _database.ExecuteScaler<T>(txt, parameters);
        }
        public IEnumerable<Dictionary<string, object>> ExecuteReader(string txt, params object[] parameters)
        {
            return _database.ExecuteReader(txt, parameters);
        }
        public IEnumerable<T> ExecuteReader<T>(string txt, params object[] parameters) where T : new()
        {
            return _database.ExecuteReader<T>(txt, parameters);
        }

        public void Dispose()
        {
            _database.Dispose();
        }


        public IGetTable Table(string tableName, string schemaName = "infoline")
        {
            return _database.Table(tableName, schemaName);
        }
        public IGetTable<T> Table<T>(string schemaName = "infoline", string tableName = null) where T : new()
        {
            return _database.Table<T>(schemaName, tableName);
        }
        public ResultStatus CreateTable(TableInfo tableInfo)
        {
            return _database.CreateTable(tableInfo);
        }
        public ITableCreate CreateTable(string tableName, string schemaName = "infoline")
        {
            return _database.CreateTable(tableName, schemaName);
        }
        public ITableCreate<T> CreateTable<T>(string tableName = null, string schemaName = "infoline") where T : InfolineTable
        {
            return _database.CreateTable<T>(tableName, schemaName);
        }
        public TableInfo TableInfo(string tableName, bool onlyColumns = false)
        {
            return _database.TableInfo(tableName, onlyColumns);
        }
        public ResultStatus DropTable(string tableName, string schemaName = "infoline")
        {
            return _database.DropTable(tableName, schemaName);
        }
        public bool TableExists(string tableName, string schemaName = "infoline")
        {
            return _database.TableExists(tableName, schemaName);
        }



        public ResultStatus ExecuteInsert<T>(T parameter, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Insert(parameter);
        }
        public ResultStatus ExecuteUpdate<T>(T parameter, bool setNull = false, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Update(parameter, a => a.id, a => new { a.created, a.id }, setNull);
        }
        public ResultStatus ExecuteDelete<T>(T parameter, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Delete(parameter, a => a.id);
        }
        public ResultStatus ExecuteDelete<T>(Guid id, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Delete(new T { id = id }, a => a.id);
        }
        public ResultStatus ExecuteBulkInsert<T>(IEnumerable<T> parameter, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Insert(parameter);
        }
        public ResultStatus ExecuteBulkUpdate<T>(IEnumerable<T> parametre, bool setNull = false, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Update(parametre, a => a.id, a => new { a.created, a.id }, setNull);
        }
        public ResultStatus ExecuteBulkDelete<T>(IEnumerable<T> parametre, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("ifoline", tableName).Delete(parametre, a => a.id);
        }
        public ResultStatus ExecuteBulkDelete<T>(IEnumerable<Guid> parametre, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>("infoline", tableName).Delete(parametre.Select(a => new T { id = a }), a => a.id);
        }


        public DbTransaction BeginTransection()
        {
            return _database.BeginTransaction();
        }
        public ResultStatus CommitTransection()
        {
            return _database.Commit();
        }
        public ResultStatus RollBackTransection()
        {
            return _database.RollBack();
        }

    }
}
