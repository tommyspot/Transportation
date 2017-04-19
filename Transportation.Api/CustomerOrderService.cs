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
using Newtonsoft.Json;

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
			var currentUser = Authentication.GetCurrentUser();
			customerOrder.CreatedUserID = currentUser.ID;
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

        [Route(HttpVerb.Get, "/customerOrdersByDate")]
        public RestApiResult GetCustomerOrderByDate(string date)
        {
            var dateJSON = JsonConvert.DeserializeObject<JObject>(date);
            DateTime fromDate = Convert.ToDateTime(dateJSON.Value<string>("fromDate"));
            DateTime toDate = Convert.ToDateTime(dateJSON.Value<string>("toDate"));

            List<CustomerOrder> customerOrders = ClarityDB.Instance.CustomerOrders.ToList();
            List<CustomerOrder> filteredCustomerOrders = new List<CustomerOrder>();

            foreach (CustomerOrder customerOrder in customerOrders) {
                if (DateTime.Compare(Convert.ToDateTime(customerOrder.DepartDate), fromDate) >= 0 &&
                    DateTime.Compare(Convert.ToDateTime(customerOrder.DepartDate), toDate) <= 0) {
                    filteredCustomerOrders.Add(customerOrder);
                }
            }

            //Grouping, Sum(Quanlity) and Sum(TotalPay)
            filteredCustomerOrders = filteredCustomerOrders.GroupBy(c => c.CustomerArea).Select(x => new CustomerOrder
            {
                CustomerArea = x.First().CustomerArea,
                Quantity = x.Sum(c => c.Quantity),
                TotalPay = x.Sum(c => c.TotalPay)
            }).ToList();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(filteredCustomerOrders) };
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
