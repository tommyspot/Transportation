using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.UnitTests
{
    public class ApiAssert
    {
        public static void ContainsParameter(IDictionary<string, object> parameters, string name)
        {
            Assert.NotNull(parameters, "parameter dictionary is null");

            foreach (string key in parameters.Keys)
            {
                if (key == name && parameters[key] != null)
                    return;
            }

            Assert.Fail("Parameter not in disctionary");
        }

        public static void ContainsParameter(IDictionary<string, object> parameters, string name, object value)
        {
            Assert.NotNull(parameters, "parameter dictionary is null");

            foreach (string key in parameters.Keys)
            {
                if (key == name && value.Equals(Convert.ChangeType(parameters[key], value.GetType())))
                    return;
            }

            Assert.Fail("Parameter not in disctionary");
        }
    }
}
