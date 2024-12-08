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
        /// VWSYS_Files tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSYS_Files dizi objesini geri döndürür.</returns>
        public VWSYS_Files[] GetVWSYS_Files(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSYS_Files>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSYS_Files tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSYS_Files dizi objesini geri döndürür.</returns>
        public VWSYS_Files[] GetVWSYS_Files(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSYS_Files>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSYS_Files tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSYS_FilesCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSYS_Files").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSYS_Files tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSYS_Files tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSYS_Files Objesini geri döndürür.</returns>
        public VWSYS_Files GetVWSYS_FilesById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSYS_Files>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
