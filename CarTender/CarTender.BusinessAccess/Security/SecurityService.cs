using CarTender.BusinessAccess;
using CarTender.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using Infoline;

namespace CarTender.Business.Security
{
    [Export(typeof(IService))]
    [ExportMetadata("ServiceType", typeof(ISecurityService))]
    public class SecurityService : ISecurityService
    {
        Timer _tickettimer;
        int _deleteTimeSpan = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
        public SecurityService()
        {
            _tickettimer = new Timer(DeleteTickets, null, _deleteTimeSpan, Timeout.Infinite);
        }
        void DeleteTickets(object state)
        {
            _tickettimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                using (var db = new WorkOfTimeManagementDatabase().GetDB())
                {
                    db.Table<SH_Ticket>().Where(a => a.endtime < DateTime.Now);
                    //db.ExecuteNonQuery("delete  from SH_Ticket  where endtime < {0}", DateTime.Now);
                }
            }
            catch
            {
            }
            _tickettimer.Change(_deleteTimeSpan, Timeout.Infinite);
        }
        public bool ChangePassword(string userid, string password, string newpassword)
        {
            throw new NotImplementedException();
        }

        public LoginResult Login(string loginname, string password, Guid deviceId, string IPAddress)
        {
            var db = new WorkOfTimeManagementDatabase();
            var user = db.GetSH_UserByLoginName(loginname);
            if (user != null)
            {

                if (!string.IsNullOrEmpty(password) && (password == user.password || GetMd5Hash(password) == user.password))
                {
                    var ticketObject = TicketIsLiveControlByUserid(user.id);
                    if (ticketObject != null)
                    {
                        var ctx = new CallContext(ticketObject.id, ticketObject.userid, deviceId, new PropertyBag(), loginname, string.Concat(user.firstname, " ", user.lastname), new string[0], new PropertyBag(), DateTime.Now.AddDays(1));
                        ctx.Activate();
                    }
                    else
                    {
                        var ctx = new CallContext(Guid.NewGuid(), user.id, deviceId, new PropertyBag(), loginname, string.Concat(user.firstname, " ", user.lastname), new string[0], new PropertyBag(), DateTime.Now.AddDays(1));
                        ctx.Activate();
                        using (var d = db.GetDB())
                        {
                            d.ExecuteInsert(new SH_Ticket { id = ctx.TicketId, IP = IPAddress, userid = ctx.UserId, createtime = DateTime.Now, endtime = DateTime.Now.AddMinutes(TicketLife) });
                        }
                    }
                    return LoginResult.OK;
                }
            }
            return LoginResult.InvalidUser;
        }

        private SH_Ticket TicketIsLiveControlByUserid(Guid userId)
        {
            var db = new WorkOfTimeManagementDatabase();
            using (var d = db.GetDB())
            {
                var ticketObject = d.Table<SH_Ticket>().Where(a => a.userid == userId && a.endtime <= DateTime.Now).OrderByDesc(a => a.createtime).Take(1).Execute().FirstOrDefault();
                if (ticketObject != null)
                {
                    if (ticketObject.endtime > DateTime.Now)
                        return ticketObject;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public string GetMd5Hash(string input)
        {
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public bool IsInRole(string userid, string role)
        {
            return true;
        }

        public void SaveTicket(CallContext ctx)
        {
            using (var db = new WorkOfTimeManagementDatabase().GetDB())
            {
                db.ExecuteInsert(new SH_Ticket { id = ctx.TicketId, userid = ctx.UserId, endtime = DateTime.Now.AddMinutes(TicketLife) });
            }
        }

        public CallContext LoadTicket(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            using (var d = db.GetDB())
            {
                var ctx = d.Table<SH_Ticket>().Where(a => a.id == id).Execute().FirstOrDefault();
                if (ctx != null)
                {
                    var user = db.GetSHUserById(ctx.userid);
                    if (user != null)
                    {
                        return new CallContext(
                            id,
                            ctx.userid,
                            Guid.Empty,
                            new PropertyBag(), user.loginname, user.firstname, new string[0], new PropertyBag(), Convert.ToDateTime(ctx.endtime));
                    }
                }
            }
            return null;
        }

        public void DeleteTicket(Guid id)
        {
            using (var db = new WorkOfTimeManagementDatabase().GetDB())
            {
                db.Table<SH_Ticket>().Delete(a => a.id == id);
            }
        }




        public int TicketLife { get; set; }
    }
}

