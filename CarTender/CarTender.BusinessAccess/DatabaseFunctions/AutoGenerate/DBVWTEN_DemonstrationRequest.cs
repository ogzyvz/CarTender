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
    partial class CarTenderDatabase
    {
        /// <summary>
        /// VWTEN_DemonstrationRequest tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWTEN_DemonstrationRequest dizi objesini geri döndürür.</returns>
        public VWTEN_DemonstrationRequest[] GetVWTEN_DemonstrationRequest(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWTEN_DemonstrationRequest>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWTEN_DemonstrationRequest tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWTEN_DemonstrationRequest dizi objesini geri döndürür.</returns>
        public VWTEN_DemonstrationRequest[] GetVWTEN_DemonstrationRequest(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWTEN_DemonstrationRequest>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWTEN_DemonstrationRequest tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWTEN_DemonstrationRequestCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWTEN_DemonstrationRequest").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWTEN_DemonstrationRequest tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWTEN_DemonstrationRequest tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWTEN_DemonstrationRequest Objesini geri döndürür.</returns>
        public VWTEN_DemonstrationRequest GetVWTEN_DemonstrationRequestById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWTEN_DemonstrationRequest>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
