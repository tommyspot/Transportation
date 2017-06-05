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

            AddOrderDetailFromJson(order.ID, json);

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
            //Increase quantity in Inventory when delete Order
            updateInventoryAfterDeleteOrder(id);

            ClarityDB.Instance.Orders.Remove(order);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = order.ToJson() };
        }

        [Route(HttpVerb.Put, "/orders/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == id);

            if (order == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            order.ApplyJson(json);
            UpdateOrderDetailsFromJson(id, json);

            ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }


        private void AddOrderDetailFromJson(long orderID, JObject json)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == orderID);

            var orderDetailJsons = json.Value<JArray>("orderDetails");
            if (orderDetailJsons != null)
            {
                foreach (JObject orderDetailJson in orderDetailJsons)
                {
                    OrderDetail orderDetail = OrderDetail.FromJson(orderDetailJson);
                    orderDetail.CreatedDate = DateTime.Now;
                    orderDetail.OrderID = orderID;
                    order.OrderDetails.Add(orderDetail);
                    //Decrease quantity in Inventory
                    updateInventory(orderDetail.ProductID, orderDetail.Quantity);
                }
                ClarityDB.Instance.SaveChanges();
            }
        }

        private void updateInventory(long productID, long soldQuantity) {
            Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productID);
            inventory.Quantity -= soldQuantity;
        }

        private void updateInventoryAfterDeleteOrder(long orderID)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == orderID);

            if (order != null && order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in order.OrderDetails)
                {
                    Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == orderDetail.ProductID);
                    inventory.Quantity += orderDetail.Quantity;
                }
            }
        }

        private void UpdateOrderDetailsFromJson(long orderID, JObject json)
        {
            DeleteOrderDetails(orderID);
            AddOrderDetailFromJson(orderID, json);
        }

        private void DeleteOrderDetails(long orderID)
        {
            var orderDetails = ClarityDB.Instance.OrderDetails.Where(x => x.OrderID == orderID);
            foreach (OrderDetail orderDetail in orderDetails)
            {
                //update inventory
                Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == orderDetail.ProductID);
                inventory.Quantity += orderDetail.Quantity;
                ClarityDB.Instance.OrderDetails.Remove(orderDetail);
            }
            ClarityDB.Instance.SaveChanges();
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
