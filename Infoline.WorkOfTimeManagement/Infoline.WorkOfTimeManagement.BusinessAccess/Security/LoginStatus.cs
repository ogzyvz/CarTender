using Infoline;
using Infoline.WorkOfTimeManagement.BusinessData;
using Infoline.Framework.Database;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{

    public class LoginStatus
    {
        public LoginResult LoginResult { get; set; }
        public Guid ticketid { get; set; }

    }

}
