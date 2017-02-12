using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Transportation.Api.Framework
{
    public class Authentication
    {
        public static bool IsAuthenticated()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;

                if (!ticket.Expired && ClarityDB.Instance.Users.Any(x => x.UserName == HttpContext.Current.User.Identity.Name))
                    return true;
            }

            return false;
        }

        public static bool IsAuthenticated(Route route)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;

                var user = ClarityDB.Instance.Users.FirstOrDefault(x => x.UserName == HttpContext.Current.User.Identity.Name);

                if (!ticket.Expired && user != null)
                {
                    if (user.Role == Role.AccountOwner || user.Role == Role.SystemAdmin)
                    {
                        return true;
                    }

                    var classAuthentication = route.Method.DeclaringType.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthenticateAttribute));
                    int classAuthentidationFlag = GetPermission(user, classAuthentication);

                    switch (classAuthentidationFlag)
                    {
                        case 0: return false;
                        case 1: return true;
                        case -1:
                            var methodAuthentication = route.Method.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthenticateAttribute));
                            int methodAuthenticationFlag = GetPermission(user, methodAuthentication);

                            if (methodAuthenticationFlag == 0)
                                return false;

                            return true;
                        default: break;
                    }
                }

            }

            return false;
        }      

        private static int GetPermission(User user, System.Reflection.CustomAttributeData customAttributeData)
        {
            if (customAttributeData != null)
            {
                var argument = customAttributeData.NamedArguments.FirstOrDefault(x => x.MemberName == "Role");
                if (argument != null && argument.TypedValue != null)
                {
                    if (argument.TypedValue.Value != null)
                    {
                        var value = (Role)argument.TypedValue.Value;
                        if (user.Role == value)
                        {
                            return 1;
                        }
                        else if (value == Role.Normal && user.Role == Role.Super || user.Role == Role.PartnerAccount)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            return -1;
        }

        public static User GetCurrentUser()
        {
            return IsAuthenticated()? ClarityDB.Instance.Users.FirstOrDefault(x => x.UserName == HttpContext.Current.User.Identity.Name) : null;
        }

        public static bool RouteRequiresAuthenticate(Route route)
        {
            return route.Method.DeclaringType.CustomAttributes.Any(x => x.AttributeType == typeof(AuthenticateAttribute))
                      || route.Method.CustomAttributes.Any(x => x.AttributeType == typeof(AuthenticateAttribute));
        }
    }
}
