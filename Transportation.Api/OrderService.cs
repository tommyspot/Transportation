using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Transportation.Api
{
    public class OrderService
    {
        public OrderService() { }

        [Route(HttpVerb.Get, "/orders")]
        public RestApiResult GetAll()
        {
            var orders = ClarityDB.Instance.Orders;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(orders) };
        }

        [Route(HttpVerb.Post, "/orders")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Order order = Order.FromJson(json);
            order.CreatedDate = DateTime.Now;

            ClarityDB.Instance.Orders.Add(order);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/orders/{id}")]
        public RestApiResult GetProductInputByID(long id)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == id);

            if (order == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = order.ToJson() };
        }

        [Route(HttpVerb.Delete, "/orders/{id}")]
        public RestApiResult Delete(long id)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == id);

            if (order == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.Orders.Remove(order);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = order.ToJson() };
        }

        [Route(HttpVerb.Put, "/orders/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == id);

            if (order == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            order.ApplyJson(json);

            //if (employee.IsInvalid())
            //    return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };

            ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        private JArray BuildJsonArray(IEnumerable<Order> orders)
        {
            JArray array = new JArray();

            foreach (Order order in orders)
            {
                array.Add(order.ToJson());
            }

            return array;
        }

    }
}
