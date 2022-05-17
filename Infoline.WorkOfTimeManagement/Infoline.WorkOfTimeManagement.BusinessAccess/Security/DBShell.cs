using Infoline.Framework.Database;
using Infoline.WorkOfTimeManagement.BusinessData;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Infoline;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{
    public partial class WorkOfTimeManagementDatabase
    {
        public SH_User GetUserInfoByUserName(string userName)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.loginname == userName).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public SH_User GetSH_UserInfoByTcNo(string tckimlikno)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.tckimlikno == tckimlikno).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public SH_User GetSH_UserInfoByEmail(string email)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.email == email).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public SH_User GetSH_UserByLoginName(string loginname)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.loginname == loginname).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public SH_PagesRole[] GetSH_PagesRolesRolId(Guid roleid)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_PagesRole>().Where(a => a.roleid == roleid).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public LoginStatus Login(string userName, string password)
        {
            var user = GetUserInfoByUserName(userName);

            if (user != null && !user.status)
            {
                return new LoginStatus { LoginResult = LoginResult.AccountDisabled };
            }

            var res = new LoginStatus();
            if (user != null)
            {
                if (!string.IsNullOrEmpty(password) && GetMd5Hash(GetMd5Hash(password)) == user.password)
                {
                    var ticketid = Guid.NewGuid();
                    using (var d = GetDB())
                    {

                        var ipAdr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                        d.ExecuteInsert(new SH_Ticket
                        {
                            id = ticketid,
                            userid = user.id,
                            createtime = DateTime.Now,
                            endtime = DateTime.Now.AddMinutes(30),
                            IP = ipAdr
                        });
                    }
                    res.LoginResult = LoginResult.OK;
                    res.ticketid = ticketid;
                    return res;
                }
                else
                {
                    res.LoginResult = LoginResult.InvalidPassword;
                }
            }
            else
                res.LoginResult = LoginResult.InvalidUser;

            return res;
        }

        public string GetMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public PageSecurity GetUserPageSecurityByticketid(Guid ticketid)
        {
            if (ticketid != null)
            {
                var pageSecurity = new PageSecurity();
                using (var db = GetDB())
                {
                    var sh_Ticket = db.Table<SH_Ticket>().Where(a => a.id == ticketid).OrderByDesc(a => a.createtime).Take(1).Execute().FirstOrDefault();
                    var sh_UserRoles = db.Table<SH_UserRole>().Where(a => a.userid == sh_Ticket.userid).OrderByDesc(a => a.created).Execute().ToArray();
                    var sh_User = db.Table<SH_User>().Where(a => a.id == sh_Ticket.userid & a.status == true).OrderByDesc(a => a.created).Execute().FirstOrDefault();
                    if (sh_UserRoles.Count() != 0)
                    {
                        var roleIds = sh_UserRoles.Where(a => a.roleid.HasValue).Select(a => a.roleid.Value).ToArray();
                        var pagesRoles = db.Table<VWSH_PagesRole>().Where(a => a.roleid.In(roleIds) && (a.status == true)).OrderByDesc(a => a.created).Execute().ToArray();
                        if (pagesRoles.Count() > 0) { pageSecurity.PagesRoles = pagesRoles; }
                    }
                    pageSecurity.user = sh_User;
                    pageSecurity.ticketid = ticketid;
                    pageSecurity.UserRoles = sh_UserRoles;

                    return pageSecurity;
                }
            }
            return null;
        }
    }
}
