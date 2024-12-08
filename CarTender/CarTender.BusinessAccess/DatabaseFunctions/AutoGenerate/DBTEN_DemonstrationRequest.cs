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
        /// TEN_DemonstrationRequest tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>TEN_DemonstrationRequest dizi objesini geri döndürür.</returns>
        public TEN_DemonstrationRequest[] GetTEN_DemonstrationRequest(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<TEN_DemonstrationRequest>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu TEN_DemonstrationRequest dizi objesini geri döndürür.</returns>
        public TEN_DemonstrationRequest[] GetTEN_DemonstrationRequest(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<TEN_DemonstrationRequest>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// TEN_DemonstrationRequest tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetTEN_DemonstrationRequestCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("TEN_DemonstrationRequest").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// TEN_DemonstrationRequest tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">TEN_DemonstrationRequest tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu TEN_DemonstrationRequest Objesini geri döndürür.</returns>
        public TEN_DemonstrationRequest GetTEN_DemonstrationRequestById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<TEN_DemonstrationRequest>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">TEN_DemonstrationRequest Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertTEN_DemonstrationRequest(TEN_DemonstrationRequest item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<TEN_DemonstrationRequest>(item);
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">TEN_DemonstrationRequest Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateTEN_DemonstrationRequest(TEN_DemonstrationRequest item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<TEN_DemonstrationRequest>(item, setNull);
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">TEN_DemonstrationRequest Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteTEN_DemonstrationRequest(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<TEN_DemonstrationRequest>(id);
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">TEN_DemonstrationRequest Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteTEN_DemonstrationRequest(TEN_DemonstrationRequest item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<TEN_DemonstrationRequest>(item);
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">TEN_DemonstrationRequest Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertTEN_DemonstrationRequest(IEnumerable<TEN_DemonstrationRequest> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<TEN_DemonstrationRequest>(item);
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">TEN_DemonstrationRequest Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateTEN_DemonstrationRequest(IEnumerable<TEN_DemonstrationRequest> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<TEN_DemonstrationRequest>(item, setNull);
            }
        }

        /// <summary>
        /// TEN_DemonstrationRequest tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">TEN_DemonstrationRequest Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteTEN_DemonstrationRequest(IEnumerable<TEN_DemonstrationRequest> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<TEN_DemonstrationRequest>(item);
            }
        }

    }
}
