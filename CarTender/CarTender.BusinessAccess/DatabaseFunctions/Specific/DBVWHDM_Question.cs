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
using System.ComponentModel;

namespace CarTender.BusinessAccess
{
    partial class WorkOfTimeManagementDatabase
    {
        public int GetAllQuestionCount()
        {
            using (var db = GetDB())
            {
                var data = db.Table<VWHDM_Question>().Count();
                return data;
            }
        }
        public int GetAnsweredQuestion()
        {
            using (var db = GetDB())
            {
                //var data = db.Table<VWHDM_Question>().Select(i => i.answer != null);
                return 1;
            }
        }
        public int GetNotAnsweredQuestion()
        {
            using (var db = GetDB())
            {
                var ans = GetAnsweredQuestion();
                var all = GetAllQuestionCount();
                return all - ans;
            }
        }
        public DateTime? GetLastQuestion()
        {
            using (var db = GetDB())
            {
                //return db.Table<VWHDM_Question>().OrderByDesc(i => i.created)[0];
                var data = db.Table<VWHDM_Question>().OrderByDesc(i => i.created).Execute().FirstOrDefault();
                DateTime? a = data.created;
                return a;
            }
        }
    }
}
