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
    partial class WorkOfTimeManagementDatabase
    {
        /// <summary>
        /// VWSYS_TablesIndexes tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSYS_TablesIndexes dizi objesini geri döndürür.</returns>
        public VWSYS_TablesIndexes[] GetVWSYS_TablesIndexes(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSYS_TablesIndexes>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSYS_TablesIndexes tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSYS_TablesIndexes dizi objesini geri döndürür.</returns>
        public VWSYS_TablesIndexes[] GetVWSYS_TablesIndexes(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSYS_TablesIndexes>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSYS_TablesIndexes tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSYS_TablesIndexesCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSYS_TablesIndexes").Where(conditionExpression).Count();
            }
        }
    }
}
