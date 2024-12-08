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
	public class VMHDM_CategoryModel : VWHDM_Category
	{
		private CarTenderDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public VMHDM_CategoryModel Load()
		{
			this.db = this.db ?? new CarTenderDatabase();
			var category = db.GetVWHDM_CategoryById(this.id);

			if (category  != null)
			{
				this.B_EntityDataCopyForMaterial(category);
			}

			return this;
		}

        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new CarTenderDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWHDM_CategoryById(this.id);
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
            db = db ?? new CarTenderDatabase();
            var res = new ResultStatus { result = true };

            var dbresult = db.InsertHDM_Category(new HDM_Category().B_EntityDataCopyForMaterial(this), this.trans);
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
                    message = "Oluşturma İşlemi Başarıyla Gerçekleşti."
                };
            }
        }

        private ResultStatus Update()
        {
            db = db ?? new CarTenderDatabase();
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateHDM_Category(new HDM_Category().B_EntityDataCopyForMaterial(this), false, this.trans);
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
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new CarTenderDatabase();
            trans = transaction ?? db.BeginTransaction();
            var item = db.GetHDM_CategoryById(this.id);
            if (item == null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Zaten Silinmiş."
                };
            }


            var dbresult = db.DeleteHDM_Category(item, trans);
           
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Silme İşlemi Başarısız Oldu."
                };
            }
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
    }
}
