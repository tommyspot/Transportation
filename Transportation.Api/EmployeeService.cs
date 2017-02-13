using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Transportation.Api
{
    public class EmployeeService
    {

        public EmployeeService() { }

        [Route(HttpVerb.Get, "/employee")]
        public RestApiResult GetAll()
        {
            var employees = ClarityDB.Instance.Employees;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(employees) };
        }

        [Route(HttpVerb.Post, "/employee")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Employee employee = Employee.FromJson(json);

            ClarityDB.Instance.Employees.Add(employee);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        private JArray BuildJsonArray(IEnumerable<Employee> employees)
        {
            JArray array = new JArray();

            foreach (Employee employee in employees)
            {
                array.Add(employee.ToJson());
            }

            return array;
        }

    }
}
