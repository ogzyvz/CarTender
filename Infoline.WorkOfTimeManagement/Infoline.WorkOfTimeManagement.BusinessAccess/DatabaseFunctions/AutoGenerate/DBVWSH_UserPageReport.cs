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
        /// VWSH_UserPageReport tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSH_UserPageReport dizi objesini geri döndürür.</returns>
        public VWSH_UserPageReport[] GetVWSH_UserPageReport(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_UserPageReport>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSH_UserPageReport tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSH_UserPageReport dizi objesini geri döndürür.</returns>
        public VWSH_UserPageReport[] GetVWSH_UserPageReport(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSH_UserPageReport>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSH_UserPageReport tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSH_UserPageReportCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSH_UserPageReport").Where(conditionExpression).Count();
            }
        }
    }
}
