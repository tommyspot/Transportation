using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Transportation.Api.Framework
{
    public class Route
    {
        public MethodInfo Method { get; set; }

        public RestApiResult Invoke(IDictionary<string, object> parameters)
        {
            Type type = Method.DeclaringType;
            object instance = type.Assembly.CreateInstance(type.FullName);            

            List<object> methodParameters = new List<object>();            

            foreach (ParameterInfo parameter in Method.GetParameters())
            {
                methodParameters.Add(parameters[parameter.Name]);
            }

            RestApiResult result = Method.Invoke(instance, methodParameters.ToArray()) as RestApiResult;
          
            return result;
        }

        public HttpVerb Verb { get; set; }

        public string Url { get; set; }
    }
}
