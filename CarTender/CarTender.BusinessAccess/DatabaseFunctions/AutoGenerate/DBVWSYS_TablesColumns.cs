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
        /// VWSYS_TablesColumns tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSYS_TablesColumns dizi objesini geri döndürür.</returns>
        public VWSYS_TablesColumns[] GetVWSYS_TablesColumns(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSYS_TablesColumns>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSYS_TablesColumns tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSYS_TablesColumns dizi objesini geri döndürür.</returns>
        public VWSYS_TablesColumns[] GetVWSYS_TablesColumns(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSYS_TablesColumns>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSYS_TablesColumns tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSYS_TablesColumnsCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSYS_TablesColumns").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSYS_TablesColumns tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSYS_TablesColumns tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSYS_TablesColumns Objesini geri döndürür.</returns>
        public VWSYS_TablesColumns GetVWSYS_TablesColumnsById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSYS_TablesColumns>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
