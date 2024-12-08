using Infoline.Framework.Database;
using CarTender;
using CarTender.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CarTender.BusinessAccess
{
	public class VMHDM_FaqModel : VWHDM_Faq
	{
		private WorkOfTimeManagementDatabase db { get; set; }
		private DbTransaction trans { get; set; }

        public VMHDM_FaqModel Load()
		{
			//This adamdan aldıgın data 

			this.db = this.db ?? new WorkOfTimeManagementDatabase(); //baglantı
			var faq = db.GetVWHDM_FaqById(this.id); //adamdan aldıgın datayı idye gore sorgula geri donusunde butun bilgiler gelir

            if (faq != null) //eger bossa degılse dıger kısımları doldurmak adına assagıdakı ıslemı gerceklestırıyoruz
            {
				this.B_EntityDataCopyForMaterial(faq); //modelimizi faq değişkenin göre dolduruyoruz 
            }

			return this; //direk faq değişkenini neden return etmiyoruz zaten aynı değerler ??
		}
        public ResultStatus Delete(DbTransaction transaction = null) //Transaction araştır ?
        {
            db = db ?? new WorkOfTimeManagementDatabase();
            trans = transaction ?? db.BeginTransaction();

            var item = db.GetHDM_FaqById(this.id); //neden View'siz ?? 

            if (item == null)
            {
                return new ResultStatus // nedir
                {
                    result = false,
                    message = "Zaten Silinmiş."
                };
            }


            var dbresult = db.DeleteHDM_Faq(item, trans); //True False'mı donüyo nasıl calısıyor ?

            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Silme İşlemi Başarısız Oldu."
                };
            } //Transaction ?
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Silme İşlemi Başarıyla Gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update() //neden private tipi neden ResultStatus ? 
        {
            db = db ?? new WorkOfTimeManagementDatabase();
            var dbresult = new ResultStatus { result = true };

            dbresult &= db.UpdateHDM_Faq(new HDM_Faq().B_EntityDataCopyForMaterial(this), false, this.trans); // ??

            if (!dbresult.result)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Güncelleme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Güncelleme İşlemi Başarıyla Gerçekleşti."
                };
            }
        }

        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeManagementDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWHDM_FaqById(this.id);
            var res = new ResultStatus { result = true };

            if (data == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                res = Insert();
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                res = Update();
            }
            if (res.result)
            {

                if (transaction == null) trans.Commit();
            }
            else
            {
                if (transaction == null) trans.Rollback();
            }
            return res;
        }

        private ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeManagementDatabase();
            var res = new ResultStatus { result = true };

            var dbresult = db.InsertHDM_Faq(new HDM_Faq().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Oluşturma İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Başarıyla Kaydedildi."
                };
            }
        }

    }
}
