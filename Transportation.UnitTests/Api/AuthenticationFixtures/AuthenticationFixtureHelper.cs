using Epinion.Clarity.Api.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Epinion.Clarity.UnitTests.Api.AuthenticationFixtures
{
    public class AuthenticationFixtureHelper
    {
        public static bool CheckMethodHasAuthenticateAttributeAndRolePropertise(Type type, string methodName, Role role)
        {
            return CheckAttributeAuthenticateInMethod(type, methodName) && CheckAttributeAuthenticateInMethodHasRolePropertise(type, methodName, role);
        }

        public static bool CheckTypeHasAuthenticateAttributeAndRolePropertise(Type type, Role role)
        {
            return CheckHasAtrributeAuthenticationInClass(type) && CheckAttributeAuthenticateInMethodHasRolePropertiseInClass(type, role);
        }

        public static bool CheckAttributeAuthenticateInMethod(Type type, string methodName)
        {
            IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes().Any(a => a.GetType() == typeof(RouteAttribute)));
            var method = methods.FirstOrDefault(x => x.Name == methodName);


            CustomAttributeData attribute = method.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthenticateAttribute));
            return attribute != null;
        }

        public static bool CheckHasAtrributeAuthenticationInClass(Type type)
        {
           var customAttribute = type.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthenticateAttribute));
           return customAttribute != null;
        }

        public static bool CheckAttributeAuthenticateInMethodHasRolePropertise(Type type, string methodName, Role role)
        {
            IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes().Any(a => a.GetType() == typeof(RouteAttribute)));
            var method = methods.FirstOrDefault(x => x.Name == methodName);


            CustomAttributeData attribute = method.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthenticateAttribute));
            var argurement = attribute.NamedArguments.FirstOrDefault(x => x.TypedValue.ArgumentType == typeof(Role));

            if (argurement.TypedValue.Value != null)
            {
                return role == (Role)argurement.TypedValue.Value;
            }
            return false;
        }

        public static bool CheckAttributeAuthenticateInMethodHasRolePropertiseInClass(Type type, Role role)
        {
            var customAttribute = type.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthenticateAttribute));
            var argurement = customAttribute.NamedArguments.FirstOrDefault(x => x.TypedValue.ArgumentType == typeof(Role));

            if (argurement.TypedValue.Value != null)
            {
                return role == (Role)argurement.TypedValue.Value;
            }
            return false;
        }
    }
}
