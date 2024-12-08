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
        /// VWHDM_Faq tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWHDM_Faq dizi objesini geri döndürür.</returns>
        public VWHDM_Faq[] GetVWHDM_Faq(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_Faq>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWHDM_Faq tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWHDM_Faq dizi objesini geri döndürür.</returns>
        public VWHDM_Faq[] GetVWHDM_Faq(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWHDM_Faq>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWHDM_Faq tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWHDM_FaqCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWHDM_Faq").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWHDM_Faq tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWHDM_Faq tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWHDM_Faq Objesini geri döndürür.</returns>
        public VWHDM_Faq GetVWHDM_FaqById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWHDM_Faq>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
