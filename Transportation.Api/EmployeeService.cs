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

        [Route(HttpVerb.Get, "/employees")]
        public RestApiResult GetAll()
        {
            var employees = ClarityDB.Instance.Employees;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(employees) };
        }

        [Route(HttpVerb.Get, "/employees/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var employees = ClarityDB.Instance.Employees
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(employees) };
        }

        [Route(HttpVerb.Get, "/employees/pageSize/{pageSize}")]
        public RestApiResult GetNumberPage(int pageSize)
        {
            var allRecords = ClarityDB.Instance.Employees.Count();
            int numOfPages = allRecords % pageSize == 0
                ? allRecords / pageSize
                : allRecords / pageSize + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Post, "/employees")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Employee employee = Employee.FromJson(json);
            employee.CreatedDate = DateTime.Now;

            ClarityDB.Instance.Employees.Add(employee);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/employees/{id}")]
        public RestApiResult GetEmployeeByID(long id)
        {
            Employee employee = ClarityDB.Instance.Employees.FirstOrDefault(x => x.ID == id);

            if (employee == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = employee.ToJson() };
        }

        [Route(HttpVerb.Delete, "/employees/{id}")]
        public RestApiResult Delete(long id)
        {
            Employee employee = ClarityDB.Instance.Employees.FirstOrDefault(x => x.ID == id);

            if (employee == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

			employee.IsDeleted = true;
            //ClarityDB.Instance.Employees.Remove(employee);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = employee.ToJson() };
        }

        [Route(HttpVerb.Put, "/employees/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            Employee employee = ClarityDB.Instance.Employees.FirstOrDefault(x => x.ID == id);

            if (employee == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            employee.ApplyJson(json);

            //if (employee.IsInvalid())
            //    return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };

            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
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
