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
    public class CustomerService
    {

        public CustomerService() { }

        [Route(HttpVerb.Get, "/customers")]
        public RestApiResult GetAll()
        {
            var customers = ClarityDB.Instance.Customers;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(customers) };
        }

        [Route(HttpVerb.Post, "/customers")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Customer Customer = Customer.FromJson(json);

            ClarityDB.Instance.Customers.Add(Customer);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

		[Route(HttpVerb.Get, "/customers/{id}")]
		public RestApiResult GetCustomerByID(long id)
		{
			Customer Customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

			if (Customer == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = Customer.ToJson() };
		}

		[Route(HttpVerb.Delete, "/customers/{id}")]
		public RestApiResult Delete(long id)
		{
			Customer Customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

			if (Customer == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			ClarityDB.Instance.Customers.Remove(Customer);
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = Customer.ToJson() };
		}

		[Route(HttpVerb.Put, "/customers/{id}")]
		public RestApiResult Update(long id, JObject json)
		{
			Customer Customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

			if (Customer == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			Customer.ApplyJson(json);

			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
		}

		private JArray BuildJsonArray(IEnumerable<Customer> customers)
        {
            JArray array = new JArray();

            foreach (Customer Customer in customers)
            {
                array.Add(Customer.ToJson());
            }

            return array;
        }

    }
}
