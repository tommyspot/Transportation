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
    public class CustomerOrderService
    {

        public CustomerOrderService() { }

        [Route(HttpVerb.Get, "/customerOrders")]
        public RestApiResult GetAll()
        {
            var customerOrders = ClarityDB.Instance.CustomerOrders;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(customerOrders) };
        }

        [Route(HttpVerb.Post, "/customerOrders")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            CustomerOrder customerOrder = CustomerOrder.FromJson(json);
			customerOrder.CreatedDate = DateTime.Now;

			ClarityDB.Instance.CustomerOrders.Add(customerOrder);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

		[Route(HttpVerb.Get, "/customerOrders/{id}")]
		public RestApiResult GetCustomerByID(long id)
		{
			CustomerOrder customerOrder = ClarityDB.Instance.CustomerOrders.FirstOrDefault(x => x.ID == id);

			if (customerOrder == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customerOrder.ToJson() };
		}

		[Route(HttpVerb.Delete, "/customerOrders/{id}")]
		public RestApiResult Delete(long id)
		{
			CustomerOrder customerOrder = ClarityDB.Instance.CustomerOrders.FirstOrDefault(x => x.ID == id);

			if (customerOrder == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			ClarityDB.Instance.CustomerOrders.Remove(customerOrder);
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customerOrder.ToJson() };
		}

		[Route(HttpVerb.Put, "/customerOrders/{id}")]
		public RestApiResult Update(long id, JObject json)
		{
			CustomerOrder customerOrder = ClarityDB.Instance.CustomerOrders.FirstOrDefault(x => x.ID == id);

			if (customerOrder == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			customerOrder.ApplyJson(json);
			customerOrder.CreatedDate = DateTime.Now;
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
		}

		private JArray BuildJsonArray(IEnumerable<CustomerOrder> customerOrders)
        {
            JArray array = new JArray();

            foreach (CustomerOrder customerOrder in customerOrders)
            {
                array.Add(customerOrder.ToJson());
            }

            return array;
        }

    }
}
