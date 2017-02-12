using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Api.Framework
{
    public interface IFormAuthenticationService
    {
        void SetAuthCookie(string username, bool createPersistenceCookies);
        void SignOut();
    }
}
