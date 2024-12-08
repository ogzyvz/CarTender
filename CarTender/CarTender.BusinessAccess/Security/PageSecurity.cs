using Infoline;
using Infoline.Framework.Database;
using CarTender.BusinessData;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CarTender.BusinessAccess
{

    public class PageSecurity
    {
        public Guid ticketid { get; set; }
        public SH_User user { get; set; }
        public SH_UserRole[] UserRoles { get; set; }
        public VWSH_PagesRole[] PagesRoles { get; set; }
    }

}
