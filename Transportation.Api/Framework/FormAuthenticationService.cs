using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Transportation.Api.Framework
{
    public class FormsAuthenticationService : IFormAuthenticationService
    {
        public void SetAuthCookie(string username, bool createPersistenceCookies)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
