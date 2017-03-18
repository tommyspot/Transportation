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

            Customer customer = Customer.FromJson(json);
			customer.CreatedDate = DateTime.Now;
			customer.Code = customer.Area.Replace(" ", "") + "_" + customer.FullName.Replace(" ", "") + "_" + customer.PhoneNo;

			ClarityDB.Instance.Customers.Add(customer);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customer.ToJson() };
        }

		[Route(HttpVerb.Get, "/customers/{id}")]
		public RestApiResult GetCustomerByID(long id)
		{
			Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

			if (customer == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customer.ToJson() };
		}

		[Route(HttpVerb.Delete, "/customers/{id}")]
		public RestApiResult Delete(long id)
		{
			Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

			if (customer == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			ClarityDB.Instance.Customers.Remove(customer);
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customer.ToJson() };
		}

		[Route(HttpVerb.Put, "/customers/{id}")]
		public RestApiResult Update(long id, JObject json)
		{
			Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

			if (customer == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			customer.ApplyJson(json);
			customer.Code = customer.Area.Replace(" ", "") + "_" + customer.FullName.Replace(" ", "") + "_" + customer.PhoneNo;
			customer.CreatedDate = DateTime.Now;
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
		}

		private JArray BuildJsonArray(IEnumerable<Customer> customers)
        {
            JArray array = new JArray();

            foreach (Customer customer in customers)
            {
                array.Add(customer.ToJson());
            }

            return array;
        }

    }
}
