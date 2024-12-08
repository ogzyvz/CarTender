using CarTender.BusinessData;
using System;
using System.Linq;

namespace CarTender.BusinessAccess
{
    partial class CarTenderDatabase
    {
        public SH_User GetSH_UserByTicketid(Guid ticketid)
        {
            var user = GetSH_TicketById(ticketid);
            if (user != null)
            {
                using (var db = GetDB())
                {
                    return db.Table<SH_User>().Where(a => a.id == user.userid).Take(1).Execute().FirstOrDefault();
                }
            }
            else
                return null;
        }

        public SH_User GetSHUserById(Guid? id)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.id == id).OrderByDesc(a => a.created).Take(1).Execute().FirstOrDefault();
            }
        }
        public SH_User GetSH_UserByTckimlikNo(string tckimlikno)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.tckimlikno == tckimlikno).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}
