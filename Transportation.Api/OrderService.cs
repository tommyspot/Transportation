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
            var orders = ClarityDB.Instance.Orders.Where(x => x.Status);

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(orders) };
        }

        [Route(HttpVerb.Get, "/orders/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize, string search)
        {
            return this.GetPerPageByStatus(pageIndex, pageSize, search, true);
        }

        [Route(HttpVerb.Get, "/orders/numberOfPages")]
        public RestApiResult GetNumberPage(string pageSize, string search)
        {
            return this.GetNumberPageByStatus(pageSize, search, true);
        }

        [Route(HttpVerb.Get, "/deletedOrders")]
        public RestApiResult GetAllDeletedOrders()
        {
            var orders = ClarityDB.Instance.Orders.Where(x => !x.Status);

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(orders) };
        }

        [Route(HttpVerb.Get, "/deletedOrders/page")]
        public RestApiResult GetDeletedOrdersPerPage(string pageIndex, string pageSize, string search)
        {
            return this.GetPerPageByStatus(pageIndex, pageSize, search, false);
        }

        [Route(HttpVerb.Get, "/deletedOrders/numberOfPages")]
        public RestApiResult GetDeletedOrdersNumberPage(string pageSize, string search)
        {
            return this.GetNumberPageByStatus(pageSize, search, false);
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

        [Route(HttpVerb.Put, "/orders/status/{id}")]
        public RestApiResult ChangeDeleteStatus(long id, JObject json)
        {
            Order order = ClarityDB.Instance.Orders.FirstOrDefault(x => x.ID == id);

            if (order == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            order.Status = json.Value<bool>("status");
            ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = order.ToJson() };
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

        private RestApiResult GetPerPageByStatus(string pageIndex, string pageSize, string search, bool status)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var orders = ClarityDB.Instance.Orders
                .Where(x => x.Status == status)
                .Where(x => String.IsNullOrEmpty(search) || x.LicensePlate.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(orders) };
        }

        private RestApiResult GetNumberPageByStatus(string pageSize, string search, bool status)
        {
            int size = Int32.Parse(pageSize);
            var allRecords = ClarityDB.Instance.Orders
                .Where(x => x.Status == status)
                .Where(x => String.IsNullOrEmpty(search) || x.LicensePlate.IndexOf(search) > -1)
                .Count();
            int numOfPages = allRecords % size == 0
                ? allRecords / size
                : allRecords / size + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }
    }
}
