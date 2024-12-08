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

namespace CarTender.BusinessAccess
{
    partial class WorkOfTimeManagementDatabase
    {
        /// <summary>
        /// VWSH_UserRole tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSH_UserRole dizi objesini geri döndürür.</returns>
        public VWSH_UserRole[] GetVWSH_UserRole(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_UserRole>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSH_UserRole tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSH_UserRole dizi objesini geri döndürür.</returns>
        public VWSH_UserRole[] GetVWSH_UserRole(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSH_UserRole>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSH_UserRole tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSH_UserRoleCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSH_UserRole").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSH_UserRole tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSH_UserRole tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSH_UserRole Objesini geri döndürür.</returns>
        public VWSH_UserRole GetVWSH_UserRoleById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSH_UserRole>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
